using HttpTelegramNotifier.Services.Interfaces;
using Telegram.Bot;

namespace HttpTelegramNotifier.Services.Implementations;

public class TelegramNotifier(string botToken, ILogger<TelegramNotifier> logger): INotifier
{
    private readonly TelegramBotClient _botClient = new TelegramBotClient(botToken);

    public async Task<bool> SendNotifyAsync(string message, long chatId=876419367)
    {
        try
        {
            await Task.Delay(20000);
            await _botClient.SendTextMessageAsync(chatId, message);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return false;
        }
    }
}