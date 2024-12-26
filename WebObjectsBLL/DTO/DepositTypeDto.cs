using MediaLib.DTO;
using System;
using System.Collections.Generic;

namespace WebObjectsBLL.DTO
{
    public class DepositTypeDTO
    {
        public Guid Id { get; set; } // Уникальный идентификатор типа депозита

        public string DepositName { get; set; } = null!; // Название депозита

        public decimal MinimumAmount { get; set; } // Минимальная сумма депозита

        public decimal? MaximumAmount { get; set; } // Максимальная сумма депозита (опционально)

        public string? AdditionalConditions { get; set; } // Дополнительные условия

        public List<DepositTermDTO> DepositTerms { get; set; } = new(); // Список сроков депозита

        public List<DocumentsDTO>? Documents { get; set; } // Список связанных документов
    }
}
