using HttpTelegramNotifier.Services;
using HttpTelegramNotifier.Services.Implementations;
using HttpTelegramNotifier.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HttpTelegramNotifier.Controllers;

[ApiController]
[Route("notify")]
public class TelegramController(INotifier telegramNotifier) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Notify([FromBody] string message)
    {
        var result = await telegramNotifier.SendNotifyAsync(message);
        return result ? Ok("Notified nah***!!!") : StatusCode(500, "H***ta s serverom!");
    }
}