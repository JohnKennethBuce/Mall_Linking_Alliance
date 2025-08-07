using Mall_Linking_Alliance.Helpers;
using Mall_Linking_Alliance.Model;
using Mall_Linking_Alliance.Watchers;
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
        private XmlProcessingWatcher _xmlProcessingWatcher;

        public frm_Main()
        {
            InitializeComponent();
            InitializeProgram();

            // Start watcher using validated paths
            _xmlProcessingWatcher = new XmlProcessingWatcher(Settings.BrowseDb, Settings);
            _xmlProcessingWatcher.Start();
        }

        /// <summary>
        /// Initialize the app and load settings
        /// </summary>
        private void InitializeProgram()
        {
            // 🔷 Load DB settings
            Settings = SettingsManager.LoadSettings();

            // 🔷 Validate paths and ensure folder structure
            Settings = SettingsValidator.ValidateAndFixSettings(Settings);

            // 🔷 Save updated settings if any defaults were applied
            SettingsManager.SaveSettings(Settings);

            // 🔷 Set global XML Directory
            XmlDirectory = Settings.BrowseDb;

            // 🔷 Ensure DB structure exists for SaveDb path
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

        private void processEODToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Info("🧾 Manual EOD processing started...");

                // Use the DeniedFiles directory and SaveDb path from Settings
                string deniedPath = Path.Combine(Settings.BrowseDb, "DeniedFiles");

                // Run EOD processing and get the summary path
                string summaryPath = EodXmlProcessor.ProcessDeniedEodFiles(deniedPath, Settings.SaveDb);

                Logger.Info("✅ Manual EOD processing completed.");
                MessageBox.Show("EOD processing completed successfully.", "Process EOD", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // ✅ Optional: Open the generated summary file after processing
                if (File.Exists(summaryPath))
                {
                    System.Diagnostics.Process.Start("notepad.exe", summaryPath);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"❌ EOD processing failed: {ex.Message}", "frm_Main");
                MessageBox.Show($"EOD processing failed:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
