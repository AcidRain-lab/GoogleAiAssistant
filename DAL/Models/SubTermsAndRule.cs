using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class SubTermsAndRule
{
    public Guid Id { get; set; }

    public Guid TermsAndRulesId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<NestedSubTerm> NestedSubTerms { get; set; } = new List<NestedSubTerm>();

    public virtual TermsAndRule TermsAndRules { get; set; } = null!;
}
