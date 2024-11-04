namespace HttpTelegramNotifier.Services.Interfaces;

public interface INotifier
{
    Task<bool> SendNotifyAsync(string message, long chatId=876419367);
}