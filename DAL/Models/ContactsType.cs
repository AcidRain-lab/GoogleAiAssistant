using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class ContactsType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ContactsTypesList> ContactsTypesLists { get; set; } = new List<ContactsTypesList>();
}
