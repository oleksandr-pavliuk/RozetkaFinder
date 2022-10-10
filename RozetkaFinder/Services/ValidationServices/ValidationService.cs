using RozetkaFinder.DTOs;
using RozetkaFinder.Repository;
using System.ComponentModel.DataAnnotations;
using RozetkaFinder.Models.Exceptions.ValidationException;
using RozetkaFinder.Models.User;
using RozetkaFinder.Helpers.Constants;

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

         // Email validation (using for checking naming of the email boxes)
         // that helps to avoid conflicts.
        private void EmailValidation(string email)
        {
            if(_repository.ReadAsync(u => u.Email == email) != null)
            {
                throw new EmailExistingException(Constants.userExistingMessage, email);
            }

            if (string.IsNullOrEmpty(email) || !(new EmailAddressAttribute().IsValid(email)))
                throw new EmailFormatException(Constants.emailExistingMessage, email);
        }


        // Email validation (using for checking security level of the user's password)
        // that helps to avoid problems.
        private void PasswordValidation(string password)
        {
            if(password.Length < 8)
            {
                throw new PasswordFormatException(Constants.passValidation8, password);
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
            if (validConditions == 0) throw new PasswordFormatException(Constants.passValidationLower, password);
            foreach (char c in password)
            {
                if (c >= 'A' && c <= 'Z')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 1) throw new PasswordFormatException(Constants.passValidationUpper, password);
            foreach (char c in password)
            {
                if (c >= '0' && c <= '9')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 2) throw new PasswordFormatException(Constants.passValidationNum, password);
            if (validConditions == 3)
            {
                char[] special = { '@', '#', '$', '%', '^', '&', '+', '=' }; // or whatever    
                if (password.IndexOfAny(special) == -1) throw new PasswordFormatException(Constants.passValidationSpecific, password);
            }

        }

        // Telegram validation (using for checking naming of the telegram's username)
        // that helps to avoid conflicts.
        private void TelegramValidation(string telegram)
        {
            if (!telegram.ToLower().StartsWith('@'))
                throw new TelegramFormatException(Constants.telegramValidation, telegram);
        }
    }
}
