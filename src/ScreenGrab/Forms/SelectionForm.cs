using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ScreenGrab
{
    public class SelectionForm : Form
    {
        private Rectangle selectionBox;
        private Point dragStart;
        private bool selecting = false;
        private bool moving = false;
        private bool resizing = false;
        private Bitmap? frozenScreen;
        private Cursor defaultCursor = Cursors.Cross;
        private Cursor moveCursor = Cursors.SizeAll;
        private Cursor resizeCursor = Cursors.SizeNWSE;

        public SelectionForm()
        {
            this.DoubleBuffered = true;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.BackColor = Color.Black;
            this.Opacity = 0.5;
            this.Cursor = defaultCursor;
            this.KeyPreview = true;
            this.KeyDown += SelectionForm_KeyDown;

            // Freeze screen and prepare for selection
            CaptureFullScreen();
        }

        private void CaptureFullScreen()
        {
            frozenScreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (Graphics g = Graphics.FromImage(frozenScreen))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, frozenScreen.Size);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (selectionBox.Contains(e.Location))
            {
                if (IsOnResizeEdge(e.Location))
                {
                    resizing = true;
                }
                else
                {
                    moving = true;
                    dragStart = new Point(e.X - selectionBox.X, e.Y - selectionBox.Y);
                }
            }
            else
            {
                selectionBox = new Rectangle(e.Location, new Size(0, 0));
                selecting = true;
            }
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (selecting)
            {
                selectionBox = new Rectangle(
                    Math.Min(dragStart.X, e.X),
                    Math.Min(dragStart.Y, e.Y),
                    Math.Abs(e.X - dragStart.X),
                    Math.Abs(e.Y - dragStart.Y)
                );
            }
            else if (moving)
            {
                selectionBox.X = e.X - dragStart.X;
                selectionBox.Y = e.Y - dragStart.Y;
            }
            else if (resizing)
            {
                selectionBox.Width = Math.Max(50, e.X - selectionBox.X);
                selectionBox.Height = Math.Max(50, e.Y - selectionBox.Y);
            }

            UpdateCursor(e.Location);
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            selecting = false;
            moving = false;
            resizing = false;
            Invalidate();
        }

        private void UpdateCursor(Point mousePos)
        {
            if (IsOnResizeEdge(mousePos))
                this.Cursor = resizeCursor;
            else if (selectionBox.Contains(mousePos))
                this.Cursor = moveCursor;
            else
                this.Cursor = defaultCursor;
        }

        private bool IsOnResizeEdge(Point mousePos)
        {
            int edgeSize = 10;
            return (Math.Abs(mousePos.X - selectionBox.Right) < edgeSize && Math.Abs(mousePos.Y - selectionBox.Bottom) < edgeSize);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (frozenScreen != null)
            {
                e.Graphics.DrawImage(frozenScreen, 0, 0);
            }

            using (Brush darkBrush = new SolidBrush(Color.FromArgb(180, Color.Black)))
            {
                e.Graphics.FillRectangle(darkBrush, this.ClientRectangle);
            }

            if (selectionBox.Width > 0 && selectionBox.Height > 0)
            {
                e.Graphics.SetClip(selectionBox);
                e.Graphics.DrawImage(frozenScreen, 0, 0);
                e.Graphics.ResetClip();

                using (Pen dottedPen = new Pen(Color.White, 2) { DashPattern = new float[] { 5, 5 } })
                {
                    e.Graphics.DrawRectangle(dottedPen, selectionBox);
                }
            }
        }

        private void SelectionForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                SaveScreenshot();
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                CopyToClipboard();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void SaveScreenshot()
        {
            if (selectionBox.Width < 5 || selectionBox.Height < 5)
                return;

            using (Bitmap bmp = new Bitmap(selectionBox.Width, selectionBox.Height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(selectionBox.Location, Point.Empty, selectionBox.Size);
                }

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "PNG Image|*.png";
                    sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    sfd.FileName = $"Screenshot_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.png";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        bmp.Save(sfd.FileName, ImageFormat.Png);
                    }
                }
            }

            this.Close();
        }

        private void CopyToClipboard()
        {
            if (selectionBox.Width < 5 || selectionBox.Height < 5)
                return;

            using (Bitmap bmp = new Bitmap(selectionBox.Width, selectionBox.Height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(selectionBox.Location, Point.Empty, selectionBox.Size);
                }
                Clipboard.SetImage(bmp);
                MessageBox.Show("Screenshot copied to clipboard!", "ScreenGrab", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.Close();
        }
    }
}
