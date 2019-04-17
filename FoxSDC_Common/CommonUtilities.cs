using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public class CommonUtilities
    {
        #region Win32

        [Flags]
        enum MoveFileFlags
        {
            None = 0,
            ReplaceExisting = 1,
            CopyAllowed = 2,
            DelayUntilReboot = 4,
            WriteThrough = 8,
            CreateHardlink = 16,
            FailIfNotTrackable = 32,
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool MoveFileEx(string lpExistingFileName, string lpNewFileName, MoveFileFlags dwFlags);

        #endregion

        public readonly static Dictionary<int, string> SupportedTypes = new Dictionary<int, string>()
        {
            {0x4,"BACKOFFICE"                            },
            {0x400,"WEB SERVER"                          },
            {0x4000,"COMPUTE_SERVER"                     },
            {0x80,"DATACENTER"                           },
            {0x2,"ENTERPRISE"                            },
            {0x40,"EMBEDDED"                             },
            {0x200,"PERSONAL"                            },
            {0x100,"SINGLE_USER_TERMINAL_SERVER"         },
            {0x1,"SMALLBUSINESS"                         },
            {0x20,"SMALLBUSINESS_RESTRICTED"             },
            {0x2000,"STORAGE_SERVER"                     },
            {0x10,"TERMINAL_SERVER"                      },
            {0x8000,"HOME_SERVER"                        }
        };

        /// <summary>
        /// Decodes the wSuiteMask from OSVERSIONINFOEX
        /// </summary>
        /// <param name="OSVerType">wSuiteMask from OSVERSIONINFOEX</param>
        /// <returns>Text of the bits</returns>
        public static string DecodeOSVerType(int OSVerType)
        {
            string ret = "";

            foreach (KeyValuePair<int, string> kvp in SupportedTypes)
            {
                if ((OSVerType & kvp.Key) == kvp.Key)
                    ret += kvp.Value + " ";
            }

            return (ret.Trim());
        }

        public static bool SpecialDeleteFile(string Filename)
        {
            if (File.Exists(Filename) == false)
                return (false);

            try
            {
                File.Delete(Filename);
            }
            catch
            {
                MoveFileEx(Filename, null, MoveFileFlags.DelayUntilReboot);
            }

            return (true);
        }

        public static bool PendingMove(string Source, string Dest)
        {
            MoveFileEx(Source, Dest, MoveFileFlags.DelayUntilReboot);
            return (true);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern void SetLastError(uint dwErrorCode);
        public static void ResetLastError()
        {
            SetLastError(0);
        }

        public static string NiceSize(Int64 SZ)
        {
            string unit = "byte" + (SZ == 1 ? "" : "s");
            double un = SZ;
            if (un > 1024)
            {
                un /= 1024f;
                unit = "KiB";
            }
            if (un > 1024)
            {
                un /= 1024f;
                unit = "MiB";
            }
            if (un > 1024)
            {
                un /= 1024f;
                unit = "GiB";
            }
            if (un > 1024)
            {
                un /= 1024f;
                unit = "TiB";
            }
            return (un.ToString("0.##") + " " + unit);
        }

        public static string GetCommonBeginning(List<string> lst)
        {
            if (lst == null)
                return ("");
            if (lst.Count == 0)
                return ("");

            string Test = lst[0];
            bool TestFailed = false;
            do
            {
                TestFailed = false;
                foreach (string l in lst)
                {
                    if (l.StartsWith(Test) == false)
                    {
                        TestFailed = true;
                        break;
                    }
                }
                if (TestFailed == true)
                {
                    if (Test.Length == 1)
                        return ("");
                    Test = Test.Substring(0, Test.Length - 1);
                }
            } while (TestFailed == true);

            return (Test);
        }
        public static Int64 DTtoINT(DateTime dt)
        {
            string s = (dt.Year - 2000).ToString("0000");
            s += dt.Month.ToString("00");
            s += dt.Day.ToString("00");
            s += dt.Hour.ToString("00");
            s += dt.Minute.ToString("00");
            s += dt.Second.ToString("00");
            return (Convert.ToInt64(s));
        }

        public static SecureString ToSecureString(string data)
        {
            SecureString Pin = new SecureString();
            foreach (char c in data)
                Pin.AppendChar(c);
            return (Pin);
        }

        public static string NewGUID
        {
            get
            {
                Guid g = Guid.NewGuid();
                return (g.ToString());
            }
        }

        public static void CalcEventLogID(EventLogReport ev)
        {
            ev.LogID = MD5Utilities.CalcMD5(ev.Source + ev.Category + ev.EventLog + ev.InstanceID.ToString() + ev.TimeGenerated.ToString("yyyyMMddHHmmss") + ev.TimeWritten.ToString("yyyyMMddHHmmss") + ev.Message);
        }

        public static bool CompareClasses(object from, object to, params string[] excludeelements)
        {
            List<string> excl = new List<string>();
            if (excludeelements != null)
            {
                foreach (string e in excludeelements)
                    excl.Add(e);
            }

            FieldInfo[] fromfields = from.GetType().GetFields();
            try
            {
                foreach (FieldInfo ff in fromfields)
                {
                    string fromcolumn = ff.Name;
                    if (excl.Contains(fromcolumn) == true)
                        continue;
                    if (ff.FieldType.MemberType == MemberTypes.TypeInfo)
                    {
                        PropertyInfo tomembers = to.GetType().GetProperty(fromcolumn);
                        if (tomembers == null)
                        {
                            FieldInfo tomembersf = to.GetType().GetField(fromcolumn);
                            if (tomembersf == null)
                                continue;

                            if (tomembersf.GetValue(to) != null || ff.GetValue(from) != null)
                            {
                                if (tomembersf.GetValue(to) == null)
                                    return (false);
                                if (ff.GetValue(from) == null)
                                    return (false);
                                if (!(tomembersf.GetValue(to).Equals(ff.GetValue(from))))
                                    return (false);
                            }
                        }
                        else
                        {
                            if (tomembers.GetValue(to) != null || ff.GetValue(from) != null)
                            {
                                if (tomembers.GetValue(to) == null)
                                    return (false);
                                if (ff.GetValue(from) == null)
                                    return (false);
                                if (!(tomembers.GetValue(to).Equals(ff.GetValue(from))))
                                    return (false);
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (false);
            }
            return (true);
        }

    }
}
