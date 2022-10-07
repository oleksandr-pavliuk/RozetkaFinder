using System.Security;

namespace RozetkaFinder.Helpers.Constants
{
    public static class Constants
    {
        //EXCEPTIONS
        public const string userNotFoundMessage = "User not found . . . Try again";
        public const string passwordOrEmailInvalid = "Password or email isn't correct . . .";
        public const string authMakeNewPassword = "Make new password. You can't use the previous password . . .";
        public const string passwordChanged = "Password changed . . . ";
        public const string passwordIsNotCorrect = "Password is not correct. Try it again . . .";
        public const string notificationChanged = "Notification changed . . . ";
        public const string goodWasNotFound = "Good wasn't found . . .";
        public const string markdownWasNotFound = "Markdown wasn't found . . .";
        public const string emailExistingMessage = "Email already exists . . . ";
        public const string userExistingMessage = "User already exists . . . ";
        public const string passValidation8 = "Password is not valid . . . Must have 8 or more then 8 signs . . .";
        public const string passValidationLower = "Password is not valid . . . Must have the LOWER case letter . . .";
        public const string passValidationUpper = "Password is not valid . . . Must have the UPPER case letter . . .";
        public const string passValidationNum = "Password is not valid . . . Must have the number . . .";
        public const string passValidationSpecific = "Password is not valid . . . Must have the SPECIFIC sign (@ , # , $, ^ , & , + , =) . . .";
        public const string telegramValidation = "Telegram is not valid . . .";


        //NAMING
        public const string emailNaming = "Rozetka Notification";

        //TELEGRAM BOT
        public const string gettingEmailBot = "Write me your email please . . .";
        public const string mainTextBot = "Thank you for using our service :) \n Now you will get your notification from this bot (if your notification setting set on the TELEGRAM)";
        public const string defaultBot = "It not command . . .";
        public const string visitBot = "Visit your subscription  . . . ";


    }
}
