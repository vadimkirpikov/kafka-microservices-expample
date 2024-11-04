using System;
using System.Threading.Tasks;
using HelloWorldApi.Factories;
using HelloWorldApi.Models;
using HelloWorldApi.Repositories;
using HelloWorldApi.Services.Interfaces;

namespace HelloWorldApi.Services.Implementations;

public class HelloService(
    INotifiersFactory notifierFactory,
    ICacheService cacheService,
    IAddRepository<HelloWorldResponse> repository) : IHelloService
{
    public async Task<HelloWorldResponse> SayHelloAsync()
    {
        var dateTime = DateTime.Now;
        var state = await cacheService.GetAsync("state");

        var result = await notifierFactory.GetNotifier(state!)
            .NotifyAsync($"Request to get 'Hello, World!' was done at {dateTime} with method '{state}'");
        var response = new HelloWorldResponse { IsNotified = result };
        await repository.AddAsync(response);
        return response;
    }
}