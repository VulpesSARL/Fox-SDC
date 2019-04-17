using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoxSDC_Common;

namespace FoxSDC_MGMT
{
    public partial class ctlDevices : UserControl
    {
        string MachineID;
        public ctlDevices(string MachineID)
        {
            this.MachineID = MachineID;
            InitializeComponent();
        }

        private void ctlDevices_Load(object sender, EventArgs e)
        {
            List<PnPDevice> cfg = Program.net.GetDevicesConfig(MachineID);
            if (cfg == null)
                return;
            foreach (PnPDevice dev in cfg)
            {
                if (dev.ClassGuid.Trim() == "")
                    dev.ClassGuid = "Unknown devices";

                if (TV.Nodes.ContainsKey(dev.ClassGuid.ToLower()) == false)
                {
                    string classname = dev.ClassGuid;
                    if (Win10DevClasses.Classes.ContainsKey(dev.ClassGuid) == true)
                        classname = Win10DevClasses.Classes[dev.ClassGuid];
                    TV.Nodes.Add(dev.ClassGuid.ToLower(), classname);
                }
                TreeNode tn = TV.Nodes.Find(dev.ClassGuid.ToLower(), false)[0];
                TreeNode stn = tn.Nodes.Add(dev.Name.Trim() == "" ? "Unknown device" : dev.Name.Trim());
                stn.Tag = dev;
                if (dev.ConfigManagerErrorCode != 0)
                {
                    tn.Expand();
                    stn.Text = "[!] " + stn.Text;
                }
            }
            TV.Sort();
        }

        private void TV_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selected = TV.SelectedNode;
            if (selected.Tag == null)
            {
                PG.SelectedObject = null;
                return;
            }

            PPnPDevice pnp = new PPnPDevice();
            ClassCopy.CopyClassData(selected.Tag, pnp);
            switch (pnp.ConfigManagerErrorCode)
            {
                case 0: pnp.ConfigManagerErrorCodeText = "This device is working properly."; break;
                case 1: pnp.ConfigManagerErrorCodeText = "This device is not configured correctly."; break;
                case 2: pnp.ConfigManagerErrorCodeText = "Windows cannot load the driver for this device."; break;
                case 3: pnp.ConfigManagerErrorCodeText = "The driver for this device might be corrupted, or your system may be running low on memory or other resources."; break;
                case 4: pnp.ConfigManagerErrorCodeText = "This device is not working properly. One of its drivers or your registry might be corrupted."; break;
                case 5: pnp.ConfigManagerErrorCodeText = "The driver for this device needs a resource that Windows cannot manage."; break;
                case 6: pnp.ConfigManagerErrorCodeText = "The boot configuration for this device conflicts with other devices."; break;
                case 7: pnp.ConfigManagerErrorCodeText = "Cannot filter."; break;
                case 8: pnp.ConfigManagerErrorCodeText = "The driver loader for the device is missing."; break;
                case 9: pnp.ConfigManagerErrorCodeText = "This device is not working properly because the controlling firmware is reporting the resources for the device incorrectly."; break;
                case 10: pnp.ConfigManagerErrorCodeText = "This device cannot start."; break;
                case 11: pnp.ConfigManagerErrorCodeText = "This device failed."; break;
                case 12: pnp.ConfigManagerErrorCodeText = "This device cannot find enough free resources that it can use."; break;
                case 13: pnp.ConfigManagerErrorCodeText = "Windows cannot verify this device's resources."; break;
                case 14: pnp.ConfigManagerErrorCodeText = "This device cannot work properly until you restart your computer."; break;
                case 15: pnp.ConfigManagerErrorCodeText = "This device is not working properly because there is probably a re-enumeration problem."; break;
                case 16: pnp.ConfigManagerErrorCodeText = "Windows cannot identify all the resources this device uses."; break;
                case 17: pnp.ConfigManagerErrorCodeText = "This device is asking for an unknown resource type."; break;
                case 18: pnp.ConfigManagerErrorCodeText = "Reinstall the drivers for this device."; break;
                case 19: pnp.ConfigManagerErrorCodeText = "Your registry might be corrupted."; break;
                case 20: pnp.ConfigManagerErrorCodeText = "Failure using the VxD loader."; break;
                case 21: pnp.ConfigManagerErrorCodeText = "System failure: Try changing the driver for this device. If that does not work, see your hardware documentation. Windows is removing this device."; break;
                case 22: pnp.ConfigManagerErrorCodeText = "This device is disabled."; break;
                case 23: pnp.ConfigManagerErrorCodeText = "System failure: Try changing the driver for this device. If that doesn't work, see your hardware documentation."; break;
                case 24: pnp.ConfigManagerErrorCodeText = "This device is not present, is not working properly, or does not have all its drivers installed."; break;
                case 25: pnp.ConfigManagerErrorCodeText = "Windows is still setting up this device."; break;
                case 26: pnp.ConfigManagerErrorCodeText = "Windows is still setting up this device."; break;
                case 27: pnp.ConfigManagerErrorCodeText = "This device does not have valid log configuration."; break;
                case 28: pnp.ConfigManagerErrorCodeText = "The drivers for this device are not installed."; break;
                case 29: pnp.ConfigManagerErrorCodeText = "This device is disabled because the firmware of the device did not give it the required resources."; break;
                case 30: pnp.ConfigManagerErrorCodeText = "This device is using an Interrupt Request (IRQ) resource that another device is using."; break;
                case 31: pnp.ConfigManagerErrorCodeText = "This device is not working properly because Windows cannot load the drivers required for this device."; break;
                default: pnp.ConfigManagerErrorCodeText = "??? " + pnp.ConfigManagerErrorCode.ToString(); break;
            }
            PG.SelectedObject = pnp;
        }
    }
}
