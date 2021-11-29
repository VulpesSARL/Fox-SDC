using FoxSDC_Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FoxSDC_Package
{
    public class PackageScriptTemplate : PKGScript
    {
        PKGInstallState State = PKGInstallState.NotSet;
        const string RegPath = "Software\\Fox\\SDC Scripted Installs\\";

        PKGStatus CheckReq(PKGRunningPackageData Package)
        {
            using (RegistryKey r = Registry.LocalMachine.OpenSubKey(RegPath))
            {
                if (r == null)
                    return (PKGStatus.Success);
                string Current = Convert.ToString(r.GetValue(Package.PackageID.Replace("\\", ""), ""));
                Int64 CurrentV;
                if (Int64.TryParse(Current, out CurrentV) == false)
                    //install it!
                    return (PKGStatus.Success);

                if (CurrentV <= Package.VersionID)
                    //nö, not needed
                    return (PKGStatus.NotNeeded);
            }

            //install it!
            return (PKGStatus.Success);
        }

        public PKGStatus CheckInstallationStatus(PKGRunningPackageData Package, PKGInstallState state)
        {
            State = state;
            Package.SetInstallPath("%PROGRAMFILES%\\Fox\\My Package");
            Package.ErrorText = "";

            PKGStatus p = CheckReq(Package);
            if (p != PKGStatus.Success)
                Package.ErrorText = "Not needed";

            return (p);
        }

        public PKGStatus PreInstall(PKGRunningPackageData Package)
        {
            PKGStatus p = CheckReq(Package);
            if (p != PKGStatus.Success)
                Package.ErrorText = "Not needed";

            return (p);
        }

        public PKGStatus PostInstall(PKGRunningPackageData Package)
        {
            using (RegistryKey r = Registry.LocalMachine.CreateSubKey(RegPath))
            {
                if (r != null)
                {
                    r.SetValue(Package.PackageID.Replace("\\", ""), Package.VersionID);
                }
            }

            Package.ErrorText = "";
            return (PKGStatus.Success);
        }

        public PKGStatus Rollback(PKGRunningPackageData Package)
        {
            Package.ErrorText = "";
            return (PKGStatus.Success);
        }

        public List<string> GetDependencies(PKGRunningPackageData Package)
        {
            return (new List<string>());
        }

        public PKGStatus ApplyUserSettings(PKGRunningPackageData Package)
        {
            Package.ErrorText = "";
            return (PKGStatus.Success);
        }
    }
}
