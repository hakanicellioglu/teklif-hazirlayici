using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Win32;

namespace Teklif_Hazırlayıcı.Helpers
{
    public static class UpdateHelper
    {
        private const string VersionInfoUrl = "https://raw.githubusercontent.com/hakanicellioglu/teklif-hazirlayici/main/version.txt";
        private const string LocalPublishPath = @"\\server\ortak\publish";

        private static HttpClient CreateSecureClient()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (request, certificate, chain, sslPolicyErrors) =>
            {
                return sslPolicyErrors == SslPolicyErrors.None;
            };
            return new HttpClient(handler);
        }

        private static string GetUninstallCommand()
        {
            string productName = Application.ProductName;
            string[] registryPaths = new[]
            {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
            };

            foreach (var root in new[] { Registry.CurrentUser, Registry.LocalMachine })
            {
                foreach (var path in registryPaths)
                {
                    using (var key = root.OpenSubKey(path))
                    {
                        if (key == null) continue;
                        foreach (var subName in key.GetSubKeyNames())
                        {
                            using (var subKey = key.OpenSubKey(subName))
                            {
                                if (subKey == null) continue;
                                string displayName = subKey.GetValue("DisplayName") as string;
                                if (!string.IsNullOrEmpty(displayName) && displayName.IndexOf(productName, StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    return subKey.GetValue("UninstallString") as string;
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        private static void UninstallCurrentVersion()
        {
            string cmd = GetUninstallCommand();
            if (string.IsNullOrEmpty(cmd))
                return;

            try
            {
                string silentCmd = cmd;
                if (cmd.IndexOf("msiexec", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    if (!cmd.Contains("/qn", StringComparison.OrdinalIgnoreCase))
                        silentCmd += " /qn";
                }
                else if (!cmd.Contains("/S", StringComparison.OrdinalIgnoreCase) &&
                         !cmd.Contains("/silent", StringComparison.OrdinalIgnoreCase) &&
                         !cmd.Contains("/quiet", StringComparison.OrdinalIgnoreCase) &&
                         !cmd.Contains("/q", StringComparison.OrdinalIgnoreCase))
                {
                    silentCmd += " /S";
                }

                var startInfo = new ProcessStartInfo("cmd.exe", "/C \"" + silentCmd + "\"")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using (var proc = Process.Start(startInfo))
                {
                    proc?.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[HATA] Uygulama kaldırılırken hata: {ex.Message}");
            }
        }

        public static async Task CheckForUpdates()
        {
            try
            {
                string versionContent = null;
                bool localSource = false;
                string localVersionFile = Path.Combine(LocalPublishPath, "version.txt");

                if (File.Exists(localVersionFile))
                {
                    versionContent = File.ReadAllText(localVersionFile, Encoding.UTF8);
                    localSource = true;
                }
                else
                {
                    using (var client = CreateSecureClient())
                    {
                        if (!VersionInfoUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                        {
                            MessageHelper.ShowError("Güncelleme adresi güvenli değil. HTTPS kullanılmalıdır.");
                            return;
                        }

                        versionContent = await client.GetStringAsync(VersionInfoUrl);
                    }
                }


                string[] lines = versionContent
                    .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                    if (lines.Length == 0)
                    {
                        Debug.WriteLine("[HATA] version.txt boş");
                        return;
                    }

                    string versionLine = lines[0].Replace("\uFEFF", "").Trim();
                    string versionString = versionLine.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    string zipUrl = lines.Length > 1 ? lines[1].Trim() : string.Empty;
                    string expectedHash = lines.Length > 2 ? lines[2].Trim() : null;

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
                            {
                                zipUrl = localSource
                                    ? "publish.zip"
                                    : $"https://github.com/hakanicellioglu/teklif-hazirlayici/releases/download/{versionString}/publish.zip";
                            }

                            if (localSource && !zipUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                            {
                                string localZip = Path.IsPathRooted(zipUrl) ? zipUrl : Path.Combine(LocalPublishPath, zipUrl);
                                if (!File.Exists(localZip))
                                {
                                    MessageHelper.ShowError("Güncelleme arşivi bulunamadı.");
                                    return;
                                }
                                File.Copy(localZip, zipPath, true);
                            }
                            else
                            {
                                if (!zipUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                                {
                                    MessageHelper.ShowError("Güncelleme adresi güvenli değil. HTTPS kullanılmalıdır.");
                                    return;
                                }

                                using (var downloadClient = CreateSecureClient())
                                {
                                    Debug.WriteLine($"[İNDİRME] Güncelleme arşivi indiriliyor: {zipUrl}");
                                    var zipBytes = await downloadClient.GetByteArrayAsync(zipUrl);
                                    File.WriteAllBytes(zipPath, zipBytes);
                                }
                            }

                            if (!string.IsNullOrEmpty(expectedHash))
                            {
                                using (var sha256 = SHA256.Create())
                                using (var fs = File.OpenRead(zipPath))
                                {
                                    var actual = sha256.ComputeHash(fs);
                                    string actualHash = BitConverter.ToString(actual).Replace("-", "").ToLowerInvariant();
                                    if (!string.Equals(actualHash, expectedHash, StringComparison.OrdinalIgnoreCase))
                                    {
                                        MessageHelper.ShowError("İndirilen dosyanın bütünlük doğrulaması başarısız. Güncelleme iptal edildi.");
                                        return;
                                    }
                                }
                            }

                            Debug.WriteLine($"[AÇMA] Zip içeriği çıkarılıyor: {tempDir}");
                            ZipFile.ExtractToDirectory(zipPath, tempDir);

                            string setupPath = Path.Combine(tempDir, "publish", "setup.exe");

                            if (File.Exists(setupPath))
                            {
                                Debug.WriteLine($"[ÇALIŞTIRMA] setup.exe başlatılıyor: {setupPath}");
                                UninstallCurrentVersion();
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
            catch (Exception ex)
            {
                Debug.WriteLine($"[HATA] Güncelleme kontrolü başarısız: {ex.Message}");
                MessageHelper.ShowError($"Güncelleme kontrolü başarısız: {ex.Message}");
            }
        }
    }
}
