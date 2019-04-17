﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_MGMT
{
    public class NT3PathListBox : ListBox
    {
        #region Databytes
        static readonly byte[] NT3FolderBitmapBytes = new byte[] {
            0x42, 0x4D, 0xF6, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x76, 0x00, 0x00, 0x00, 0x28, 0x00,
            0x00, 0x00, 0x90, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00,
            0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x80,
            0x00, 0x00, 0x00, 0x80, 0x80, 0x00, 0x80, 0x00, 0x00, 0x00, 0x80, 0x00, 0x80, 0x00, 0x80, 0x80,
            0x00, 0x00, 0x80, 0x80, 0x80, 0x00, 0xC0, 0xC0, 0xC0, 0x00, 0x00, 0x00, 0xFF, 0x00, 0x00, 0xFF,
            0x00, 0x00, 0x00, 0xFF, 0xFF, 0x00, 0xFF, 0x00, 0x00, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0xFF,
            0x00, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0xCC, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x0C, 0xCC, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x00, 0x88,
            0x88, 0x88, 0x88, 0x88, 0x80, 0xCC, 0x00, 0x88, 0x88, 0x88, 0x88, 0x88, 0x80, 0xCC, 0x08, 0x88,
            0x88, 0x88, 0x88, 0x88, 0x88, 0x0C, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x0F, 0x0B, 0xFB, 0xFB, 0xFB, 0xFB, 0xF0, 0xCC, 0x0F, 0x0B,
            0x8B, 0x8B, 0x8B, 0x8B, 0x80, 0xCC, 0x0B, 0xFB, 0xFB, 0xFB, 0xFB, 0xFB, 0xFB, 0x0C, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0B, 0x0F,
            0xBF, 0xBF, 0xBF, 0xBF, 0xBF, 0x0C, 0x00, 0x08, 0xB8, 0xB8, 0xB8, 0xB8, 0xB8, 0x0C, 0x0F, 0xBF,
            0xBF, 0xBF, 0xBF, 0xBF, 0xBF, 0x0C, 0x07, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x70, 0x07, 0x77,
            0x77, 0x77, 0x77, 0x77, 0x77, 0x70, 0x07, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x70, 0x07, 0x77,
            0x77, 0x77, 0x77, 0x77, 0x77, 0x70, 0x07, 0x77, 0x77, 0x77, 0x77, 0x77, 0x77, 0x70, 0x07, 0x77,
            0x77, 0x77, 0x77, 0x77, 0x77, 0x70, 0x0F, 0xB0, 0xFB, 0xFB, 0xFB, 0xFB, 0xFB, 0x0C, 0x0F, 0x00,
            0x8B, 0x8B, 0x8B, 0x8B, 0x8B, 0x0C, 0x0B, 0xFB, 0xFB, 0xFB, 0xFB, 0xFB, 0xFB, 0x0C, 0x0F, 0x88,
            0x88, 0x88, 0x88, 0x88, 0x88, 0x70, 0x0F, 0x00, 0x88, 0x88, 0x88, 0x88, 0x88, 0x70, 0x0F, 0x88,
            0x88, 0x88, 0x88, 0x89, 0x98, 0x70, 0x0F, 0x88, 0x80, 0x00, 0x88, 0x88, 0x88, 0x70, 0x0F, 0x88,
            0x08, 0x08, 0x08, 0x08, 0x88, 0x70, 0x0F, 0x88, 0x88, 0xFF, 0xF8, 0x88, 0x88, 0x70, 0x0B, 0xF0,
            0xBF, 0xBF, 0xBF, 0xBF, 0xBF, 0xB0, 0x00, 0xF0, 0xB8, 0xB8, 0xB8, 0xB8, 0xB8, 0xB0, 0x0F, 0xBF,
            0xBF, 0xBF, 0xBF, 0xBF, 0xBF, 0x0C, 0x0F, 0x88, 0x80, 0x00, 0x00, 0x08, 0x88, 0x70, 0x0F, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x70, 0x0F, 0x88, 0x80, 0x00, 0x08, 0x88, 0x88, 0x70, 0x0F, 0x88,
            0x88, 0x08, 0x88, 0x88, 0x88, 0x70, 0x0F, 0x80, 0x00, 0x00, 0x00, 0x00, 0x88, 0x70, 0x0F, 0x88,
            0x87, 0x77, 0x88, 0x88, 0x88, 0x70, 0x0F, 0xBF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x0F,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0B, 0xFB, 0xFB, 0xFB, 0xFB, 0xFB, 0xFB, 0x0C, 0x0F, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x70, 0x0F, 0x88, 0x88, 0x88, 0x88, 0x88, 0x88, 0x70, 0x0F, 0x88,
            0x08, 0x8F, 0x80, 0x88, 0x88, 0x70, 0x0F, 0x88, 0x80, 0x00, 0x88, 0x88, 0x88, 0x70, 0x0F, 0x80,
            0x80, 0x80, 0x80, 0x80, 0x88, 0x70, 0x0F, 0x88, 0x88, 0x7F, 0xF8, 0x88, 0x88, 0x70, 0x0B, 0xFB,
            0xFB, 0xFB, 0xFB, 0xFB, 0xF0, 0xCC, 0x00, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xCC, 0x0F, 0xBF,
            0xBF, 0xBF, 0xBF, 0xBF, 0xBF, 0x0C, 0x0F, 0x88, 0x80, 0x00, 0x00, 0x08, 0x88, 0x70, 0x0F, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x70, 0x0F, 0x00, 0x78, 0xF7, 0x87, 0x00, 0x88, 0x70, 0x0F, 0x00,
            0x00, 0x80, 0x00, 0x00, 0x08, 0x70, 0x0F, 0x80, 0x00, 0x00, 0x00, 0x00, 0x88, 0x70, 0x0F, 0x8F,
            0xF7, 0x77, 0xFF, 0xFF, 0xFF, 0x70, 0x0F, 0xBF, 0xBF, 0xBF, 0xBF, 0xBF, 0xB0, 0xCC, 0x0F, 0x0F,
            0x0F, 0x0F, 0x0F, 0x0F, 0x00, 0xCC, 0x0B, 0xFB, 0xFB, 0xFB, 0xFB, 0xFB, 0xFB, 0x0C, 0x0F, 0x88,
            0x88, 0x88, 0x88, 0x88, 0x80, 0x70, 0x0F, 0x88, 0x88, 0x88, 0x88, 0x88, 0x88, 0x70, 0x0F, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x88, 0x70, 0x0F, 0x88, 0x88, 0x88, 0x88, 0x88, 0x88, 0x70, 0x0F, 0x88,
            0x08, 0x08, 0x08, 0x08, 0x88, 0x70, 0x0F, 0x77, 0x77, 0x87, 0x77, 0x77, 0x78, 0x70, 0x0B, 0xFB,
            0xFB, 0xFB, 0x00, 0x00, 0x0C, 0xCC, 0x00, 0xF0, 0xF0, 0xF0, 0x00, 0x00, 0x0C, 0xCC, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0xCC, 0x0F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x70, 0x0F, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x70, 0x0F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x70, 0x0F, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x70, 0x0F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x70, 0x0F, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x70, 0xC0, 0xBF, 0xBF, 0xB0, 0xCC, 0xCC, 0xCC, 0xCC, 0xC0, 0x0F,
            0x0F, 0x00, 0xCC, 0xCC, 0xCC, 0xCC, 0xC0, 0xBF, 0xBF, 0xB0, 0xCC, 0xCC, 0xCC, 0xCC, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xCC, 0x00,
            0x00, 0x0C, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x00, 0x00, 0x0C, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x00,
            0x00, 0x0C, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
            0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC};
        #endregion

        public class NT3PathElement
        {
            public string Name;
            public int Indent;
            public FolderState State;
            public string FullFolderName;

            public NT3PathElement()
            {

            }

            public NT3PathElement(string Name, int Indent, FolderState State, string FullFolderName)
            {
                this.Name = Name;
                this.Indent = Indent;
                this.State = State;
                this.FullFolderName = FullFolderName;
            }
        }

        public enum FolderState : int
        {
            Opened = 0, //Light opened folder - index 0
            CurrentFolder = 1, //Darken opened folder - index 1
            SubFolder = 2 //Closed folder - index 2
        }

        Bitmap NT3FolderBitmap;

        public NT3PathListBox()
        {
            NT3FolderBitmap = (Bitmap)Bitmap.FromStream(new MemoryStream(NT3FolderBitmapBytes));
            NT3FolderBitmap.MakeTransparent(Color.FromArgb(0, 0, 255));
            this.DrawMode = DrawMode.OwnerDrawVariable;
            this.DrawItem += NT3PathListBox_DrawItem;
            this.MeasureItem += NT3PathListBox_MeasureItem;
            this.ScrollAlwaysVisible = true;
        }

        private void NT3PathListBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (this.Font.Height < 16)
                e.ItemHeight = 16;
            else
                e.ItemHeight = this.Font.Height;
        }

        public void ListPath(string Path, List<string> Elements)
        {
            int currentselectedindex = this.SelectedIndex;
            this.Items.Clear();
            int Indent = 0;
            string FFN = "";
            foreach (string P in Path.Split('\\'))
            {
                if (P.Trim() == "")
                    continue;
                FFN += P + "\\";
                this.Items.Add(new NT3PathElement(P + (Indent == 0 ? "\\" : ""), Indent, FolderState.Opened,FFN));
                Indent++;
            }
            ((NT3PathElement)this.Items[this.Items.Count - 1]).State = FolderState.CurrentFolder;
            foreach(string P in Elements)
            {
                this.Items.Add(new NT3PathElement(P, Indent, FolderState.SubFolder, FFN + P + "\\"));
            }
            if (currentselectedindex == -1)
                return;
            if (currentselectedindex > this.Items.Count-1)
                return;
            this.SelectedIndex = currentselectedindex;
        }

        private void NT3PathListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (this.Items.Count == 0)
                return;
            if (e.Index == -1)
                return;
            NT3PathElement p = (NT3PathElement)this.Items[e.Index];
            e.DrawBackground();
            int YOffset = 0;
            if (e.Bounds.Height > 16)
                YOffset = (e.Bounds.Height - 16) / 2;
            int XIndent = 7 * p.Indent;
            e.Graphics.DrawImage(NT3FolderBitmap, new Rectangle(XIndent, e.Bounds.Top + YOffset, 16, 16), new Rectangle(16 * (int)p.State, 0, 16, 16), GraphicsUnit.Pixel);
            e.Graphics.DrawString(p.Name, e.Font, SystemBrushes.WindowText, XIndent + 17, e.Bounds.Top + YOffset);
            e.DrawFocusRectangle();
        }
    }
}
