using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public class NullTest
    {

        public static bool Test(object data, params string[] SkipNames)
        {
            FieldInfo[] fields = data.GetType().GetFields();
            try
            {
                foreach (FieldInfo f in fields)
                {
                    string column = f.Name;
                    bool Skip = false;
                    foreach (string sk in SkipNames)
                    {
                        if (sk == column)
                        {
                            Skip = true;
                            break;
                        }
                    }
                    if (Skip == true)
                        continue;

                    if (f.GetValue(data) != null)
                        continue;
                    else
                        return (false);
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (false);
            }
            return (true);
        }

        public static bool TestBlankString(object data, params string[] SkipNames)
        {
            FieldInfo[] fields = data.GetType().GetFields();
            try
            {
                foreach (FieldInfo f in fields)
                {
                    string column = f.Name;
                    bool Skip = false;
                    foreach (string sk in SkipNames)
                    {
                        if (sk == column)
                        {
                            Skip = true;
                            break;
                        }
                    }
                    if (Skip == true)
                        continue;

                    if (f.GetValue(data).ToString().Trim() == "")
                        return (false);
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
