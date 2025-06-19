using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Teklif_Hazırlayıcı.Helpers
{
    public static class AssemblyInfoHelper
    {
        public static void UpdateAssemblyVersion(string newVersion)
        {
            if (string.IsNullOrWhiteSpace(newVersion))
                throw new ArgumentException(nameof(newVersion));

            // Projenin kök klasörünü bir üst dizinden hesapla
            string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));

            // AssemblyInfo.cs dosyasının göreli yolu
            string assemblyInfoPath = Path.Combine(projectRoot, @"Properties", "AssemblyInfo.cs");

            if (!File.Exists(assemblyInfoPath))
                throw new FileNotFoundException("AssemblyInfo.cs dosyası bulunamadı.", assemblyInfoPath);

            string content = File.ReadAllText(assemblyInfoPath, Encoding.UTF8);

            // AssemblyVersion ve AssemblyFileVersion satırlarını güncelle
            content = Regex.Replace(
                content,
                @"Assembly(File)?Version\(""[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+""\)",
                match => $"Assembly{(match.Groups[1].Success ? "File" : "")}Version(\"{newVersion}\")"
            );

            File.WriteAllText(assemblyInfoPath, content, Encoding.UTF8);
        }
    }
}
