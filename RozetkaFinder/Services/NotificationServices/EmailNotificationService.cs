using AutoMapper.Configuration;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;


namespace RozetkaFinder.Services.Notification
{
    public class EmailNotificationService : INotificationService
    {
        public async void Send(string emailTo, string emailFrom, string password, string link)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Rozetka Notification", emailFrom));
            message.To.Add(MailboxAddress.Parse(emailTo));

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
