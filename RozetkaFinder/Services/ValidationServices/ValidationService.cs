using RozetkaFinder.DTOs;
using RozetkaFinder.Repository;
using System.ComponentModel.DataAnnotations;
using RozetkaFinder.Models.Exceptions.ValidationException;
using RozetkaFinder.Models.User;

namespace RozetkaFinder.Services.ValidationServices
{
    public interface IValidationService
    {
        void ModelValidation(UserRegisterDTO request);

    }
    public class ValidationService : IValidationService
    {
        private readonly IRepository<User> _repository; 
        public ValidationService(IRepository<User> repository)
        {
            _repository = repository;
        }
        public void ModelValidation(UserRegisterDTO request)
        {
            EmailValidation(request.Email);

            PasswordValidation(request.Password);

            TelegramValidation(request.Telegram);
        }

         // =============== EMAIL VALIDATION ================
        private void EmailValidation(string email)
        {
            if(_repository.ReadAsync(u => u.Email == email) != null)
            {
                throw new EmailExistingException("User already exist . . .", email);
            }

            if (string.IsNullOrEmpty(email) || !(new EmailAddressAttribute().IsValid(email)))
                throw new EmailFormatException("Email is not valid . . . ", email);
        }

        //=================== PASSWORD VALIDATION ======================
        private void PasswordValidation(string password)
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

        }

        //========================= TELEGRAM VALIDATION =========================
        private void TelegramValidation(string telegram)
        {
            if (!telegram.ToLower().StartsWith('@'))
                throw new TelegramFormatException("Telegram is not valid . . ", telegram);
        }
    }
}
