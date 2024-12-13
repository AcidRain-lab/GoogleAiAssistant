using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Lead
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int LeadSourceId { get; set; }

    public int JobCategoryId { get; set; }

    public int WorkTypeId { get; set; }

    public Guid Id { get; set; }

    public string? CompanyName { get; set; }

    public string? CrossReference { get; set; }

    public Guid? LocationAddressId { get; set; }

    public Guid? MailingAddressId { get; set; }

    public Guid? BillingAddressId { get; set; }

    public string? Notes { get; set; }

    public int? LeadStatusId { get; set; }

    public int? PriorityId { get; set; }

    public Guid? OwnerId { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public virtual Location? BillingAddress { get; set; }

    public virtual JobCategory JobCategory { get; set; } = null!;

    public virtual LeadSource LeadSource { get; set; } = null!;

    public virtual LeadStatus? LeadStatus { get; set; }

    public virtual ICollection<LeadsTrade> LeadsTrades { get; set; } = new List<LeadsTrade>();

    public virtual Location? LocationAddress { get; set; }

    public virtual Location? MailingAddress { get; set; }

    public virtual Priority? Priority { get; set; }

    public virtual WorkType WorkType { get; set; } = null!;
}
