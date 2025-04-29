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
            _isDarkMode = !_isDarkMode;

            Color darkBackground = Color.FromArgb(40, 40, 40);
            Color lightBackground = Color.FromArgb(0, 56, 64);

            Color fromColor = form.BackColor;
            Color toColor = _isDarkMode ? darkBackground : lightBackground;

            StartBackgroundTransition(form, fromColor, toColor);

            ApplyTheme(form);
        }


        private static void ApplyTheme(Control parent)
        {
            Color darkColor1 = Color.FromArgb(40, 40, 40);
            Color darkColor2 = Color.FromArgb(70, 70, 70);
            Color darkColor3 = Color.FromArgb(125, 125, 125);
            Color darkColor4 = Color.FromArgb(225, 225, 225);
            Color darkColor5 = Color.FromArgb(255, 255, 255);

            Color lightColor1 = Color.FromArgb(0, 56, 64);
            Color lightColor2 = Color.FromArgb(0, 90, 91);
            Color lightColor3 = Color.FromArgb(0, 115, 105);
            Color lightColor4 = Color.FromArgb(0, 140, 114);
            Color lightColor5 = Color.FromArgb(2, 166, 118);

            Color background = _isDarkMode ? darkColor1 : lightColor1;
            Color panel = _isDarkMode ? darkColor2 : lightColor2;
            Color border = _isDarkMode ? darkColor3 : lightColor3;
            Color textPrimary = _isDarkMode ? darkColor4 : lightColor5;
            Color textSecondary = _isDarkMode ? darkColor3 : lightColor4;

            parent.BackColor = background;

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
                            ctrl.ForeColor = _isDarkMode ? Color.Black : Color.White;
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
