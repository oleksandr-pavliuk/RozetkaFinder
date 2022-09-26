namespace RozetkaFinder.Services.Notification
{
    public interface INotificationService
    {
        void Send(string to, string from, string password, string link);
    }
}
