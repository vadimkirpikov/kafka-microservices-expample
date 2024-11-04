using System.Threading.Tasks;

namespace HelloWorldApi.Services.Interfaces;

public interface ICacheService
{
    Task<string?> GetAsync(string key);
    Task SetAsync(string key, string value);
    Task SwapAsync(string key);
}