using MediaLib.DTO;
using System;

namespace WebObjectsBLL.DTO
{
    public class CreditTypeDTO
    {
        public Guid Id { get; set; }
        public string CreditName { get; set; } = null!;
        public string CreditAmount { get; set; } = null!;
        public string CreditTerm { get; set; } = null!;
        public string? AdditionalConditions { get; set; }
        public List<DocumentsDTO>? Documents { get; set; } // Добавлено поле для документов
    }

}