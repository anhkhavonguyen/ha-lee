namespace Harvey.Message.Notifications
{
    public interface SendAllPendingSmsCommand
    {
        bool IsSendAllMessage { get; }
    }
}
