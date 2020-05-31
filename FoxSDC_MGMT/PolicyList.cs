using FoxSDC_Common;
using FoxSDC_MGMT.Policies;
using FoxSDC_MGMT.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_MGMT
{
    class PolicyListElement
    {
        public int TypeID;
        public string Name;
        public string Description;
        public Type UserControl;
        public int IconIndex;

        public PolicyListElement()
        {

        }
        public PolicyListElement(int typeid, string name, string desc, Type usrcontrol, int iconindex)
        {
            TypeID = typeid;
            Name = name;
            Description = desc;
            UserControl = usrcontrol;
            IconIndex = iconindex;
        }

        public override string ToString()
        {
            return (Name);
        }
    }

    public interface PolicyElementInterface
    {
        bool SetData(PolicyObject obj);
        string GetData();
    }


    class PolicyList
    {
        public static List<PolicyListElement> Elements = new List<PolicyListElement>()
        {
#if DEBUG
            new PolicyListElement(PolicyIDs.Test, "Test", "Simple plain test element", typeof(ctlTesting), 5),
#endif
            new PolicyListElement(PolicyIDs.SignCertificate, "Server->Client Certificate", "Installs a certificate to verify server->client signing", typeof(ctlCertificates), 6),
            new PolicyListElement(PolicyIDs.PackageCertificate, "Package Certificate", "Installs a certificate to verify the installation/update packages that the sends to the client", typeof(ctlPackageCertificates), 6),
            new PolicyListElement(PolicyIDs.LinkedPolicy, "Link to another policy", "Links another policy to this place, so one policy set can be placed to multiple groups, where no inheritance can happen", typeof(ctlLinkPolicy), 8),
            new PolicyListElement(PolicyIDs.PackagePolicy, "Install packages", "Install packages or allow the use (non-admin) to install optional packages", typeof(ctlInstallPackages), 9),
            new PolicyListElement(PolicyIDs.WSUS, "WSUS Configuration", "Configure the Windows Update Services to use a different server than directly Microsoft", typeof(ctlWSUSSettings), 10),
            new PolicyListElement(PolicyIDs.InternationalSettings, "International Configuration", "Configures Windows International Settings (a.k.a. Regional and Language settings)", typeof(ctlIntl), 12),
            new PolicyListElement(PolicyIDs.ReportingPolicy, "Reporting Configuration", "Adds a reporting configuration, to send alerts / reporting to the administrator or client", typeof(ctlReporting), 13),
            new PolicyListElement(PolicyIDs.ClientSettings, "Fox SDC Agent Settings", "Changes settings affecting the Fox SDC Agent", typeof(ctlClientSettings), 15),
            new PolicyListElement(PolicyIDs.PortMapping, "Port Mapping", "Adds a TCP Listener to the agent where programs are able to directly connect to the remote-network, offering other services apart from SDC (like WSUS, SMTP Ports, ...)", typeof(ctlPortMapping), 24)
        };

        public static int GetIconIndex(int TypeID)
        {
            foreach (PolicyListElement ele in Elements)
            {
                if (ele.TypeID == TypeID)
                    return (ele.IconIndex);
            }
            return (5);//Nix
        }

        public static PolicyElementInterface GetInstance(int TypeID)
        {
            foreach (PolicyListElement ele in Elements)
            {
                if (ele.TypeID == TypeID)
                {
                    PolicyElementInterface i = (PolicyElementInterface)Activator.CreateInstance(ele.UserControl);
                    return (i);
                }
            }
            return (null);
        }
    }
}
