using Mall_Linking_Alliance.Model;
using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Mall_Linking_Alliance.Helpers
{
    public static class XmlProcessor
    {
        public static bool Process(string xmlContent, string fileName, TblSettings settings)
        {
            var result = ProcessXml(xmlContent, fileName, settings);

            string targetFolder = result.Success
                ? Path.Combine(settings.BrowseDb, "ProcessedFiles")
                : Path.Combine(settings.BrowseDb, "DeniedFiles");

            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            string destinationPath = Path.Combine(targetFolder, Path.GetFileName(fileName));

            if (File.Exists(destinationPath))
                File.Delete(destinationPath);

            File.Move(fileName, destinationPath);

            if (!result.Success || result.HasErrors)
            {
                WriteDeniedLog(destinationPath, result);
                Logger.Error($"❌ {result.Message}", "XmlProcessor");
                return false;
            }

            Logger.Info($"✅ Processed: {Path.GetFileName(fileName)}", "XmlProcessor");
            return true;
        }

        private static XmlProcessingResult ProcessXml(string xmlContent, string fileName, TblSettings settings)
        {
            var result = new XmlProcessingResult();

            try
            {
                var doc = XDocument.Parse(xmlContent);

                var order = doc.Root?.Element("Order");
                if (order == null)
                {
                    result.Success = false;
                    result.HasErrors = true;
                    result.Message = "Missing <Order> element.";
                    return result;
                }

                var tenantIdAttr = order.Attribute("tenantId")?.Value;
                var tenantKeyAttr = order.Attribute("tenantKey")?.Value;

                Logger.Info($"🧾 Order: TenantId={tenantIdAttr}, TenantKey={tenantKeyAttr}", "XmlProcessor");

                var newSettings = new TblSettings
                {
                    TenantId = int.TryParse(tenantIdAttr, out int tid) ? tid : (int?)null,
                    TenantKey = tenantKeyAttr,
                    Doc = fileName,
                    BrowseDb = settings.BrowseDb,
                    SaveDb = settings.SaveDb
                };

                SettingsManager.SaveSettings(newSettings);

                result.Message = "Processed successfully.";
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.HasErrors = true;
                result.Message = $"Exception: {ex.Message}";
                return result;
            }
        }

        private static void WriteDeniedLog(string deniedFilePath, XmlProcessingResult result)
        {
            string logPath = Path.ChangeExtension(deniedFilePath, ".log");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"🗓️ Date: {DateTime.Now}");
            sb.AppendLine($"📄 File: {Path.GetFileName(deniedFilePath)}");
            sb.AppendLine($"❌ Reason: {result.Message}");

            File.WriteAllText(logPath, sb.ToString());
        }
    }
}
