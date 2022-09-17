namespace RozetkaFinder.Services.Notification
{
    public interface INotificationService
    {
        string Name { get; }
        void Send();
    }
}
