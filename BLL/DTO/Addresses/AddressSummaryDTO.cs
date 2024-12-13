using BLL.Constants;
using BLL.DTO.Countries;
using BLL.DTO.States;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Addresses
{
    public class AddressSummaryDTO
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = ApplicationConstants.CityRequired)]
        public string City { get; set; } = null!;
        [Required(ErrorMessage = ApplicationConstants.StreetRequired)]
        public string Street { get; set; } = null!;
        [Required(ErrorMessage = ApplicationConstants.SuiteAptUnitRequired)]
        public string SuiteAptUnit { get; set; } = null!;
        [Required(ErrorMessage = ApplicationConstants.ZipCodeRequired)]
        public int Zip { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = ApplicationConstants.SelectState)]
        public int StateId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = ApplicationConstants.SelectCountry)]
        public int CountryId { get; set; }
        public List<CountryDTO> CountryList { get; set; } = new() { new CountryDTO { CountryId = 0, Name = ApplicationConstants.CountrySelect } };
        public List<StateDTO> StateList { get; set; } = new() { new StateDTO { StateId = 0, Name = ApplicationConstants.StateSelect } };

    }
}
