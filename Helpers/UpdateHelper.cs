using System;
using System.Diagnostics;
using System.IO;
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
        private const string VersionInfoUrl = "https://raw.githubusercontent.com/hakanicellioglu/teklif-hazirlayici/main/version.txt";
        private const string InstallerUrl = "https://github.com/hakanicellioglu/teklif-hazirlayici/main/setup.exe";

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

                    // Güvenli dönüşüm
                    if (!Version.TryParse(versionString, out Version latestVersion))
                    {
                        Debug.WriteLine($"[HATA] Geçersiz sürüm dizesi: '{versionString}'");
                        MessageHelper.ShowError($"Geçersiz sürüm formatı: {versionString}");
                        return;
                    }

                    Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new Version("1.0.0.0");

                    Debug.WriteLine($"[SÜRÜM] Mevcut Sürüm: {currentVersion}");
                    Debug.WriteLine($"[SÜRÜM] Sunucudaki Sürüm: {latestVersion}");

                    if (latestVersion > currentVersion)
                    {
                        DialogResult result = MessageBox.Show(
                            "Yeni bir sürüm bulundu. Güncelleme yapmak ister misiniz?",
                            "Güncelleme",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            string tempFile = Path.Combine(Path.GetTempPath(), "teklif-hazirlayici-update.exe");
                            client.DownloadFile(InstallerUrl, tempFile);
                            Process.Start(tempFile);
                            Environment.Exit(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[HATA] Güncelleme kontrolü başarısız: {ex.Message}");
                MessageHelper.ShowError($"Güncelleme kontrolü başarısız: {ex.Message}");
            }
        }
    }
}
