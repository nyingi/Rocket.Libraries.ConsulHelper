namespace Rocket.Libraries.ConsulHelper.Services
{
    using System;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// This helper class contains extension methods that augument default Log writing to information printed out on the Console.
    /// </summary>
    internal static class NoisyLogger
    {
        /// <summary>
        /// Log using level 'Info' and write to screen in green.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="message">The message to be written.</param>
        public static void LogNoisyInformation(this ILogger logger, string message)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                WriteToConsole(message, "Information");
                logger.LogInformation(message);
            }
            finally
            {
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Log using level 'Warning' and write to screen in yellow.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="message">The message to be written.</param>
        public static void LogNoisyWarning(this ILogger logger, string message)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                WriteToConsole(message, "Warning");
                logger.LogWarning(message);
            }
            finally
            {
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Log using level 'Error' and write to screen in red.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="exception">Any exception that may have been thrown.</param>
        /// <param name="message">The message to be written.</param>
        public static void LogNoisyError(this ILogger logger, Exception exception, string message)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                WriteToConsole(message, "Error");
                if (exception != null)
                {
                    logger.LogError(exception, message);
                }
                else
                {
                    logger.LogError(message);
                }
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private static void WriteToConsole(string message, string level)
        {
            Console.WriteLine($"[ConsulHelper] - [{level}]: {message}");
        }
    }
}