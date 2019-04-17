using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
#if EnableIIFCode

    public class If
    {
        static bool CleanCode(string code)
        {
            Regex regex = new Regex(@"(?<=\"").*?(?=\"")");
            foreach (Match match in regex.Matches(code))
            {
                code = code.Replace("\"" + match.Value + "\"", "");
            }
            if (code.Contains("//") == true || code.Contains("/*") == true ||
                code.Contains("*/") == true || code.Contains("\r") == true ||
                code.Contains("\n") == true)
                return (false);
            return (true);
        }

        public static void InterpretIf(string Condition, Dictionary<string, object> Variables, out bool ConditionMet, out bool Error, out string ErrorText)
        {
            Error = false;
            ErrorText = "";
            ConditionMet = false;

            if (CleanCode(Condition)==false)
            {
                Error = true;
                ErrorText = "Invalid code";
                ConditionMet = false;
                Debug.WriteLine("Invalid code");
                return;
            }

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters cp = new CompilerParameters();
            cp.GenerateInMemory = true;
            cp.IncludeDebugInformation = false;
            cp.TreatWarningsAsErrors = false;

            string source = "";
            source += "using System;\r\n";
            source += "\r\n";
            source += "public class iftest {\r\n";
            source += "public bool Test(){\r\n";
            foreach (KeyValuePair<string, object> kvp in Variables)
            {
                if (kvp.Value.GetType() == typeof(string))
                {
                    source += "string " + kvp.Key + "=@\"" + ((string)kvp.Value).Replace("\"", "\"\"") + "\";\r\n";
                }
                if (kvp.Value.GetType() == typeof(int))
                {
                    source += "int " + kvp.Key + "=" + ((int)kvp.Value).ToString() + ";\r\n";
                }
                if (kvp.Value.GetType() == typeof(Int64))
                {
                    source += "Int64 " + kvp.Key + "=" + ((Int64)kvp.Value).ToString() + ";\r\n";
                }
                if (kvp.Value.GetType() == typeof(DateTime))
                {
                    DateTime DT = (DateTime)kvp.Value;
                    source += "DateTime " + kvp.Key + "= new DateTime(" +
                        DT.Year.ToString() + "," +
                        DT.Month.ToString() + "," +
                        DT.Day.ToString() + "," +
                        DT.Hour.ToString() + "," +
                        DT.Minute.ToString() + "," +
                        DT.Second.ToString() + "," +
                        DT.Millisecond.ToString() + ");\r\n";
                }
            }
            source += "if (" + Condition + ")\r\nreturn(true);\r\nelse\r\nreturn(false);\r\n}}\r\n";

            Debug.WriteLine(source);

            CompilerResults cr = provider.CompileAssemblyFromSource(cp, source);
            if (cr.Errors.Count > 0)
            {
                foreach (CompilerError ce in cr.Errors)
                {
                    Debug.WriteLine(ce.ToString());
                    ErrorText += ce.ToString() + "\r\n";
                }
                ConditionMet = false;
                Error = true;
                return;
            }

            try
            {
                object o = cr.CompiledAssembly.CreateInstance("iftest");
                Type type = o.GetType();
                bool ret = (bool)type.InvokeMember("Test", BindingFlags.InvokeMethod | BindingFlags.Default, null, o, null);
                ConditionMet = ret;
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                ErrorText = "SEH Error";
                ConditionMet = false;
                Error = true;
                return;
            }
        }
    }

#endif

}
