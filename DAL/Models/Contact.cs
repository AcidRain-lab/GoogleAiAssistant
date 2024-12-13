using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Contact
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public Guid Id { get; set; }

    public string? CompanyName { get; set; }

    public string? CrossReference { get; set; }

    public string? CompanyJobTitle { get; set; }

    public Guid? MailingAddressId { get; set; }

    public Guid? BilllingAddressId { get; set; }

    public Guid? LeadId { get; set; }

    public string? Notes { get; set; }

    public Guid? OwnerId { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Location? BilllingAddress { get; set; }

    public virtual ICollection<ContactsTypesList> ContactsTypesLists { get; set; } = new List<ContactsTypesList>();

    public virtual Location? MailingAddress { get; set; }
}
