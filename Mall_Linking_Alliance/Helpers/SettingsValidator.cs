using System;
using System.IO;
using Mall_Linking_Alliance.Model;

namespace Mall_Linking_Alliance.Helpers
{
    public static class SettingsValidator
    {
        public static TblSettings ValidateAndFixSettings(TblSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (string.IsNullOrWhiteSpace(settings.BrowseDb) || !Directory.Exists(settings.BrowseDb))
            {
                string defaultBrowsePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XmlWatcher");
                Directory.CreateDirectory(defaultBrowsePath);
                settings.BrowseDb = defaultBrowsePath;
            }

            if (string.IsNullOrWhiteSpace(settings.SaveDb) || !File.Exists(settings.SaveDb))
            {
                string defaultSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Alliance_DB.db");
                settings.SaveDb = defaultSavePath;
            }

            Directory.CreateDirectory(Path.Combine(settings.BrowseDb, "ProcessedFiles"));
            Directory.CreateDirectory(Path.Combine(settings.BrowseDb, "DeniedFiles"));

            return settings;
        }
    }
}
