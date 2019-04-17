using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FoxSDC_Agent_UI
{
    //[Designer("System.Windows.Forms.Design.FormDocumentDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
    public class FForm : Form
    {
        public FForm()
        {
            this.Load += FForm_Load;
        }

        void FForm_Load(object sender, EventArgs e)
        {
            if (this.DesignMode == true)
                return;
            this.Font = SystemFonts.CaptionFont;
            switch (this.StartPosition)
            {
                case FormStartPosition.CenterParent:
                    this.CenterToParent(); break;
                case FormStartPosition.CenterScreen:
                    this.CenterToScreen(); break;
            }
        }

    }
}
