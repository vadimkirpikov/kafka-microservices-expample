using HttpTelegramNotifier.Models;
using HttpTelegramNotifier.Services;
using HttpTelegramNotifier.Services.Hosted;
using HttpTelegramNotifier.Services.Implementations;
using HttpTelegramNotifier.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddHostedService<KafkaNotifier>();
builder.Services.AddScoped<INotificationsConsumer, KafkaConsumer>();
builder.Services.AddSingleton<INotifier>(serviceProvider =>
{
    var botToken = builder.Configuration["BotToken"];
    var logger = serviceProvider.GetRequiredService<ILogger<TelegramNotifier>>();
    return new TelegramNotifier(botToken!, logger);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.UseRouting();
app.Map("/", () => Results.Redirect("/swagger/index.html"));
app.Run();
