using RozetkaFinder.DTOs;
using RozetkaFinder.Repository;
using System.ComponentModel.DataAnnotations;
using RozetkaFinder.Models.Exceptions.ValidationException;

namespace RozetkaFinder.Services.ValidationServices
{
    public interface IValidationService
    {
        Task<bool> ModelValidationAsync(UserRegisterDTO request);

    }
    public class ValidationService : IValidationService
    {
        private readonly IUserRepository _repository; 
        public ValidationService(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> ModelValidationAsync(UserRegisterDTO request)
        {
            bool result = true;
            if (await EmailValidationAsync(request.Email))
                result = true;
            

            if(await PasswordValidationAsync(request.Password))
                result = true;
            

            if (await TelegramValidation(request.Telegram))
                result = true;

            return result;
        }
         // =============== EMAIL VALIDATION ================
        private async Task<bool> EmailValidationAsync(string email)
        {
            if(await _repository.ReadAsync(email) != null)
            {
                throw new EmailExistingException("User already exist . . .", email);
            }

            if (!string.IsNullOrEmpty(email) && new EmailAddressAttribute().IsValid(email))
                return true;
            else
            {
                throw new EmailFormatException("Email is not valid . . . ", email);
            }
        }

        //=================== PASSWORD VALIDATION ======================
        private async Task<bool> PasswordValidationAsync(string password)
        {
            if(password.Length < 8)
            {
                throw new PasswordFormatException("Password is not valid . . . Must have 8 or more then 8 signs . . .", password);
            }

            int validConditions = 0;
            foreach (char c in password)
            {
                if (c >= 'a' && c <= 'z')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 0) throw new PasswordFormatException("Password is not valid . . . Must have the LOWER case letter . . .", password);
            foreach (char c in password)
            {
                if (c >= 'A' && c <= 'Z')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 1) throw new PasswordFormatException("Password is not valid . . . Must have the UPPER case letter . . .", password);
            foreach (char c in password)
            {
                if (c >= '0' && c <= '9')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 2) throw new PasswordFormatException("Password is not valid . . . Must have the number . . .", password);
            if (validConditions == 3)
            {
                char[] special = { '@', '#', '$', '%', '^', '&', '+', '=' }; // or whatever    
                if (password.IndexOfAny(special) == -1) throw new PasswordFormatException("Password is not valid . . . Must have the SPECIFIC sign (@ , # , $, ^ , & , + , =) . . .", password);
            }
            return true;

        }

        //========================= TELEGRAM VALIDATION =========================
        private async Task<bool> TelegramValidation(string telegram)
        {
            if (!telegram.ToLower().StartsWith('@'))
                throw new TelegramFormatException("Telegram is not valid . . ", request.Telegram);
            return true;
        }
    }
}
