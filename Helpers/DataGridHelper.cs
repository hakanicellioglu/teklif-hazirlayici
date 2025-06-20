using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teklif_Hazırlayıcı.Helpers
{
    public static class DataGridHelper
    {
        public static void SetupGridColumnProperties(DataGridView grid)
        {
            foreach (DataGridViewColumn col in grid.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                col.Resizable = DataGridViewTriState.False;
            }
        }
        public static void RestoreColumnVisibility(DataGridView grid, Dictionary<string, bool> columnSettings)
        {
            foreach (DataGridViewColumn col in grid.Columns)
            {
                if (columnSettings.TryGetValue(col.Name, out bool visible))
                {
                    col.Visible = visible;
                }
            }
        }

    }

}
