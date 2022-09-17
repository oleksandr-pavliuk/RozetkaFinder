namespace RozetkaFinder.Services.Notification
{
    public class EmailNotificationService : INotificationService
    {
        public string Name { get; } = "email";
        public void Send()
        {
           throw new NotImplementedException();
        }
    }
}
