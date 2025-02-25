using System;
using System.Windows.Forms;

namespace ScreenGrab
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            TrayForm trayApp = new TrayForm();
            HotkeyManager.RegisterHotkeys(trayApp);
            Application.Run(trayApp);
        }
    }
}
