using DAL.Models;

namespace DAL.Repositories
{
    public  interface IPhoneTypesRepository
    {
        Task<List<PhoneType>> GetPhoneTypesList();
    }
}
