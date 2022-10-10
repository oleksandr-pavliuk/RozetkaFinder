using MailKit.Net.Smtp;
using MimeKit;
using RozetkaFinder.Models;
using RozetkaFinder.Helpers.Constants;
using RozetkaFinder.Services.JSONServices;
using Telegram.Bots.Requests;

namespace RozetkaFinder.Services.Notification
{
    public class EmailNotificationService : INotificationService
    {
        private readonly IJsonService _jsonService;
        private EmailModel emailModel;
        private MimeMessage message = new MimeMessage();
        public EmailNotificationService(IJsonService jsonService)
        {
            _jsonService = jsonService;
        }

        // Method for sending email messages.
        public async void Send(string email, string link)
        {
            GetEmailModel();
            FormatingEmailMessage(email, link);
            

            SmtpClient client = new SmtpClient();
            try
            {
               await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync(emailModel.Email, emailModel.AppPassword);
               await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
            
        }

        // Method for getting email model(email which use our project).
        private async void GetEmailModel()
        {
            emailModel = await _jsonService.GetEmailModelAsync();
        }

        //Method for formating the email text , links , etc.
        private void FormatingEmailMessage(string email, string link)
        {
            message.From.Add(new MailboxAddress(Constants.emailNaming, emailModel.Email));
            message.To.Add(MailboxAddress.Parse(email));

            message.Subject = "The price of the product has DROPPED ";

            message.Body = new TextPart("plain")
            {
                Text = $"The price has droppes, congritulation, visit the link: \n {link}"
            };
        }
    }
}
