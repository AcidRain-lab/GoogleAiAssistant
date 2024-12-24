using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IDatabaseMediaContext
    {
        DbSet<Avatar> Avatars { get; set; }
        DbSet<MediaDatum> MediaData { get; set; }
        DbSet<ObjectType> ObjectTypes { get; set; }
        Task<int> SaveChangesAsync();
    }
}
