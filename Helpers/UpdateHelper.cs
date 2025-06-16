using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace Teklif_Hazırlayıcı.Helpers
{
    /// <summary>
    /// Provides simple version checking and update capability.
    /// </summary>
    public static class UpdateHelper
    {
        private const string VersionInfoUrl = "https://github.com/hakanicellioglu/teklif-hazirlayici/version.txt";
        private const string InstallerUrl = "https://github.com/hakanicellioglu/teklif-hazirlayici/setup.exe";

        /// <summary>
        /// Checks for a newer version and downloads it if available.
        /// </summary>
        public static void CheckForUpdates()
        {
            try
            {
                using (var client = new WebClient())
                {
                    string versionString = client.DownloadString(VersionInfoUrl).Trim();
                    Version latestVersion = new Version(versionString);
                    Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

                    if (latestVersion > currentVersion)
                    {
                        DialogResult result = MessageBox.Show(
                            "Yeni bir sürüm bulundu. Güncelleme yapmak ister misiniz?",
                            "Güncelleme",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            string tempFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "teklif-hazirlayici-update.exe");
                            client.DownloadFile(InstallerUrl, tempFile);
                            Process.Start(tempFile);
                            Environment.Exit(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError($"Güncelleme kontrolü başarısız: {ex.Message}");
            }
        }
    }
}
