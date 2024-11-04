﻿namespace HttpTelegramNotifier.Models;

public class KafkaSettings
{
    public string? BootstrapServers { get; set; }
    public string? TopicName { get; set; }
    public string? GroupId { get; set; }
}