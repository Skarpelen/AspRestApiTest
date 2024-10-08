﻿using System.Collections.Concurrent;

namespace AspRestApiTest.Features.Logger
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, CustomLogger> _loggers = new ConcurrentDictionary<string, CustomLogger>();

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new CustomLogger(name));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
