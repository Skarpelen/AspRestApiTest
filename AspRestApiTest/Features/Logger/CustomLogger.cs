namespace AspRestApiTest.Features.Logger
{
    using CustomLog = AspRestApiTest.Features.Logger.Log;

    public class CustomLogger : ILogger
    {
        private readonly string _name;

        public CustomLogger(string name)
        {
            _name = name;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return default!;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel is not LogLevel.Trace;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var logEntry = formatter(state, exception);
            //var message = $"[{_name}] {logEntry}";
            var message = $"{logEntry}";

            if (exception != null)
            {
                message += Environment.NewLine + $"Exception: {exception.GetType()} - {exception.Message}{Environment.NewLine}{exception.StackTrace}";
            }

            switch (logLevel)
            {
                case LogLevel.Trace:
                    CustomLog.Trace(message);
                    break;
                case LogLevel.Debug:
                    CustomLog.Debug(message);
                    break;
                case LogLevel.Information:
                    CustomLog.Info(message);
                    break;
                case LogLevel.Warning:
                    CustomLog.Warning(message);
                    break;
                case LogLevel.Error:
                    CustomLog.Error(message);
                    break;
                case LogLevel.Critical:
                    CustomLog.Critical(message);
                    break;
                case LogLevel.None:
                    break;
            }
        }
    }
}
