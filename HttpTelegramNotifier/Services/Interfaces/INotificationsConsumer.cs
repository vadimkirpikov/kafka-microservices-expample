namespace HttpTelegramNotifier.Services.Interfaces;

public interface INotificationsConsumer
{
    Task ConsumeNotifications(CancellationToken cancellationToken);
}