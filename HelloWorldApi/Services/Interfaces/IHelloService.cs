﻿using System.Threading.Tasks;
using HelloWorldApi.Models;

namespace HelloWorldApi.Services.Interfaces;

public interface IHelloService
{ 
    Task<HelloWorldResponse> SayHelloAsync();
}