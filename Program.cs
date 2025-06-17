using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Teklif_Hazırlayıcı.DataAccess;
using Teklif_Hazırlayıcı.Forms;
using Teklif_Hazırlayıcı.Helpers;

namespace Teklif_Hazırlayıcı
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            try
            {
                DatabaseInitializer.Initialize();

                string projectPath = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\.."));
                AppDomain.CurrentDomain.SetData("DataDirectory", projectPath);

                await UpdateHelper.CheckForUpdates();

                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new login());
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageHelper.ShowError($"Hata oluştu: {ex.Message}");
            }
        }
    }
}
