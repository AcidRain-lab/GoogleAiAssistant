using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public partial class Lead
{
    [NotMapped]
    public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();

    [NotMapped]
    public List<MediaDatum> MediaData { get; set; } = new List<MediaDatum>();

    [NotMapped]
    public string Initials
    {
        get
        {
            // Возвращает первые буквы имени и фамилии. Если какое-то из полей пустое, возвращает только доступные инициалы.
            return (!string.IsNullOrEmpty(FirstName) ? FirstName[0].ToString() : "") +
                   (!string.IsNullOrEmpty(LastName) ? LastName[0].ToString() : "");
        }
    }
}
