using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class TermsAndRule
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<SubTermsAndRule> SubTermsAndRules { get; set; } = new List<SubTermsAndRule>();
}
