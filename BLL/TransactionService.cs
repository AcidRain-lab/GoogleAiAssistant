using DAL.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class TransactionService
    {
        private readonly CrmContext _context;

        public TransactionService(CrmContext context)
        {
            _context = context;
        }

        public List<Transaction> GetAllTransactions()
        {
            return _context.Transactions.OrderBy(t => t.Date).ThenBy(t => t.Id).ToList();
        }

        public Transaction AddTransaction(Transaction transaction)
        {
            // Добавляем новую транзакцию
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            // Получаем все транзакции, отсортированные по Id
            var transactions = _context.Transactions.OrderBy(t => t.Id).ToList();

            // Пересчитываем баланс для всех транзакций
            double balance = 0;
            foreach (var trans in transactions)
            {
                if (trans.ActionType == 1) // Приход
                {
                    balance += trans.Quantity;
                }
                else if (trans.ActionType == 2) // Расход
                {
                    balance -= trans.Quantity;
                }
                trans.Balance = balance;
            }

            // Сохраняем изменения в базе данных
            _context.SaveChanges();

            return transaction;
        }


        public Transaction GetTransactionById(int id)
        {
            return _context.Transactions.FirstOrDefault(t => t.Id == id);
        }

        public void UpdateTransaction(Transaction updatedTransaction)
        {
            var existingTransaction = _context.Transactions.FirstOrDefault(t => t.Id == updatedTransaction.Id);
            if (existingTransaction != null)
            {
                // Обновляем поля транзакции
                existingTransaction.Date = updatedTransaction.Date;
                existingTransaction.Quantity = updatedTransaction.Quantity;
                existingTransaction.ActionType = updatedTransaction.ActionType;

                _context.SaveChanges();

                // Пересчитываем баланс начиная с этой транзакции
                RecalculateBalances(existingTransaction.Id);
            }
        }


        public void DeleteTransaction(int id)
        {
            var transaction = _context.Transactions.FirstOrDefault(t => t.Id == id);
            if (transaction != null)
            {
                // Удаляем транзакцию
                _context.Transactions.Remove(transaction);
                _context.SaveChanges();

                // Пересчитываем баланс начиная с этой транзакции
                RecalculateBalances(id);
            }
        }


        private void RecalculateBalances(int startTransactionId)
        {
            // Получаем транзакции, начиная с указанного ID, отсортированные по возрастанию ID
            var transactions = _context.Transactions
                .Where(t => t.Id >= startTransactionId)
                .OrderBy(t => t.Id)
                .ToList();

            // Получаем баланс перед началом пересчета
            double previousBalance = _context.Transactions
                .Where(t => t.Id < startTransactionId)
                .OrderByDescending(t => t.Id)
                .FirstOrDefault()?.Balance ?? 0;

            // Пересчитываем баланс для всех последующих транзакций
            foreach (var transaction in transactions)
            {
                if (transaction.ActionType == 1) // Приход
                {
                    previousBalance += transaction.Quantity;
                }
                else if (transaction.ActionType == 2) // Расход
                {
                    previousBalance -= transaction.Quantity;
                }
                transaction.Balance = previousBalance;
            }

            // Сохраняем изменения в базе данных
            _context.SaveChanges();
        }

    }
}
