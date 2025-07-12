using Mall_Linking_Alliance.Helpers;
using Mall_Linking_Alliance.Model;
using System;
using System.IO;
using System.Windows.Forms;

namespace Mall_Linking_Alliance
{
    public partial class Config : Form
    {
        private TblSettings _currentSettings;

        public Config()
        {
            InitializeComponent();
            LoadSettings();
        }

        /// <summary>
        /// Load the current settings from the database and populate the textboxes.
        /// </summary>
        private void LoadSettings()
        {
            _currentSettings = SettingsManager.LoadSettings();

            if (_currentSettings != null)
            {
                txt_TenantId.Text = _currentSettings.TenantId?.ToString() ?? "";
                txt_TenantKey.Text = _currentSettings.TenantKey ?? "";
                txt_TmId.Text = _currentSettings.TmId?.ToString() ?? "";
                txt_Doc.Text = _currentSettings.Doc ?? "";
                txt_BrowseDb.Text = _currentSettings.BrowseDb ?? "";
                txt_SaveDb.Text = _currentSettings.SaveDb ?? "";
            }
            else
            {
                _currentSettings = new TblSettings();
            }
        }

        /// <summary>
        /// Save button clicked — validate and persist the data to database.
        /// </summary>
        private void btn_Save_Click(object sender, EventArgs e)
        {
            _currentSettings.TenantId = int.TryParse(txt_TenantId.Text.Trim(), out int tenantId) ? tenantId : (int?)null;
            _currentSettings.TenantKey = txt_TenantKey.Text.Trim();
            _currentSettings.TmId = int.TryParse(txt_TmId.Text.Trim(), out int tmId) ? tmId : (int?)null;
            _currentSettings.Doc = txt_Doc.Text.Trim();
            _currentSettings.BrowseDb = txt_BrowseDb.Text.Trim();
            _currentSettings.SaveDb = txt_SaveDb.Text.Trim();

            SettingsManager.SaveSettings(_currentSettings);

            MessageBox.Show("✅ Settings saved successfully.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        /// <summary>
        /// Browse for folder to set BrowseDb path.
        /// </summary>
        private void btn_BrowseDb_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "SQLite Database (*.db)|*.db|All files (*.*)|*.*";
                ofd.Title = "Select or create SQLite Database file";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txt_SaveDb.Text = ofd.FileName;
                }
            }
        }

        /// <summary>
        /// Browse for folder to set SaveDb path.
        /// </summary>
        private void btn_SaveDb_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select folder for SaveDb";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txt_SaveDb.Text = fbd.SelectedPath;
                }
            }
        }
    }
}
