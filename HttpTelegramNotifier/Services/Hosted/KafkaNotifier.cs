using HttpTelegramNotifier.Services.Interfaces;

namespace HttpTelegramNotifier.Services.Hosted;

public class KafkaNotifier(INotificationsConsumer notificationsConsumer) : IHostedService
{
    private CancellationTokenSource? _cts;
    
    public Task StartAsync(CancellationToken cancellationToken)
    {

        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        
        Task.Run(() => notificationsConsumer.ConsumeNotifications(_cts.Token), cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cts!.CancelAsync();
        return Task.CompletedTask;
    }
}