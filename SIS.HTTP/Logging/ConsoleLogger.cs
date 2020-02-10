﻿using System;

namespace SIS.HTTP.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[{DateTime.UtcNow.ToString()} {message}]");
        }
    }
}