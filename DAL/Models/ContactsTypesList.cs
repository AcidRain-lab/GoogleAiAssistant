using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class ContactsTypesList
{
    public Guid Id { get; set; }

    public Guid ContactId { get; set; }

    public int ContactTypeId { get; set; }

    public virtual Contact Contact { get; set; } = null!;

    public virtual ContactsType ContactType { get; set; } = null!;
}
