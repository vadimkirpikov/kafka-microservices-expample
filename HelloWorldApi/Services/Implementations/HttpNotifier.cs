using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HelloWorldApi.Services.Interfaces;

namespace HelloWorldApi.Services.Implementations;

public class HttpNotifier(string url): INotifier
{
    public async Task<bool> NotifyAsync(string text)
    {
        var client = new HttpClient(new HttpClientHandler { UseProxy = false });
        var content = new StringContent($"\"{text}\"" , Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);
        return response.IsSuccessStatusCode;
    }
}