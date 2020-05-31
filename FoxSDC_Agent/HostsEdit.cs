using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Agent
{
    class HostsEdit
    {
        public static void AppendIntoHOSTSFile(string Entry, string IPAddress)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Entry) == true || string.IsNullOrWhiteSpace(IPAddress) == true)
                    return;

                string HostsLine = IPAddress + " " + Entry + "      # from Fox SDC Agent";
                string HostsFile = Environment.ExpandEnvironmentVariables("%SYSTEMROOT%\\System32\\Drivers\\Etc\\Hosts");
                if (File.Exists(HostsFile) == false)
                {
                    File.AppendAllText(HostsFile, "\r\n" + HostsLine + "\r\n", Encoding.ASCII);
                    return;
                }

                List<string> Data = new List<string>();
                bool AppendLine = true;

                using (TextReader txtr = new StreamReader(File.Open(HostsFile, FileMode.Open, FileAccess.Read, FileShare.Read)))
                {
                    string Line = "";
                    while ((Line = txtr.ReadLine()) != null)
                    {
                        Data.Add(Line);

                        if (Line.Contains("#") == true)
                            Line = Line.Substring(0, Line.IndexOf('#')).Trim();
                        if (Line.Trim() == "")
                            continue;
                        string[] splitty = Line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (splitty.Length < 2)
                            continue;
                        if (splitty[0].ToLower().Trim() == IPAddress.ToLower().Trim() && //exact match - out, no changes
                            splitty[1].ToLower().Trim() == Entry.ToLower().Trim())
                            return;
                        if(splitty[1].ToLower().Trim() == Entry.ToLower().Trim()) //Name match, but not it's IP Address
                        {
                            Data.RemoveAt(Data.Count - 1);
                            Data.Add(HostsLine);
                            AppendLine = false;
                        }
                    }
                }

                if (AppendLine == false)
                {
                    File.WriteAllLines(HostsFile, Data.ToArray(), Encoding.ASCII);
                }
                else
                {
                    File.AppendAllText(HostsFile, "\r\n" + HostsLine + "\r\n", Encoding.ASCII);
                }
            }
            catch (Exception ee)
            {
                FoxEventLog.WriteEventLog("Cannot edit HOSTS file: " + ee.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }
        }
    }
}
