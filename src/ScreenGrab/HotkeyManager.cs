using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScreenGrab
{
    public static class HotkeyManager
    {
        private const int WM_HOTKEY = 0x0312;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        public static void RegisterHotkeys(Form form)
        {
            RegisterHotKey(form.Handle, 1, 0, (uint)Keys.PrintScreen);
        }

        public static void ProcessHotkeyMessage(Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                Application.Run(new SelectionForm()); // Launch selection box
            }
        }
    }
}
