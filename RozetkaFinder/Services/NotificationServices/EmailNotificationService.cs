using AutoMapper.Configuration;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;


namespace RozetkaFinder.Services.Notification
{
    public class EmailNotificationService : INotificationService
    {
        public void Send(string emailTo, string emailFrom, string password, string link)
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
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(emailFrom, password);
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
