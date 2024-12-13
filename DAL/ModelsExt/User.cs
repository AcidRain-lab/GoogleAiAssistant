using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public partial class User
    {
        [NotMapped]
        public bool HasAvatar { get; set; }

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

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        [NotMapped]
        public string Password { get; set; } = null!; // only for create HashPassword from Post

    }
}
