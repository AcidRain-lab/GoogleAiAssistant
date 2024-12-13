using BLL.DTO;
using BLL.DTO.Phones;
using DAL.Repositories;

namespace BLL.Services.Implementations
{
    public class PhoneTypeManager : IPhoneTypeManager
    {
        private readonly IPhoneTypesRepository _phoneTypeRepository;
        public PhoneTypeManager(IPhoneTypesRepository phoneTypesRepository)
        {
            _phoneTypeRepository = phoneTypesRepository;
        }

        public async Task<List<PhoneTypeSummaryDTO>> GetPhoneTypeList()
        {
            var phoneTypeList = await _phoneTypeRepository.GetPhoneTypesList();
            List<PhoneTypeSummaryDTO> phoneTypes = new();
            if (phoneTypeList.Any())
            {
                phoneTypeList.ForEach(phoneType =>
                {
                    phoneTypes.Add(new PhoneTypeSummaryDTO()
                    {
                        PhoneTypeId = phoneType.Id,
                        Name = phoneType.Name,
                    });
                });
            }
            return phoneTypes;
        }
    }
}
