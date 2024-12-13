using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class ObjectType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DocumentsDatum> DocumentsData { get; set; } = new List<DocumentsDatum>();

    public virtual ICollection<MediaDatum> MediaData { get; set; } = new List<MediaDatum>();
}
