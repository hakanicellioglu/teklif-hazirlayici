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

            Color fromColor = form.BackColor;
            Color toColor = _isDarkMode ? darkBackground : lightBackground;

            StartBackgroundTransition(form, fromColor, toColor);

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

            Color lightColor1 = SystemColors.ControlLightLight;
            Color lightColor2 = SystemColors.ControlLight;
            Color lightColor3 = SystemColors.ControlDark;
            Color lightColor4 = SystemColors.ControlText;
            Color lightColor5 = SystemColors.ControlText;

            Color background = _isDarkMode ? darkColor1 : lightColor1;
            Color panel = _isDarkMode ? darkColor2 : lightColor2;
            Color border = _isDarkMode ? darkColor3 : lightColor3;
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
                        ctrl.BackColor = panel;
                        ctrl.ForeColor = textPrimary;
                    }

                    if (tags.Contains("button"))
                    {
                        if (tags.Contains("primary"))
                        {
                            ctrl.BackColor = border;
                            ctrl.ForeColor = textPrimary;
                        }
                        else if (tags.Contains("secondary"))
                        {
                            ctrl.BackColor = panel;
                            ctrl.ForeColor = textPrimary;
                        }
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

        private static Timer transitionTimer;
        private static Color startColor;
        private static Color targetColor;
        private static Form targetForm;
        private static int transitionStep = 0;
        private const int MaxSteps = 30; // ne kadar adımda geçecek (hız ayarı)

        private static void StartBackgroundTransition(Form form, Color from, Color to)
        {
            startColor = from;
            targetColor = to;
            targetForm = form;
            transitionStep = 0;

            if (transitionTimer == null)
            {
                transitionTimer = new Timer();
                transitionTimer.Interval = 15; // daha hızlı / yavaş için ayarla
                transitionTimer.Tick += TransitionTimer_Tick;
            }

            transitionTimer.Start();
        }

        private static void TransitionTimer_Tick(object sender, EventArgs e)
        {
            if (transitionStep >= MaxSteps)
            {
                transitionTimer.Stop();
                targetForm.BackColor = targetColor;
                return;
            }

            float percent = (float)transitionStep / MaxSteps;
            int r = (int)(startColor.R + (targetColor.R - startColor.R) * percent);
            int g = (int)(startColor.G + (targetColor.G - startColor.G) * percent);
            int b = (int)(startColor.B + (targetColor.B - startColor.B) * percent);

            targetForm.BackColor = Color.FromArgb(r, g, b);

            transitionStep++;
        }

    }
}
