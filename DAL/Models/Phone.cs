using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Phone
{
    public Guid Id { get; set; }

    public string Number { get; set; } = null!;

    public bool? IsPrimary { get; set; }

    public int PhoneTypeId { get; set; }

    public Guid AssociatedRecordId { get; set; }

    public virtual PhoneType PhoneType { get; set; } = null!;
}
