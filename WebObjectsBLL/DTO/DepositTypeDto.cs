using MediaLib.DTO;
using System;
using System.Collections.Generic;

namespace WebObjectsBLL.DTO
{
    public class DepositTypeDTO
    {
        public Guid Id { get; set; }
        public string DepositName { get; set; } = null!;
        public string? Description { get; set; }
        public List<DepositTermDTO> DepositTerms { get; set; } = new();
        public List<DocumentsDTO>? Documents { get; set; }
    }
}