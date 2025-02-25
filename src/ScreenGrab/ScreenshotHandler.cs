using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ScreenGrab
{
    public static class ScreenshotHandler
    {
        public static void CaptureScreen()
        {
            using (Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bmp.Size);
                }

                // Save with timestamp
                string fileName = $"Screenshot_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.png";
                string savePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), fileName);
                bmp.Save(savePath, ImageFormat.Png);

                MessageBox.Show($"Screenshot saved: {savePath}", "ScreenGrab", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
