using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent.Push
{
    class Filesystem
    {
        public static NetStringList ListFiles(string ReqString)
        {
            NetStringList lst = new NetStringList();
            lst.Items = new List<string>();
            PushDirListReq req;
            try
            {
                req = JsonConvert.DeserializeObject<PushDirListReq>(ReqString);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (lst);
            }

            if (req.ShowFiles == false && req.ShowFolders == false)
            {
                switch (req.Folder)
                {
                    case "drives":
                        foreach (DriveInfo drive in DriveInfo.GetDrives())
                        {
                            if (drive.IsReady == true)
                                lst.Items.Add(drive.Name + " - " + drive.VolumeLabel + " (" + drive.DriveType + ")");
                            else
                                lst.Items.Add(drive.Name + " - ?? (" + drive.DriveType + ")");
                        }
                        break;
                    default:
                        return (lst);
                }
            }
            else if (req.ShowFiles == true && req.ShowFolders == true)
            {
                return (lst);
            }
            else if (req.ShowFiles == true)
            {
                foreach (string file in Directory.EnumerateFiles(req.Folder, req.Filter, SearchOption.TopDirectoryOnly))
                {
                    lst.Items.Add(Path.GetFileName(file));
                }
            }
            else if (req.ShowFolders == true)
            {
                string foodir = req.Folder;
                if (foodir.EndsWith("\\") == false)
                    foodir += "\\";
                foreach (string dir in Directory.EnumerateDirectories(req.Folder, req.Filter, SearchOption.TopDirectoryOnly))
                {
                    lst.Items.Add(dir.Substring(foodir.Length, dir.Length - foodir.Length));
                }
            }
            return (lst);
        }

        public static int CheckFile (string Filename)
        {
            try
            {
                if (File.Exists(Filename) == true)
                    return ((int)PushFileState.File);
                if (Directory.Exists(Filename) == true)
                    return ((int)PushFileState.Folder);
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return ((int)PushFileState.RemoteError);
            }
            return ((int)PushFileState.NotExistent);
        }
    }
}
