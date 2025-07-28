using Mall_Linking_Alliance.Helpers;
using Mall_Linking_Alliance.Model;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mall_Linking_Alliance
{
    public partial class frm_Main : Form
    {
        // 🔷 Settings
        private TblSettings Settings { get; set; }

        // 🔷 XML watcher & concurrency
        private string XmlDirectory;
        private static readonly SemaphoreSlim xmlProcessingLock = new SemaphoreSlim(1, 1);
        private FileSystemWatcher _xmlWatcher;

        public frm_Main()
        {
            InitializeComponent();

            InitializeProgram();

            StartXmlWatcher();
        }

        /// <summary>
        /// Initialize the app and load settings
        /// </summary>
        private void InitializeProgram()
        {
            // 🔷 Load DB settings
            Settings = SettingsManager.LoadSettings();

            XmlDirectory = Settings.BrowseDb;

            if (string.IsNullOrWhiteSpace(Settings.SaveDb))
            {
                Settings.SaveDb = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Alliance_DB.db");
                SettingsManager.SaveSettings(Settings); // save this fallback path
            }

            // Ensure database structure exists now that path is guaranteed
            DatabaseInitializer.EnsureDatabaseStructure(Settings.SaveDb);


            // If invalid, default to Desktop and save it
            if (string.IsNullOrWhiteSpace(XmlDirectory) || !Directory.Exists(XmlDirectory))
            {
                XmlDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XMLWatcher");

                Settings.BrowseDb = XmlDirectory;

                Directory.CreateDirectory(XmlDirectory);
                SettingsManager.SaveSettings(Settings);

                MessageBox.Show($"⚠️ XML directory was invalid. Defaulting to: {XmlDirectory}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Always ensure subfolders exist
            Directory.CreateDirectory(Path.Combine(XmlDirectory, "ProcessedFiles"));
            Directory.CreateDirectory(Path.Combine(XmlDirectory, "DeniedFiles"));
        }

        /// <summary>
        /// Start watching the XML directory for new files
        /// </summary>
        private void StartXmlWatcher()
        {
            if (string.IsNullOrWhiteSpace(XmlDirectory) || !Directory.Exists(XmlDirectory))
                return;

            _xmlWatcher = new FileSystemWatcher(XmlDirectory, "*.xml")
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
            };

            _xmlWatcher.Created += async (s, e) => await ProcessNewXml(e.FullPath);
            _xmlWatcher.EnableRaisingEvents = true;

            Logger.Info($"📂 Watching directory: {XmlDirectory}", "frm_Main");
        }

        /// <summary>
        /// Process a newly detected XML file
        /// </summary>
        private async Task ProcessNewXml(string filePath)
        {
            await xmlProcessingLock.WaitAsync();

            try
            {
                if (!File.Exists(filePath))
                    return;

                Logger.Info($"📄 Detected new XML: {Path.GetFileName(filePath)}", "frm_Main");

                string xmlContent = File.ReadAllText(filePath, Encoding.UTF8);

                bool success = XmlProcessor.Process(xmlContent, filePath, Settings);

                Logger.Info(success
                    ? $"✅ Processed: {Path.GetFileName(filePath)}"
                    : $"❌ Denied: {Path.GetFileName(filePath)}", "frm_Main");
            }
            catch (Exception ex)
            {
                Logger.Error($"❌ Error processing XML: {ex.Message}", "frm_Main");
            }
            finally
            {
                xmlProcessingLock.Release();
            }
        }

        /// <summary>
        /// On form load
        /// </summary>
        private void frm_Main_Load(object sender, EventArgs e)
        {
            // Optional startup logic
        }

        /// <summary>
        /// Exit app from context menu
        /// </summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Open config form from context menu
        /// </summary>
        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var configForm = new Config())
            {
                configForm.ShowDialog();
            }

            // Reload settings if updated
            Settings = SettingsManager.LoadSettings();
            XmlDirectory = Settings.BrowseDb;

            Logger.Info("🔄 Settings reloaded.", "frm_Main");
        }
    }
}
