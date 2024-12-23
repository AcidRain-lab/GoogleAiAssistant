using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class NestedSubTerm
{
    public Guid Id { get; set; }

    public Guid SubTermsAndRulesId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual SubTermsAndRule SubTermsAndRules { get; set; } = null!;
}
