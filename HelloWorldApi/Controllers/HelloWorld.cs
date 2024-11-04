using System.Threading.Tasks;
using HelloWorldApi.Models;
using HelloWorldApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorldApi.Controllers;

[ApiController]
[Route("hello-world")]
public class HelloWorldController(IHelloService helloService, ICacheService cacheService) : ControllerBase
{
    [HttpGet("with-notify")]
    public async Task<IActionResult> GetHelloWorld()
    {
        var response = await helloService.SayHelloAsync();
        return Ok(response);
    }

    [HttpGet("replace-state")]
    public async Task<IActionResult> ReplaceState()
    {
        await cacheService.SwapAsync("state");
        return Ok();
    }
}