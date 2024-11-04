using Confluent.Kafka;
using HelloWorldApi.Models;
using HelloWorldApi.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace HelloWorldApi.Services.Implementations;

public class KafkaNotifier: INotifier
{
    private readonly IProducer<string, string> _producer;
    private readonly string _topicName;
    public KafkaNotifier(IOptions<KafkaSettings> kafkaSettings)
    {
        _topicName = kafkaSettings.Value.TopicName!;
        Console.WriteLine($"Kafka Topic Name: {_topicName}");
        var config = new ProducerConfig
        {
            BootstrapServers = kafkaSettings.Value.BootstrapServers
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }
    public async Task<bool> NotifyAsync(string text)
    {
        try
        {
            var result = await _producer.ProduceAsync(_topicName, new Message<string, string> { Value = text });
            Console.WriteLine($"Message: {result.TopicPartitionOffset}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}