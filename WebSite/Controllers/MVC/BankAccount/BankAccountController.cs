using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebObjectsBLL.DTO;
using WebObjectsBLL.Services;

namespace WebSite.Controllers.MVC.BankAccount
{
    [Authorize]
    public class BankAccountController : Controller
    {
        private readonly BankAccountService _bankAccountService;

        public BankAccountController(BankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        // Отображение списка счетов клиента
        public async Task<IActionResult> Index(Guid clientId)
        {
            if (clientId == Guid.Empty)
                return BadRequest("Invalid client ID.");

            var accounts = await _bankAccountService.GetByClientIdAsync(clientId);
            ViewBag.ClientId = clientId; // Передача clientId для создания новых счетов или возврата
            return View(accounts);
        }

        // Добавление нового счета (GET)
        public IActionResult Add(Guid clientId)
        {
            if (clientId == Guid.Empty)
                return BadRequest("Invalid client ID.");

            var account = new BankAccountDTO
            {
                ClientId = clientId // Связь счета с клиентом
            };
            return View(account);
        }

        // Добавление нового счета (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Guid clientId, int bankAccountTypeId, int currencyId, string accountName)
        {
            if (clientId == Guid.Empty || bankAccountTypeId <= 0 || currencyId <= 0 || string.IsNullOrEmpty(accountName))
            {
                ModelState.AddModelError("", "Invalid data for creating a bank account.");
                return View();
            }

            try
            {
                await _bankAccountService.CreateForClientAsync(clientId, bankAccountTypeId, currencyId, accountName);
                return RedirectToAction(nameof(Index), new { clientId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while creating the account: {ex.Message}");
                return View();
            }
        }

        // Редактирование существующего счета (GET)
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid account ID.");

            var account = await _bankAccountService.GetByIdAsync(id);
            if (account == null)
                return NotFound("Bank account not found.");

            return View(account);
        }

        // Редактирование существующего счета (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BankAccountDTO accountDto)
        {
            if (!ModelState.IsValid)
                return View(accountDto);

            try
            {
                await _bankAccountService.UpdateAsync(accountDto);
                return RedirectToAction(nameof(Index), new { clientId = accountDto.ClientId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while updating the account: {ex.Message}");
                return View(accountDto);
            }
        }

        // Удаление счета
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, Guid clientId)
        {
            if (id == Guid.Empty || clientId == Guid.Empty)
                return BadRequest("Invalid account or client ID.");

            try
            {
                await _bankAccountService.DeleteAsync(id);
                return RedirectToAction(nameof(Index), new { clientId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while deleting the account: {ex.Message}");
                return RedirectToAction(nameof(Index), new { clientId });
            }
        }
    }
}
