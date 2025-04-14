using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teklif_Hazırlayıcı
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string projectPath = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\.."));
            AppDomain.CurrentDomain.SetData("DataDirectory", projectPath);



            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new login());
        }
    }
}
