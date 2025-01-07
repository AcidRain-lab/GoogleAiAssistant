using System;

namespace WebObjectsBLL.DTO
{
    public class BankAccountDTO
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; } = null!;
        public string AccountName { get; set; } = null!;
        public int BankAccountTypeId { get; set; }
        public Guid ClientId { get; set; }
        public DateTime OpenedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string? ContractTerms { get; set; }
        public decimal Balance { get; set; }
        public int BankCurrencyId { get; set; }
        public bool IsFop { get; set; }

        // Связанные карты
        public List<BankCardDTO> BankCards { get; set; } = new();

        // Информация о клиенте
        public ClientDTO Client { get; set; } = null!;

        // Добавляем свойства для заранее вычисленных данных
        public string ClientFullName { get; set; } = null!;
        public List<string> LinkedCardInfo { get; set; } = new();
    }
}
