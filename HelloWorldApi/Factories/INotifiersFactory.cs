using HelloWorldApi.Services.Interfaces;

namespace HelloWorldApi.Factories;

public interface INotifiersFactory
{
    INotifier GetNotifier(string state);
}