using Mall_Linking_Alliance.Model;
using System;
using System.Data.SQLite;
using System.IO;

namespace Mall_Linking_Alliance.Helpers
{
    public static class DatabaseHelper
    {
        private static readonly string DbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yourdbfile.db");
        private static readonly string ConnectionString = $"Data Source={DbPath};Version=3;";

        /// <summary>
        /// Insert a log entry into Logs table.
        /// </summary>
        public static void InsertLog(TblLog log)
        {
            try
            {
                using (var conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = @"
                        INSERT INTO Logs (LogDate, LogLevel, Source, Message) 
                        VALUES (@LogDate, @LogLevel, @Source, @Message);";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@LogDate", log.LogDate);
                        cmd.Parameters.AddWithValue("@LogLevel", log.LogLevel);
                        cmd.Parameters.AddWithValue("@Source", log.Source);
                        cmd.Parameters.AddWithValue("@Message", log.Message);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Failed to save log to DB: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
