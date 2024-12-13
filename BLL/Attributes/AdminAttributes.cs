using BLL.DTO;
using System.ComponentModel.DataAnnotations;

namespace BLL.Attributes
{
    public class ValidatePhoneListAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var phones = value as List<PhoneDTO>;

            if(phones !=null)
            {
                foreach (var phone in phones)
                {
                    
                    if ((string.IsNullOrEmpty(phone.Number)) && !phone.IsDeleted)
                    {
                        return new ValidationResult("Please enter phone number.", new[] { nameof(PhoneDTO.Number)});
                    }

                    if ((phone.PhoneTypeId == 0) && !phone.IsDeleted)
                    {
                        return new ValidationResult("Please select a valid phone type.", new[] { nameof(PhoneDTO.PhoneTypeId) });
                    }
                }
            }

            return ValidationResult.Success;
        }
    }


}
