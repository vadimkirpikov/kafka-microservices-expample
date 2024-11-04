using HelloWorldApi.Services.Implementations;
using HelloWorldApi.Services.Interfaces;

namespace HelloWorldApi.Factories;

public class NotifiersFactory(IServiceProvider serviceProvider): INotifiersFactory
{
    public INotifier GetNotifier(string state)
    {
        return state switch
        {
            "http" =>  serviceProvider.GetRequiredService<HttpNotifier>(),
            "kafka" =>  serviceProvider.GetRequiredService<KafkaNotifier>(),
            _ => serviceProvider.GetRequiredService<HttpNotifier>()
        };
    }
}