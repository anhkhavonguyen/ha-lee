namespace Harvey.Notification.Application.Domains.Notifications.Queries
{
    public interface IGetNotificationsQuery
    {
        GetNotificationsResponse Execute(GetNotificationsRequest request);
    }
}
