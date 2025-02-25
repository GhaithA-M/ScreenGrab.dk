using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace ScreenGrab
{
    public static class ScreenshotHandler
    {
        private static string lastSaveFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static bool alwaysSaveToLastFolder = false; // User preference

        public static void SaveOrCopyScreenshot(Bitmap bmp)
        {
            using (SaveForm saveForm = new SaveForm(bmp))
            {
                saveForm.ShowDialog();
            }
        }

        public static void SaveScreenshot(Bitmap bmp)
        {
            string savePath;

            if (!alwaysSaveToLastFolder || string.IsNullOrEmpty(lastSaveFolder)) 
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Select a folder to save your screenshots";
                    fbd.SelectedPath = lastSaveFolder; // Suggest the last used folder

                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        lastSaveFolder = fbd.SelectedPath; // Store last used folder
                    }
                    else
                    {
                        return; // User canceled the save
                    }
                }
            }

            // Generate file name
            string fileName = $"Screenshot_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.png";
            savePath = Path.Combine(lastSaveFolder, fileName);

            // Ensure folder exists
            Directory.CreateDirectory(lastSaveFolder);

            // Save the screenshot
            bmp.Save(savePath, ImageFormat.Png);

            MessageBox.Show($"Screenshot saved: {savePath}", "ScreenGrab", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void CopyToClipboard(Bitmap bmp)
        {
            Clipboard.SetImage(bmp);
            MessageBox.Show("Screenshot copied to clipboard!", "ScreenGrab", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void SetAlwaysSave(bool saveAutomatically)
        {
            alwaysSaveToLastFolder = saveAutomatically;
        }
    }
}
