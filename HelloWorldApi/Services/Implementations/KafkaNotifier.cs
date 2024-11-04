using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using HelloWorldApi.Models;
using HelloWorldApi.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HelloWorldApi.Services.Implementations;

public class KafkaNotifier: INotifier
{
    private readonly IProducer<string, string> _producer;
    private readonly string _topicName;
    private readonly ILogger<KafkaNotifier> _logger;
    public KafkaNotifier(IOptions<KafkaSettings> kafkaSettings, ILogger<KafkaNotifier> logger)
    {
        _logger = logger;
        _topicName = kafkaSettings.Value.TopicName!;
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
            _logger.LogInformation($"Message: {result.TopicPartitionOffset}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            return false;
        }
    }
}