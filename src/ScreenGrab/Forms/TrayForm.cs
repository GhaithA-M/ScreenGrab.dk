using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScreenGrab
{
    public class TrayForm : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

        public TrayForm()
        {
            // Hide main window
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            // Create a right-click menu
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Take Screenshot", null, OnScreenshot);
            trayMenu.Items.Add("Exit", null, OnExit);

            // Create a system tray icon
            trayIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application, // Change this to a custom icon later
                ContextMenuStrip = trayMenu,
                Visible = true,
                Text = "ScreenGrab"
            };
        }

        private void OnScreenshot(object? sender, EventArgs e)
        {
            ScreenshotHandler.CaptureScreen();
        }

        private void OnExit(object? sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

    }
}
