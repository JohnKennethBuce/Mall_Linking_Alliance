using Mall_Linking_Alliance.Model;
using System;
using System.Data.SQLite;

namespace Mall_Linking_Alliance.Helpers
{
    public static class SettingsManager
    {
        // 🔷 What’s this for?  
        // Defines the connection string to your SQLite database.
        // Uses the same folder where your app runs (StartupPath) and opens your `.db` file.
        private static readonly string ConnectionString =
            $"Data Source={System.Windows.Forms.Application.StartupPath}\\Alliance_DB.db;Version=3;";

        /// <summary>
        /// 🔷 What’s this for?  
        /// Reads the first row from `tblsettings` table in your DB
        /// and loads it into a TblSettings object in memory.
        /// </summary>
        public static TblSettings LoadSettings()
        {
            TblSettings settings = new TblSettings();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(
                    "SELECT tenantid, tenantkey, tmid, doc, browsedb, savedb FROM tblsettings LIMIT 1",
                    connection))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) // 🔷 What’s this for?  
                                       // Checks if at least 1 row exists
                    {
                        // 🔷 What’s this for?  
                        // Reads each column from DB row and sets it into our settings object.
                        settings.TenantId = reader["tenantid"] as int?;
                        settings.TenantKey = reader["tenantkey"]?.ToString();
                        settings.TmId = reader["tmid"] as int?;
                        settings.Doc = reader["doc"]?.ToString();
                        settings.BrowseDb = reader["browsedb"]?.ToString();
                        settings.SaveDb = reader["savedb"]?.ToString();
                    }
                }
            }

            return settings; // 🔷 Returns the settings loaded from DB
        }

        /// <summary>
        /// 🔷 What’s this for?  
        /// Saves (updates) your settings back into the `tblsettings` table in DB.
        /// Overwrites the first row with new values.
        /// </summary>
        public static void SaveSettings(TblSettings settings)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(
                    @"UPDATE tblsettings SET 
                        tenantid = @tenantid,
                        tenantkey = @tenantkey,
                        tmid = @tmid,
                        doc = @doc,
                        browsedb = @browsedb,
                        savedb = @savedb",
                    connection))
                {
                    // 🔷 What’s this for?  
                    // Assigns the values from our `settings` object into SQL parameters.
                    // If a field is null, sends DBNull.Value (since DB cannot store C# null)
                    command.Parameters.AddWithValue("@tenantid", settings.TenantId.HasValue ? (object)settings.TenantId.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@tenantkey", settings.TenantKey ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@tmid", settings.TmId.HasValue ? (object)settings.TmId.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@doc", settings.Doc ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@browsedb", settings.BrowseDb ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@savedb", settings.SaveDb ?? (object)DBNull.Value);

                    command.ExecuteNonQuery(); // 🔷 Executes the UPDATE statement
                }
            }
        }
    }
}
