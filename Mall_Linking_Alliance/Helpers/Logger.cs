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
        public static void Info(string message, string source = "System", string dbPath = null)
        {
            WriteLog("INFO", message, source, dbPath);
        }

        /// <summary>
        /// Log a warning message.
        /// </summary>
        public static void Warn(string message, string source = "System", string dbPath = null)
        {
            WriteLog("WARN", message, source, dbPath);
        }


        /// <summary>
        /// Log an error message.
        /// </summary>
        public static void Error(string message, string source = "System", string dbPath = null)
        {
            WriteLog("ERROR", message, source, dbPath);
        }

        /// <summary>
        /// Write the log entry to file and optionally DB.
        /// </summary>
        private static void WriteLog(string level, string message, string source, string dbPath)
        {
            var log = new TblLog
            {
                LogDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                LogLevel = level,
                Source = source,
                Message = message
            };

            string logLine = $"[{log.LogDate}] [{log.LogLevel}] [{log.Source}] {log.Message}";

            // Always write to file
            File.AppendAllText(logFilePath, logLine + Environment.NewLine);

            // Optionally write to DB if dbPath is valid
            if (!string.IsNullOrWhiteSpace(dbPath) && File.Exists(dbPath))
            {
                DatabaseHelper.InsertLog(log, dbPath);
            }
        }
    }
}
