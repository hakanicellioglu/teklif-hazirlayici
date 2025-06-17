using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using System.IO.Compression;

namespace Teklif_Hazırlayıcı.Helpers
{
    public static class UpdateHelper
    {
        private const string VersionInfoUrl = "https://raw.githubusercontent.com/hakanicellioglu/teklif-hazirlayici/main/version.txt";

        public static void CheckForUpdates()
        {
            try
            {
                using (var client = new WebClient())
                {
                    string versionContent = client.DownloadString(VersionInfoUrl);
                    string[] lines = versionContent
                        .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                    if (lines.Length == 0)
                    {
                        Debug.WriteLine("[HATA] version.txt boş");
                        return;
                    }

                    string versionString = lines[0].Trim();
                    string zipUrl = lines.Length > 1 ? lines[1].Trim() : string.Empty;

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
                            string tempDir = Path.Combine(Path.GetTempPath(), "TeklifHazirlayiciUpdate");
                            string zipPath = Path.Combine(tempDir, "publish.zip");

                            if (Directory.Exists(tempDir))
                                Directory.Delete(tempDir, true);

                            Directory.CreateDirectory(tempDir);

                            if (string.IsNullOrEmpty(zipUrl))
                                zipUrl = $"https://github.com/hakanicellioglu/teklif-hazirlayici/releases/download/{versionString}/publish.zip";

                            Debug.WriteLine($"[İNDİRME] Güncelleme arşivi indiriliyor: {zipUrl}");
                            client.DownloadFile(zipUrl, zipPath);

                            Debug.WriteLine($"[AÇMA] Zip içeriği çıkarılıyor: {tempDir}");
                            ZipFile.ExtractToDirectory(zipPath, tempDir);

                            string setupPath = Path.Combine(tempDir, "publish", "setup.exe");

                            if (File.Exists(setupPath))
                            {
                                Debug.WriteLine($"[ÇALIŞTIRMA] setup.exe başlatılıyor: {setupPath}");
                                Process.Start(setupPath);
                                Environment.Exit(0);
                            }
                            else
                            {
                                MessageHelper.ShowError("setup.exe bulunamadı. Güncelleme başlatılamadı.");
                            }
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
