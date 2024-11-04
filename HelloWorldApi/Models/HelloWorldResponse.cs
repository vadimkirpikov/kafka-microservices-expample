using System;

namespace HelloWorldApi.Models;

public class HelloWorldResponse
{
    public string Text { get; set; } = "Hello, World!";
    public DateTime DateTime { get; set; } = DateTime.Now;
    public bool IsNotified { get; set; }
}