using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public class ClassCopy
    {
        public static byte[] CopyClassDataGetBytes(object something)
        {
            int sz = Marshal.SizeOf(something);
            IntPtr ptr = Marshal.AllocHGlobal(sz);
            Marshal.StructureToPtr(something, ptr, false);
            byte[] b = new byte[sz];
            Marshal.Copy(ptr, b, 0, sz);
            Marshal.FreeHGlobal(ptr);
            return (b);
        }

        public static T CopyClassDatafromBinary<T>(byte[] data)
        {
            if (data == null)
                return (default(T));
            if (data.Length == 0)
                return (default(T));

            IntPtr ptr = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, ptr, data.Length);
            T inst = (T)Activator.CreateInstance(typeof(T));
            Marshal.PtrToStructure(ptr, inst);
            Marshal.FreeHGlobal(ptr);
            return (inst);
        }

        public static bool CopyClassData(object from, object to)
        {
            FieldInfo[] fromfields = from.GetType().GetFields();
            try
            {
                foreach (FieldInfo ff in fromfields)
                {
                    string fromcolumn = ff.Name;
                    if (ff.FieldType.MemberType == MemberTypes.TypeInfo)
                    {
                        PropertyInfo tomembers = to.GetType().GetProperty(fromcolumn);
                        if (tomembers == null)
                        {
                            FieldInfo tomembersf = to.GetType().GetField(fromcolumn);
                            if (tomembersf == null)
                                continue;
                            if (ff.GetValue(from) == null)
                                continue;
                            tomembersf.SetValue(to, Convert.ChangeType(ff.GetValue(from), ff.FieldType));
                        }
                        else
                        {
                            if (tomembers.CanWrite == false)
                                continue;
                            if (ff.GetValue(from) == null)
                                continue;
                            tomembers.SetValue(to, Convert.ChangeType(ff.GetValue(from), ff.FieldType));
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
