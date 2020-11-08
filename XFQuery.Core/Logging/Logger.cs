using System;

namespace XFQuery.Core.Logging
{
    public enum LogType
    {
        Ok,
        Debug,
        Info,
        Notice,
        Warning,
        Error
    }

    public abstract class Logger
    {
        protected void Write(LogType logType, string format, params object[] args)
        {
            var msg = $"[{DateTime.UtcNow}] : {string.Format(format, args)}";

            ConsoleColor consoleColor;

            switch (logType)
            {
                case LogType.Ok:
                    consoleColor = ConsoleColor.Green;
                    break;

                case LogType.Debug:
                    consoleColor = ConsoleColor.Gray;
                    break;

                case LogType.Info:
                    consoleColor = ConsoleColor.Cyan;
                    break;

                case LogType.Notice:
                    consoleColor = ConsoleColor.Magenta;
                    break;

                case LogType.Warning:
                    consoleColor = ConsoleColor.Yellow;
                    break;

                case LogType.Error:
                    consoleColor = ConsoleColor.Red;
                    break;

                default:
                    consoleColor = ConsoleColor.Gray;
                    break;
            }

            Console.ForegroundColor = consoleColor;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        protected void WriteException(string message, Exception ex)
        {
            var outerEx = ex;
            while (ex.InnerException != null) ex = ex.InnerException;
            if (outerEx.GetType() != ex.GetType()) Write(LogType.Error, "ExType: {0}", outerEx.GetType().Name);
            Write(LogType.Error, $"{message} ({ex.GetType().Name}: {ex.Message})\n{ex.StackTrace}");
        }
    }
}