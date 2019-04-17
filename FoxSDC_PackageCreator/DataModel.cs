using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_PackageCreator
{
    class PackageDataUtilities
    {
        public static PKGFolder Findfolder (PKGRootData Package, string Folder)
        {
            foreach(PKGFolder fldr in Package.Folders)
            {
                if (fldr.FolderName.ToLower() == Folder.ToLower())
                    return (fldr);
            }
            return (null);
        }

        public static void DeleteFolder(PKGRootData Package, string FullFoldername)
        {
            Debug.WriteLine("DELTREE: " + FullFoldername);
            if (FullFoldername.EndsWith("\\") == false)
                FullFoldername += "\\";

            List<PKGFolder> RM = new List<PKGFolder>();
            List<PKGFile> RM2 = new List<PKGFile>();

            foreach (PKGFolder f in Package.Folders)
            {
                if (f.FolderName.ToLower().StartsWith(FullFoldername.ToLower()) == true)
                {
                    RM.Add(f);
                }
            }

            foreach(PKGFile f in Package.Files)
            {
                if(f.FolderName.ToLower().StartsWith(FullFoldername.ToLower())==true)
                {
                    RM2.Add(f);
                }
            }
            
            foreach(PKGFolder f in RM)
            {
                Package.Folders.Remove(f);
            }

            foreach(PKGFile f in RM2)
            {
                Package.Files.Remove(f);
            }
        }

        public static void RenameFolder(PKGRootData Package, string FullFoldername, string NewName)
        {
            Debug.WriteLine("REN: " + FullFoldername + " => " + NewName);
            if (FullFoldername.EndsWith("\\") == true)
                FullFoldername = FullFoldername.Substring(0, FullFoldername.Length - 1);
            string FullNewName = "";
            if (FullFoldername.LastIndexOf("\\") == -1)
            {
                FullNewName = NewName;
            }
            else
            {
                FullNewName = FullFoldername.Substring(0, FullFoldername.LastIndexOf("\\")) + "\\" + NewName;
            }

            Debug.WriteLine("RENF: " + FullFoldername + " => " + FullNewName);

            foreach (PKGFolder f in Package.Folders)
            {
                if (f.FolderName.StartsWith(FullFoldername) == true)
                {
                    f.FolderName = FullNewName + f.FolderName.Substring(FullFoldername.Length, f.FolderName.Length - FullFoldername.Length);
                }
            }

            foreach (PKGFile f in Package.Files)
            {
                if (f.FolderName.StartsWith(FullFoldername) == true)
                {
                    f.FolderName = FullNewName + f.FolderName.Substring(FullFoldername.Length, f.FolderName.Length - FullFoldername.Length);
                }
            }
        }

        public static void AddNewFolder(PKGRootData Package, string FullFoldername)
        {
            if (FullFoldername.EndsWith("\\") == false)
                FullFoldername += "\\";

            foreach (PKGFolder f in Package.Folders)
            {
                if (f.FolderName.Trim().ToLower() == FullFoldername.Trim().ToLower())
                    return;
            }

            Debug.WriteLine("MKDIR: " + FullFoldername);
            PKGFolder nf = new PKGFolder();
            nf.FolderName = FullFoldername.Trim();
            Package.Folders.Add(nf);
        }

        public static void AddNewFile(PKGRootData Package, PKGFile file)
        {
            if (file.FileName.Trim() == "")
                return;

            if (file.SrcFile.Trim() == "")
                return;
            
            if (file.FolderName.EndsWith("\\") == false)
                file.FolderName += "\\";
            
            AddNewFolder(Package, file.FolderName);
            Package.Files.Add(file);
        }
    }  
}
