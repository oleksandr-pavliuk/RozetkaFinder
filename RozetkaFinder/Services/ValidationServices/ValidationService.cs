using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace RozetkaFinder.Services.ValidationServices
{
    public interface IValidationService
    {
        Task<bool> EmailValidationAsync(string email);
        /*Task<bool> TelegramValidationAsync(string telegram);
        Task<bool> PasswordValidationAsync(string password);
        Task<bool> PersonalValidationAsync(string data);*/

    }
    public class ValidationService : IValidationService
    {
        public async Task<bool> EmailValidationAsync(string email)
        {
            if (!string.IsNullOrEmpty(email) && new EmailAddressAttribute().IsValid(email))
                return true;
            else return false;
        }
    }
}
