namespace HelloWorldApi.Services.Interfaces;

public interface INotifier
{
    Task<bool> NotifyAsync(string text);
}