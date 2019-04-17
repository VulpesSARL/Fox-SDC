using FoxSDC_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class PolicyCertificates
    {
        public static List<byte[]> GetPolicyCertificates(SQLLib sql)
        {
            List<byte[]> lst = new List<byte[]>();

            SqlDataReader dr = sql.ExecSQLReader("select DataBlob from Policies where Type=3 and Enabled=1");
            while(dr.Read())
            {
                string data = Convert.ToString(dr["DataBlob"]);
                PolicyPackageCertificates cer=null;
                try
                {
                    cer = JsonConvert.DeserializeObject<PolicyPackageCertificates>(data);
                }
                catch
                {

                }
                if (cer == null)
                    continue;
                if (cer.UUCerFile == null || cer.UUCerFile == "")
                    continue;
                try
                {
                    lst.Add(Convert.FromBase64String(cer.UUCerFile));
                }
                catch
                {

                }
            }
            dr.Close();

            return (lst);
        }
    }
}
