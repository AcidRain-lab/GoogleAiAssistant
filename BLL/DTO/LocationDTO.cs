using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class LocationDTO
    {

        [Required(ErrorMessage = "Please enter a street.")]
        public string Street { get; set; }

        public string SuiteAptUnit { get; set; }

        [Required(ErrorMessage = "Please enter a city.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter a zip code.")]
        public int Zip { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid state.")]
        [Display(Name = "State")]
        public int StateId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid country.")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }


        public LocationDTO() { }

        public LocationDTO(string street, string suiteAptUnit, string city, int zip, int stateId, int countryId)
        {
            Street = street;
            SuiteAptUnit = suiteAptUnit;
            City = city;
            Zip = zip;
            StateId = stateId;
            CountryId = countryId;
        }
    }
}
