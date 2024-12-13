using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Avatar
{
    public Guid AssociatedRecordId { get; set; }

    public string Name { get; set; } = null!;

    public string Extension { get; set; } = null!;

    public byte[] Content { get; set; } = null!;

    public int ObjectTypeId { get; set; }
}
