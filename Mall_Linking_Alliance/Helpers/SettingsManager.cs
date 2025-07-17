using Mall_Linking_Alliance.Model;
using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace Mall_Linking_Alliance.Helpers
{
    public static class SettingsManager
    {
        // 🔷 Database path and connection string
        private static readonly string DatabasePath =
            Path.Combine(Application.StartupPath, "Alliance_DB.db");

        private static readonly string ConnectionString =
            $"Data Source={DatabasePath};Version=3;";

        /// <summary>
        /// 🔷 Initializes the database and creates the table if it doesn't exist
        /// </summary>
        public static void InitializeDatabase()
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    // Create the table if it doesn't exist
                    using (var command = new SQLiteCommand(@"
                        CREATE TABLE IF NOT EXISTS tblsettings (
                            tenantid INTEGER,
                            tenantkey TEXT,
                            tmid INTEGER,
                            doc TEXT,
                            browsedb TEXT,
                            savedb TEXT
                        )", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Insert default row if table is empty
                    using (var command = new SQLiteCommand(@"
                        INSERT INTO tblsettings (tenantid, tenantkey, tmid, doc, browsedb, savedb)
                        SELECT NULL, NULL, NULL, NULL, NULL, NULL
                        WHERE NOT EXISTS (SELECT 1 FROM tblsettings)", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database initialization error: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 🔷 Loads settings from database with error handling
        /// </summary>
        public static TblSettings LoadSettings()
        {
            TblSettings settings = new TblSettings();

            try
            {
                // Make sure database exists
                if (!File.Exists(DatabasePath))
                {
                    InitializeDatabase();
                }

                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    using (var command = new SQLiteCommand(
                        "SELECT tenantid, tenantkey, tmid, doc, browsedb, savedb FROM tblsettings LIMIT 1",
                        connection))
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            settings.TenantId = reader["tenantid"] as int?;
                            settings.TenantKey = reader["tenantkey"]?.ToString();
                            settings.TmId = reader["tmid"] as int?;
                            settings.Doc = reader["doc"]?.ToString();
                            settings.BrowseDb = reader["browsedb"]?.ToString();
                            settings.SaveDb = reader["savedb"]?.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading settings: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return settings;
        }

        /// <summary>
        /// 🔷 Saves settings using INSERT OR REPLACE to handle both insert and update
        /// </summary>
        public static void SaveSettings(TblSettings settings)
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    // Delete existing data and insert new (simpler than INSERT OR REPLACE)
                    using (var command = new SQLiteCommand("DELETE FROM tblsettings", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Insert the new settings
                    using (var command = new SQLiteCommand(@"
                        INSERT INTO tblsettings (tenantid, tenantkey, tmid, doc, browsedb, savedb)
                        VALUES (@tenantid, @tenantkey, @tmid, @doc, @browsedb, @savedb)", connection))
                    {
                        command.Parameters.AddWithValue("@tenantid", settings.TenantId.HasValue ? (object)settings.TenantId.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@tenantkey", settings.TenantKey ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@tmid", settings.TmId.HasValue ? (object)settings.TmId.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@doc", settings.Doc ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@browsedb", settings.BrowseDb ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@savedb", settings.SaveDb ?? (object)DBNull.Value);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 🔷 Checks if the database file exists
        /// </summary>
        public static bool DatabaseExists()
        {
            return File.Exists(DatabasePath);
        }

        /// <summary>
        /// 🔷 Gets the full path to the database file
        /// </summary>
        public static string GetDatabasePath()
        {
            return DatabasePath;
        }
    }
}