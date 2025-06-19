using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Teklif_Hazırlayıcı.Helpers
{
    public static class AssemblyInfoHelper
    {
        public static void UpdateAssemblyVersion(string newVersion)
        {
            if (string.IsNullOrWhiteSpace(newVersion))
                throw new ArgumentException("newVersion");

            string assemblyInfoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Properties", "AssemblyInfo.cs");
            if (!File.Exists(assemblyInfoPath))
                return;

            string content = File.ReadAllText(assemblyInfoPath, Encoding.UTF8);
            content = Regex.Replace(content, @"AssemblyVersion\(""[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+""\)", $"AssemblyVersion(\"{newVersion}\")");
            File.WriteAllText(assemblyInfoPath, content, Encoding.UTF8);
        }
    }
}
