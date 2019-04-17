using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoxSDC_Package
{
    public class PackageScriptTemplate : PKGScript
    {
        PKGInstallState State = PKGInstallState.NotSet;

        public PKGStatus CheckInstallationStatus(PKGRunningPackageData Package, PKGInstallState state)
        {
            State = state;
            Package.SetInstallPath("%PROGRAMFILES%\\Fox\\My Package");
            Package.ErrorText = "";
            return (PKGStatus.Success);
        }

        public PKGStatus PreInstall(PKGRunningPackageData Package)
        {
            Package.ErrorText = "";
            return (PKGStatus.Success);
        }

        public PKGStatus PostInstall(PKGRunningPackageData Package)
        {
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
