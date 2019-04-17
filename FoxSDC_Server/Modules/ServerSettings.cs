using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Server
{
    class ServerSettingsDB
    {
        [VulpesRESTfulRet("CertificateList")]
        public NetStringList CertificateList;

        [VulpesRESTProtected]
        [VulpesRESTful(VulpesRESTfulVerb.GET, "api/mgmt/certificates/installedcerts", "CertificateList", "")]
        public RESTStatus GetInstalledCertificates(SQLLib sql, object dummy, NetworkConnectionInfo ni)
        {
            if (ni.HasAcl(ACLFlags.ChangeServerSettings) == false)
            {
                ni.Error = "Access denied";
                ni.ErrorID = ErrorFlags.AccessDenied;
                return (RESTStatus.Denied);
            }
            
            CertificateList = new NetStringList();
            CertificateList.Items = Certificates.GetCertificates(StoreLocation.LocalMachine);
            if (CertificateList.Items == null)
            {
                ni.Error = "Cannot get list";
                ni.ErrorID = ErrorFlags.SystemError;
                return (RESTStatus.ServerError);
            }
            return (RESTStatus.Success);
        }
    }
}
