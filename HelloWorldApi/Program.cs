using System.Data;
using HelloWorldApi.Factories;
using HelloWorldApi.Models;
using HelloWorldApi.Repositories;
using HelloWorldApi.Services.Implementations;
using HelloWorldApi.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var httpNotifyUrl = builder.Configuration["HttpNotifyUrl"];
var postgresConnection = builder.Configuration.GetConnectionString("PostgresConnection");


builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

builder.Services.AddScoped<IHelloService, HelloService>();
builder.Services.AddScoped<HttpNotifier>(serviceProvider => new HttpNotifier(httpNotifyUrl!));
builder.Services.AddScoped<KafkaNotifier>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<INotifiersFactory, NotifiersFactory>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
});
builder.Services.AddScoped<IDbConnection>(serviceProvider => new NpgsqlConnection(postgresConnection));
builder.Services.AddScoped<IAddRepository<HelloWorldResponse>, ResponsesRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var repository = scope.ServiceProvider.GetRequiredService<IAddRepository<HelloWorldResponse>>();
    while (true)
    {
        try
        {
            repository.CreateTable();
            break;
        }
        catch
        {
            continue;
        }
    }


}
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger/index.html"));
app.Run();
