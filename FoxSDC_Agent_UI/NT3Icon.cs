using FoxSDC_Agent_UI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_Agent_UI
{
    public partial class frmNT3Icon : FForm
    {
        const int GapBmpToText = 5;

        const int WM_NCLBUTTONDOWN = 0xA1;
        const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        static extern bool ReleaseCapture();

        private void frmNT3Icon_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        Bitmap PaintBitmap(string Text, bool Selected)
        {
            Bitmap bmp = new Bitmap(100, 100);
            Graphics gra = Graphics.FromImage(bmp);
            SizeF fntsz = gra.MeasureString(Text, SystemFonts.DefaultFont);
            Bitmap Fox = Resources.LogoAloneSquare3232;

            Size PicSZ = new Size();
            PicSZ.Height = (int)fntsz.Height + Fox.Height + GapBmpToText;

            if (Fox.Width > fntsz.Width)
                PicSZ.Width = Fox.Width;
            else
                PicSZ.Width = (int)fntsz.Width;

            bmp = new Bitmap(PicSZ.Width, PicSZ.Height);
            gra = Graphics.FromImage(bmp);

            gra.Clear(this.TransparencyKey);
            gra.DrawImageUnscaledAndClipped(Fox, new Rectangle((bmp.Width / 2) - (Fox.Width / 2), 0, Fox.Width, Fox.Height));
            gra.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            StringFormat fmt = new StringFormat();
            fmt.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip | StringFormatFlags.FitBlackBox;
            if (Selected == false)
            {
                gra.DrawString(Text, SystemFonts.DefaultFont, new SolidBrush(SystemColors.ControlText), 0, Fox.Height + GapBmpToText, fmt);
            }
            else
            {
                gra.FillRectangle(new SolidBrush(SystemColors.Highlight), new RectangleF(0, Fox.Height + GapBmpToText, bmp.Width, bmp.Height - Fox.Height));
                gra.DrawString(Text, SystemFonts.DefaultFont, new SolidBrush(SystemColors.HighlightText), 0, Fox.Height + GapBmpToText, fmt);
            }
            return (bmp);
        }

        public frmNT3Icon()
        {
            InitializeComponent();
        }

        private void frmNT3Icon_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = PaintBitmap("Fox SDC", false);
            this.Width = this.BackgroundImage.Width;
            this.Height = this.BackgroundImage.Height;
            this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height;
            this.Left = Screen.PrimaryScreen.WorkingArea.Left;
        }

        private void frmNT3Icon_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.MainDLG.allowclose == true)
                return;
            if (e.CloseReason == CloseReason.WindowsShutDown || e.CloseReason == CloseReason.TaskManagerClosing)
                return;
            e.Cancel = true;
        }

        private void frmNT3Icon_MouseClick(object sender, MouseEventArgs e)
        {
            Program.MainDLG.cntxMain.Show(this.Left + e.Location.X, this.Top + e.Location.Y);
        }

        private void frmNT3Icon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Program.MainDLG.showStatusToolStripMenuItem_Click(sender, e);
        }

        private void frmNT3Icon_Activated(object sender, EventArgs e)
        {
            this.BackgroundImage = PaintBitmap("Fox SDC", true);
        }

        private void frmNT3Icon_Deactivate(object sender, EventArgs e)
        {
            this.BackgroundImage = PaintBitmap("Fox SDC", false);
        }
    }
}
