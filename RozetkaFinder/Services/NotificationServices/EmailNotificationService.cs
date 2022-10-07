using MailKit.Net.Smtp;
using MimeKit;
using RozetkaFinder.Models;
using RozetkaFinder.Helpers.Constants;
using RozetkaFinder.Services.JSONServices;

namespace RozetkaFinder.Services.Notification
{
    public class EmailNotificationService : INotificationService
    {
        private readonly IJsonService _jsonService;

        public EmailNotificationService(IJsonService jsonService)
        {
            _jsonService = jsonService;
        }
        public async void Send(string email, string link)
        {

            MimeMessage message = new MimeMessage();

            EmailModel emailModel = await _jsonService.GetEmailModelAsync();
            var emailFrom = emailModel.Email;
            var password = emailModel.AppPassword;
            message.From.Add(new MailboxAddress(Constants.emailNaming, emailFrom));
            message.To.Add(MailboxAddress.Parse(email));

            message.Subject = "The price of the product has DROPPED ";

            message.Body = new TextPart("plain")
            {
               Text = $"The price has droppes, congritulation, visit the link: \n {link}"
            };

            SmtpClient client = new SmtpClient();
            try
            {
               await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync(emailFrom, password);
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
    }
}
