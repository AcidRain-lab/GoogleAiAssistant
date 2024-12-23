using System;
using System.Collections.Generic;

namespace WebObjectsBLL.DTO
{
    public class TermsAndRulesDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<SubTermsAndRulesDto> SubTermsAndRules { get; set; } = new();
    }

    public class SubTermsAndRulesDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<NestedSubTermDto> NestedSubTerms { get; set; } = new();
    }

    public class NestedSubTermDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
