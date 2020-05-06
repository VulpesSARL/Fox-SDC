using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_Agent_UI
{
    public partial class frmNotes : FForm
    {
        string NoteText;
        string DontShowAgainRegistryEntry;

        #region Beep!

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool MessageBeep(beepType uType);

        enum beepType : uint
        {
            /// <summary>
            /// A simple windows beep
            /// </summary>            
            SimpleBeep = 0xFFFFFFFF,
            /// <summary>
            /// A standard windows OK beep
            /// </summary>
            OK = 0x00,
            /// <summary>
            /// A standard windows Question beep
            /// </summary>
            Question = 0x20,
            /// <summary>
            /// A standard windows Exclamation beep
            /// </summary>
            Exclamation = 0x30,
            /// <summary>
            /// A standard windows Asterisk beep
            /// </summary>
            Asterisk = 0x40,
        }

        #endregion

        public frmNotes(string NoteText, string DontShow)
        {
            DontShowAgainRegistryEntry = DontShow;
            this.NoteText = NoteText;
            InitializeComponent();
        }

        public bool ShouldShowThis()
        {
            using (RegistryKey r = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Fox\\SDC\\Hints"))
            {
                if (r == null)
                    return (true);
                string e = r.GetValue(DontShowAgainRegistryEntry, 0).ToString();
                if (e == "1")
                    return (false);
            }
            return (true);
        }

        private void frmOldOSMessage_Load(object sender, EventArgs e)
        {
            txtMessage.Text = NoteText;
            txtMessage.SelectionStart = 0;
            txtMessage.SelectionLength = 0;
            MessageBeep(beepType.Asterisk);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (chkDontShowThisAgain.Checked == true)
            {
                using (RegistryKey r = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Fox\\SDC\\Hints"))
                {
                    if (r != null)
                    {
                        r.SetValue(DontShowAgainRegistryEntry, 1, RegistryValueKind.DWord);
                    }
                }
            }
            this.Close();
        }
    }
}
