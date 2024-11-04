using System.Threading.Tasks;

namespace HelloWorldApi.Services.Interfaces;

public interface INotifier
{
    Task<bool> NotifyAsync(string text);
}