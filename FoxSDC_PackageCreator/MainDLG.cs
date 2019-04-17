using FoxSDC_Common;
using FoxSDC_PackageCreator.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_PackageCreator
{
    public partial class MainDLG : FForm
    {
        PKGRootData Package;
        bool Modified = false;
        string CurrentFile = "";

        void NewPackage()
        {
            this.Text = Program.AppTitle + " (Untitled)";
            CurrentFile = "";
            Package = new PKGRootData();
            Package.Folders = new List<PKGFolder>();
            Package.Files = new List<PKGFile>();
            Package.Description = "";
            Package.Title = "new package";
            Package.HeaderID = "FoxPackageScriptV1";
            Package.Outputfile = "New Package.foxpkg";
            Package.Script = Resources.PackageScriptTemplate;
            Package.PackageID = Guid.NewGuid().ToString();
            Package.NoReceipt = false;
            Package.VersionID = 1;
            PackageDataUtilities.AddNewFolder(Package, "%SYSTEMROOT%");
            PackageDataUtilities.AddNewFolder(Package, "%SYSTEMROOT%\\SYSTEM32");
            PackageDataUtilities.AddNewFolder(Package, "%USERPROFILE%");
            PackageDataUtilities.AddNewFolder(Package, "%TEMP%");
            PackageDataUtilities.AddNewFolder(Package, "%PUBLIC%");
            PackageDataUtilities.AddNewFolder(Package, "%PROGRAMFILES%");
            PackageDataUtilities.AddNewFolder(Package, "%PROGRAMDATA%");
            PackageDataUtilities.AddNewFolder(Package, "%LOCALAPPDATA%");
            PackageDataUtilities.AddNewFolder(Package, "%APPDATA%");
            PackageDataUtilities.AddNewFolder(Package, "%COMMONPROGRAMFILES%");
            PackageDataUtilities.AddNewFolder(Package, "%INSTALLPATH%");
            Directory.SetCurrentDirectory(Program.AppPath);
        }

        void CreateFileFolder(string Name)
        {
            TreeNode CurrentTN = null;
            foreach (string folder in Name.Split('\\'))
            {
                if (folder.Trim() == "")
                    continue;
                bool TNFound = false;
                if (CurrentTN == null)
                {
                    foreach (TreeNode tnf in treeFileFolders.Nodes)
                    {
                        if (tnf.Name.ToLower() == folder.ToLower())
                        {
                            CurrentTN = tnf;
                            TNFound = true;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (TreeNode tnf in CurrentTN.Nodes)
                    {
                        if (tnf.Name.ToLower() == folder.ToLower())
                        {
                            CurrentTN = tnf;
                            TNFound = true;
                            break;
                        }
                    }
                }
                if (TNFound == false)
                {
                    TreeNode ntn = new TreeNode(folder, 0, 0);
                    ntn.Name = folder;
                    if (CurrentTN != null)
                    {
                        CurrentTN.Nodes.Add(ntn);
                        CurrentTN = ntn;
                    }
                    else
                    {
                        treeFileFolders.Nodes.Add(ntn);
                        CurrentTN = ntn;
                    }
                }
            }
        }

        void LoadPackage()
        {
            lstFiles.Items.Clear();
            treeFileFolders.Nodes.Clear();

            txtPackageTitle.Text = Package.Title;
            txtPackageDescription.Text = Package.Description;
            txtPackageID.Text = Package.PackageID;
            txtScript.Text = Package.Script;
            txtOutputFilename.Text = Package.Outputfile;
            txtVersion.Text = Package.VersionID.ToString();
            chkNoReceipt.Checked = Package.NoReceipt;

            foreach (PKGFolder fldr in Package.Folders)
            {
                CreateFileFolder(fldr.FolderName);
            }

            Modified = false;
        }

        public MainDLG()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainDLG_Load(object sender, EventArgs e)
        {
            if (Settings.Default.WinH != 0 && Settings.Default.WinW != 0 && Settings.Default.WinX != 0 && Settings.Default.WinY != 0)
            {
                this.Top = Settings.Default.WinY;
                this.Left = Settings.Default.WinX;
                this.Width = Settings.Default.WinW;
                this.Height = Settings.Default.WinH;
            }
            this.WindowState = (FormWindowState)Settings.Default.WinState;

            Program.LoadImageList(imageList1);
            NewPackage();
            LoadPackage();
        }

        private void txtMULTI_TextChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void renameFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeFileFolders.SelectedNode == null)
                return;

            TreeNode TN = treeFileFolders.SelectedNode;
            string FP = TN == null ? "" : TN.FullPath;
            string PATH = TN == null ? "" : TN.Name;
            frmAskText frm = new frmAskText("Rename folder", "Rename folder " + PATH, PATH, false);
            if (frm.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            foreach (TreeNode tn in TN.Parent != null ? TN.Parent.Nodes : treeFileFolders.Nodes)
            {
                if (frm.ReturnedText.ToLower() == tn.Name.ToLower())
                {
                    if (MessageBox.Show(this, "The folder " + frm.ReturnedText + " already exists. Do you want to merge the 2 folders?", Program.AppTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        tn.Remove();
                        break;
                    }
                }
            }
            PackageDataUtilities.RenameFolder(Package, FP, frm.ReturnedText);
            Modified = true;
            treeFileFolders.SelectedNode.Name = frm.ReturnedText;
            treeFileFolders.SelectedNode.Text = frm.ReturnedText;
        }

        void NewFolder(TreeNode TN)
        {
            string FP = TN == null ? "" : TN.FullPath;
            string PATH = TN == null ? "" : TN.Name;
            frmAskText frm = new frmAskText("Create folder", "Create folder to " + (PATH == "" ? "\\" : PATH), "", false);
            if (frm.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            TreeNode ntn = new TreeNode(frm.ReturnedText, 0, 0);
            ntn.Name = frm.ReturnedText;
            if (TN != null)
            {
                TN.Nodes.Add(ntn);
            }
            else
            {
                treeFileFolders.Nodes.Add(ntn);
            }
            PackageDataUtilities.AddNewFolder(Package, ntn.FullPath);
            Modified = true;
        }

        private void currentFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFolder(treeFileFolders.SelectedNode);
        }

        private void rootFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFolder(null);
        }

        bool UpdateValues()
        {
            Package.HeaderID = "FoxPackageScriptV1";
            Package.Title = txtPackageTitle.Text;
            Package.Description = txtPackageDescription.Text;
            Package.PackageID = txtPackageID.Text;
            Package.Script = txtScript.Text;
            Package.Outputfile = txtOutputFilename.Text;
            Package.VersionID = 0;
            Package.NoReceipt = chkNoReceipt.Checked;
            if (Int64.TryParse(txtVersion.Text, out Package.VersionID) == false)
            {
                MessageBox.Show(this, "Invalid version ID", Program.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return (false);
            }
            return (true);
        }

        bool WriteFile()
        {
            if (CurrentFile == "")
                return (false);

            if (UpdateValues() == false)
                return (false);

            try
            {
                string data = JsonConvert.SerializeObject(Package, Formatting.Indented);
                File.WriteAllText(CurrentFile, data, Encoding.UTF8);
                Directory.SetCurrentDirectory(Path.GetDirectoryName(CurrentFile));
            }
            catch (Exception ee)
            {
                MessageBox.Show(this, "Cannot write the file \n" + CurrentFile + "\n" + ee.Message, Program.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return (false);
            }

            Modified = false;
            return (true);
        }

        bool Save()
        {
            if (CurrentFile == "")
                return (SaveAs());

            return (WriteFile());
        }

        bool SaveAs()
        {
            SaveFileDialog cmdlg = new SaveFileDialog();
            cmdlg.Title = "Save Package Script as";
            cmdlg.Filter = "Fox Package Script|*.foxps";
            cmdlg.DefaultExt = ".foxps";
            cmdlg.CheckPathExists = true;
            if (cmdlg.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return (false);

            CurrentFile = cmdlg.FileName;
            this.Text = Program.AppTitle + " (" + CurrentFile + ")";

            return (WriteFile());
        }

        bool AskSave()
        {
            if (Modified == false)
                return (true);
            DialogResult res = MessageBox.Show(this, "The file \n" + (CurrentFile == "" ? "(untitled)" : CurrentFile) + "\nhas been changed. Do you want to save the changes?", Program.AppTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
            if (res == System.Windows.Forms.DialogResult.No)
                return (true);
            if (res == System.Windows.Forms.DialogResult.Yes)
                return (Save());
            if (res == System.Windows.Forms.DialogResult.Cancel)
                return (false);

            return (true);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AskSave() == false)
                return;
            NewPackage();
            LoadPackage();
        }

        private void MainDLG_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AskSave() == false)
            {
                e.Cancel = true;
                return;
            }

            Settings.Default.WinState = (int)this.WindowState;
            this.WindowState = FormWindowState.Normal;
            Settings.Default.WinY = this.Top;
            Settings.Default.WinX = this.Left;
            Settings.Default.WinW = this.Width;
            Settings.Default.WinH = this.Height;
            Settings.Default.Save();
        }

        private void cmdNewGUID_Click(object sender, EventArgs e)
        {
            Package.PackageID = Guid.NewGuid().ToString();
            txtPackageID.Text = Package.PackageID;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void saveasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AskSave() == false)
                return;

            OpenFileDialog cmdlg = new OpenFileDialog();
            cmdlg.Title = "Open Package Script";
            cmdlg.Filter = "Fox Package Script|*.foxps";
            cmdlg.DefaultExt = ".foxps";
            cmdlg.CheckPathExists = true;
            cmdlg.CheckFileExists = true;
            if (cmdlg.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;

            string data = "";
            try
            {
                data = File.ReadAllText(cmdlg.FileName, Encoding.UTF8);
                Directory.SetCurrentDirectory(Path.GetDirectoryName(cmdlg.FileName));
            }
            catch (Exception ee)
            {
                MessageBox.Show(this, "Cannot read the file \n" + cmdlg.FileName + "\n" + ee.Message, Program.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            try
            {
                PKGRootData r = JsonConvert.DeserializeObject<PKGRootData>(data);
                if (r.HeaderID != "FoxPackageScriptV1")
                {
                    MessageBox.Show(this, "The file " + cmdlg.FileName + " is not a valid Package Script", Program.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                Package = r;
                CurrentFile = cmdlg.FileName;
                this.Text = Program.AppTitle + " (" + CurrentFile + ")";
                LoadPackage();
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                MessageBox.Show(this, "The file " + cmdlg.FileName + " cannot be parsed.", Program.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UpdateValues() == false)
                return;
            frmCompilePackage cmp = new frmCompilePackage(Package);
            cmp.ShowDialog(this);
            txtOutputFilename.Text = Package.Outputfile;
        }

        private void propertiesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeFileFolders.SelectedNode == null)
                return;

            TreeNode TN = treeFileFolders.SelectedNode;
            PKGFolder folder = PackageDataUtilities.Findfolder(Package, TN.FullPath);
            if (folder == null)
                return;
            frmPropertiesFolder frm = new frmPropertiesFolder(folder);
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                Modified = true;
            }
        }

        private void deleteFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeFileFolders.SelectedNode == null)
                return;

            TreeNode TN = treeFileFolders.SelectedNode;
            if (MessageBox.Show(this, "Do you want to delete the folder " + TN.FullPath + " including files?", Program.AppTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != System.Windows.Forms.DialogResult.Yes)
                return;

            string FullPath = TN.FullPath;
            if (TN.Parent == null)
                treeFileFolders.Nodes.Remove(TN);
            else
                TN.Parent.Nodes.Remove(TN);

            PackageDataUtilities.DeleteFolder(Package, FullPath);

            Modified = true;
        }

        void LoadFileList()
        {
            lstFiles.Items.Clear();

            if (treeFileFolders.SelectedNode == null)
                return;

            string fullfoldername = treeFileFolders.SelectedNode.FullPath;
            if (fullfoldername.EndsWith("\\") == false)
                fullfoldername += "\\";
            fullfoldername = fullfoldername.ToLower();

            foreach (PKGFile f in Package.Files)
            {
                string folder = f.FolderName.Trim().ToLower();
                if (folder.EndsWith("\\") == false)
                    folder += "\\";
                if (folder == fullfoldername)
                {
                    ListViewItem l = new ListViewItem(f.FileName);
                    l.SubItems.Add(f.SrcFile);
                    l.Tag = f;
                    lstFiles.Items.Add(l);
                }
            }
        }


        private void addFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeFileFolders.SelectedNode == null)
                return;

            TreeNode TN = treeFileFolders.SelectedNode;
            frmFileProperties fp = new frmFileProperties(TN.FullPath);
            if (fp.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            PackageDataUtilities.AddNewFile(Package, fp.file);
            CreateFileFolder(fp.file.FolderName);
            LoadFileList();
        }

        private void addmultipleFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeFileFolders.SelectedNode == null)
                return;

            OpenFileDialog cmdlg = new OpenFileDialog();
            cmdlg.Filter = "All files|*.*";
            cmdlg.Title = "Browse for source files";
            cmdlg.Multiselect = true;
            cmdlg.CheckFileExists = true;
            if (cmdlg.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            foreach (string filename in cmdlg.FileNames)
            {
                PKGFile file = new PKGFile();
                file.FileName = Path.GetFileName(filename);
                file.FolderName = treeFileFolders.SelectedNode.FullPath;
                if (file.FolderName.EndsWith("\\") == false)
                    file.FolderName += "\\";
                file.SrcFile = filename;
                file.ID = "";
                file.InstallThisFile = true;
                PackageDataUtilities.AddNewFile(Package, file);
            }
            LoadFileList();
        }

        private void deleteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItems.Count == 0)
                return;
            foreach (ListViewItem l in lstFiles.SelectedItems)
            {
                Package.Files.Remove((PKGFile)l.Tag);
            }
            LoadFileList();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItems.Count == 0)
                return;
            if (lstFiles.SelectedItems.Count > 1)
                return;
            PKGFile f = (PKGFile)lstFiles.SelectedItems[0].Tag;
            frmFileProperties frm = new frmFileProperties(f);
            if (frm.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            LoadFileList();
        }

        private void addFolderFromSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeFileFolders.SelectedNode == null)
                return;

            string folder = treeFileFolders.SelectedNode.FullPath;
            if (folder.EndsWith("\\") == false)
                folder += "\\";

            FolderBrowserDialog fldr = new FolderBrowserDialog();
            fldr.Description = "Browse for files to add to " + folder;
            fldr.ShowNewFolderButton = false;
            if (fldr.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;

            foreach (string filename in Directory.EnumerateFiles(fldr.SelectedPath, "*.*", SearchOption.AllDirectories))
            {
                PKGFile file = new PKGFile();
                file.FileName = Path.GetFileName(filename);
                string p = Path.GetDirectoryName(filename);
                p = p.Substring(fldr.SelectedPath.Length);
                if (p.StartsWith("\\") == true)
                    p = p.Substring(1);
                p = folder + p;
                if (p.EndsWith("\\") == false)
                    p += "\\";

                file.FolderName = p;
                if (file.FolderName.EndsWith("\\") == false)
                    file.FolderName += "\\";
                file.SrcFile = filename;
                file.ID = "";
                file.InstallThisFile = true;
                PackageDataUtilities.AddNewFile(Package, file);
                CreateFileFolder(file.FolderName);
            }

            LoadFileList();
        }

        private void treeFileFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LoadFileList();
        }

        private void cmdSelectOutputFilename_Click(object sender, EventArgs e)
        {
            SaveFileDialog cmdlg = new SaveFileDialog();
            cmdlg.Title = "Save Package as";
            cmdlg.Filter = "Fox Package|*.foxpkg";
            cmdlg.DefaultExt = ".foxpkg";
            cmdlg.CheckPathExists = true;
            if (cmdlg.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            txtOutputFilename.Text = cmdlg.FileName;
        }

        private void chkNoReceipt_CheckedChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void testCompilePackageScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Package.Script = txtScript.Text;
            string ErrorText = "";
            string DLLFilename;
            if (PackageCompiler.CompileScript(Package.Script, out ErrorText, "", out DLLFilename) == null)
            {
                MessageBox.Show(this, ErrorText, Program.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            MessageBox.Show(this, "No errors.", Program.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cmdPlus1_Click(object sender, EventArgs e)
        {
            Int64 ver;
            if (Int64.TryParse(txtVersion.Text, out ver) == false)
                return;
            ver++;
            txtVersion.Text = ver.ToString();
        }

        private void cmdDT_Click(object sender, EventArgs e)
        {
            txtVersion.Text = DateTime.Now.ToString("yyyyMMdd");
        }

        private void txtVersion_TextChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void cmdDT2_Click(object sender, EventArgs e)
        {
            txtVersion.Text = DateTime.Now.ToString("yyMMdd") + "01";
        }
    }
}
