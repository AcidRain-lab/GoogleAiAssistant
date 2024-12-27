using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebObjectsBLL.DTO
{

    public class ClientProfileDTO
    {
        public Guid ClientId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PassportData { get; set; }
        public string TaxId { get; set; }
        public bool IsActive { get; set; }

        // Связанные сущности
        public IEnumerable<BankCardDTO> BankCards { get; set; }
        public IEnumerable<CreditDTO> Credits { get; set; }
        public IEnumerable<DepositDTO> Deposits { get; set; }
        public IEnumerable<CashbackDTO> Cashbacks { get; set; }
        public IEnumerable<RegularPaymentDTO> RegularPayments { get; set; }
        public IEnumerable<MessageDTO> Messages { get; set; }
    }
}

