using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScreenGrab
{
    public class SaveForm : Form
    {
        private Bitmap screenshot;
        private Button saveButton, copyButton;
        private CheckBox autoSaveCheckBox;

        public SaveForm(Bitmap bmp)
        {
            this.screenshot = bmp;
            this.Text = "Save or Copy Screenshot";
            this.Size = new Size(300, 150);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;

            saveButton = new Button() { Text = "Save", Left = 30, Width = 100, Top = 20 };
            copyButton = new Button() { Text = "Copy", Left = 160, Width = 100, Top = 20 };
            autoSaveCheckBox = new CheckBox() { Text = "Always save to this folder", Left = 30, Top = 60 };

            saveButton.Click += SaveButton_Click;
            copyButton.Click += CopyButton_Click;
            autoSaveCheckBox.CheckedChanged += AutoSaveCheckBox_CheckedChanged;

            this.Controls.Add(saveButton);
            this.Controls.Add(copyButton);
            this.Controls.Add(autoSaveCheckBox);
        }

        private void SaveButton_Click(object? sender, EventArgs e)
        {
            ScreenshotHandler.SaveScreenshot(screenshot);
            this.Close();
        }

        private void CopyButton_Click(object? sender, EventArgs e)
        {
            ScreenshotHandler.CopyToClipboard(screenshot);
            this.Close();
        }

        private void AutoSaveCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            ScreenshotHandler.SetAlwaysSave(autoSaveCheckBox.Checked);
        }
    }
}
