using Mall_Linking_Alliance.Model;
using System;
using System.IO;

namespace Mall_Linking_Alliance.Helpers
{
    public static class Logger
    {
        private static readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppLog.txt");

        /// <summary>
        /// Log an informational message.
        /// </summary>
        public static void Info(string message, string source = "System")
        {
            WriteLog("INFO", message, source);
        }

        /// <summary>
        /// Log a warning message.
        /// </summary>
        public static void Warn(string message, string source = "System")
        {
            WriteLog("WARN", message, source);
        }

        /// <summary>
        /// Log an error message.
        /// </summary>
        public static void Error(string message, string source = "System")
        {
            WriteLog("ERROR", message, source);
        }

        /// <summary>
        /// Write the log entry to file and optionally DB.
        /// </summary>
        private static void WriteLog(string level, string message, string source)
        {
            var log = new TblLog
            {
                LogDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                LogLevel = level,
                Source = source,
                Message = message
            };

            string logLine = $"[{log.LogDate}] [{log.LogLevel}] [{log.Source}] {log.Message}";

            // Append to file
            File.AppendAllText(logFilePath, logLine + Environment.NewLine);

            // Save to DB if needed (optional stub)
            SaveLogToDatabase(log);
        }

        private static void SaveLogToDatabase(TblLog log)
        {
            // TODO: implement DB insert if needed
            // Example:
            // Database.InsertLog(log);
        }
    }
}
