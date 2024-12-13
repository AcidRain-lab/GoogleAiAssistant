using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class MediaDatum
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Extension { get; set; } = null!;

    public byte[]? Content { get; set; }

    public Guid AssociatedRecordId { get; set; }

    public bool IsPrime { get; set; }

    public int ObjectTypeId { get; set; }

    public Guid? OwnerId { get; set; }

    public DateTime? CreatedDateTime { get; set; }
}
