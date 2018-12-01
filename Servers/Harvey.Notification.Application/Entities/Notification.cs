namespace Harvey.Notification.Application.Entities
{
    public class Notification : BaseEntity<long>
    {
        public string Content { get; set; }
        public int Status { get; set; }
        public int NotificationTypeId { get; set; }
        public virtual NotificationType NotificationType { get; set; }
        public int TemplateId { get; set; }
        public Action Action { get; set; }
        public virtual Template Template { get; set; }
        public string Receivers { get; set; }
    }

    public enum Action
    {
        WelcomeBack,
        Redemption,
        Welcome,
        ResetPassword,
        ResendSignUp,
        ReSendPin,
        ChangeMobilePhone,
        ForgotPassword,
        EmailResetPassword,
        ReminderExpiryMembership,
        ReminderExpiryRewardPoints
    }
}
