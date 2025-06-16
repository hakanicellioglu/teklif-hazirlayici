using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teklif_Hazırlayıcı.Helpers
{
    public static class ThemeManager
    {
        private static bool _isDarkMode = false;

        public static void ToggleTheme(Form form)
        {
            SetTheme(form, !_isDarkMode);
        }

        public static void SetTheme(Form form, bool darkMode)
        {
            _isDarkMode = darkMode;

            EnsureTags(form);

            Color darkBackground = SystemColors.ControlDarkDark;
            Color lightBackground = SystemColors.ControlLightLight;

            Color backgroundColor = _isDarkMode ? darkBackground : lightBackground;

            form.BackColor = backgroundColor;

            ApplyTheme(form);
        }

        public static void ApplyThemeToAllOpenForms(bool darkMode)
        {
            foreach (Form form in Application.OpenForms)
            {
                SetTheme(form, darkMode);
            }
        }

        private static void EnsureTags(Control parent)
        {
            if (parent.Tag == null)
            {
                if (parent is Label)
                    parent.Tag = "label";
                else if (parent is Button)
                    parent.Tag = "button";
                else if (parent is TextBox || parent is ComboBox || parent is MaskedTextBox || parent is NumericUpDown)
                    parent.Tag = "input";
                else if (parent is CheckBox)
                    parent.Tag = "checkbox";
                else if (parent is LinkLabel)
                    parent.Tag = "link";
                else if (parent is Panel)
                    parent.Tag = "panel";
                else if (parent is DataGridView)
                    parent.Tag = "grid";
            }

            foreach (Control ctrl in parent.Controls)
            {
                EnsureTags(ctrl);
            }
        }


        private static void ApplyTheme(Control parent)
        {
            Color darkColor1 = SystemColors.ControlDarkDark;
            Color darkColor2 = SystemColors.ControlDark;
            Color darkColor3 = SystemColors.ControlLight;
            Color darkColor4 = SystemColors.ControlLight;
            Color darkColor5 = SystemColors.ControlLight;
            Color darkSecondary = SystemColors.Control;

            Color lightColor1 = SystemColors.ControlLightLight;
            Color lightColor2 = SystemColors.ControlLight;
            Color lightColor3 = SystemColors.ControlDark;
            Color lightColor4 = SystemColors.ControlText;
            Color lightColor5 = SystemColors.ControlText;

            Color background = _isDarkMode ? darkColor1 : lightColor1;
            Color panel = _isDarkMode ? darkColor2 : lightColor2;
            Color border = _isDarkMode ? darkColor3 : lightColor3;
            Color secondary = _isDarkMode ? darkSecondary : lightColor2;
            Color textPrimary = _isDarkMode ? darkColor4 : lightColor5;
            Color textSecondary = _isDarkMode ? darkColor3 : lightColor4;

            parent.BackColor = background;
            parent.ForeColor = textPrimary;

            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl.Tag != null)
                {
                    var tags = ctrl.Tag.ToString().Split(' ').Select(t => t.Trim().ToLower());

                    if (tags.Contains("label"))
                        ctrl.ForeColor = textPrimary;

                    if (tags.Contains("input"))
                    {
                        // Make inputs blend with their parent background
                        ctrl.BackColor = ctrl.Parent?.BackColor ?? background;
                        ctrl.ForeColor = textPrimary;
                    }

                    if (tags.Contains("button"))
                    {
                        if (tags.Contains("primary"))
                        {
                            ctrl.BackColor = border;
                            ctrl.ForeColor = textSecondary;
                        }
                        else if (tags.Contains("secondary"))
                        { 
                            ctrl.BackColor = secondary;
                            ctrl.ForeColor = textPrimary;
                        }
                    }

                    if (tags.Contains("grid") && ctrl is DataGridView grid)
                    {
                        grid.BackgroundColor = background;
                        grid.ForeColor = textPrimary;
                        grid.GridColor = border;
                        grid.ColumnHeadersDefaultCellStyle.BackColor = panel;
                        grid.ColumnHeadersDefaultCellStyle.ForeColor = textPrimary;
                        grid.DefaultCellStyle.BackColor = background;
                        grid.DefaultCellStyle.ForeColor = textPrimary;
                        grid.DefaultCellStyle.SelectionBackColor = secondary;
                        grid.DefaultCellStyle.SelectionForeColor = textPrimary;
                        grid.EnableHeadersVisualStyles = false;
                    }

                    if (tags.Contains("icon"))
                    {
                        ctrl.BackColor = Color.Transparent;
                        ctrl.ForeColor = textPrimary;
                    }

                    if (tags.Contains("checkbox"))
                        ctrl.ForeColor = textPrimary;

                    if (tags.Contains("link") && ctrl is LinkLabel link)
                        link.LinkColor = textSecondary;
                }

                if (ctrl.HasChildren)
                    ApplyTheme(ctrl);
            }
        }

    }
}
