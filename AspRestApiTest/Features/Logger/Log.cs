namespace AspRestApiTest.Features.Logger
{
    public class Log
    {
        /// <summary>
        /// Информационное сообщение в логи
        /// </summary>
        /// <param name="message">Сообщение</param>
        public static void Info(object message, ConsoleColor color = ConsoleColor.Cyan)
        {
            Send($"{message}", LogLevel.Information, color);
        }

        /// <summary>
        /// Предупреждающее сообщение в логи
        /// </summary>
        /// <param name="message">Сообщение</param>
        public static void Warning(object message, ConsoleColor color = ConsoleColor.Yellow)
        {
            Send($"{message}", LogLevel.Warning, color);
        }

        /// <summary>
        /// Сообщение об ошибке в логи
        /// </summary>
        /// <param name="message">Сообщение</param>
        public static void Error(object message, ConsoleColor color = ConsoleColor.DarkRed)
        {
            Send($"{message}", LogLevel.Error, color);
        }

        /// <summary>
        /// Сообщение trace в логи
        /// </summary>
        /// <param name="message">Сообщение</param>
        public static void Trace(object message)
        {
            //
        }

        /// <summary>
        /// Критические логи
        /// </summary>
        /// <param name="message">Сообщение</param>
        public static void Critical(object message)
        {
            Send($"{message}", LogLevel.Critical, ConsoleColor.Red);
        }

        /// <summary>
        /// Сообщение Debug в логи
        /// </summary>
        /// <param name="message">Сообщение</param>
        public static void Debug(object message)
        {
            //
        }

        private static void Send(string message, LogLevel level, ConsoleColor color)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var formattedMessage = "[" + timestamp + "] [" + level.ToString().ToUpper() + "] " + message;
            SendRaw(formattedMessage, color);
        }

        private static void SendRaw(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message, color);
            Console.ResetColor();
        }
    }
}
