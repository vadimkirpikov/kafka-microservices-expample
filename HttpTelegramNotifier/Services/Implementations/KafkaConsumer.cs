using Confluent.Kafka;
using HttpTelegramNotifier.Models;
using HttpTelegramNotifier.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace HttpTelegramNotifier.Services.Implementations;

public class KafkaConsumer: INotificationsConsumer
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly ILogger<KafkaConsumer> _logger;
    private readonly INotifier _notifier;

    public KafkaConsumer(IOptions<KafkaSettings> kafkaSettings, ILogger<KafkaConsumer> logger, INotifier notifier)
    {
        _notifier = notifier;
        _logger = logger;
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = kafkaSettings.Value.BootstrapServers,
            GroupId = kafkaSettings.Value.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        _consumer.Subscribe(kafkaSettings.Value.TopicName);
    }
    public async Task ConsumeNotifications(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cancellationToken);
                    var result = await _notifier.SendNotifyAsync(consumeResult.Message.Value);
                    if (!result)
                    {
                        _logger.LogWarning("The notification was not sent");
                    }
                    else
                    {
                        _logger.LogInformation("The notification was sent successfully");
                    }
                }
                catch (ConsumeException e)
                {
                    _logger.LogError(e, "An error occured while consuming a notification");
                }
            }
        }
        catch (OperationCanceledException e)
        {
            _logger.LogError(e, "The notification was cancelled");
        }
        finally
        {
            _consumer.Close();
        }
    }
}