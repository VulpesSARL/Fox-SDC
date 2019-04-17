using FoxSDC_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_MGMT
{
    static class GroupFolders
    {
        public static void CreateRootFolder(TreeView tv)
        {
            TreeNode tn = tv.Nodes.Add("grp:root", "Groups", 0, 1);
            TreeNode stn = tn.Nodes.Add("grp:expand", "(expand)");
            stn.Tag = "**PENDING**";
        }

        public static void UpdateTreeNode(TreeNode tn, bool IncludePolicies)
        {
            if (tn.Name.StartsWith("grp:") == false)
                return;
            tn.Nodes.Clear();
            TreeNode stn = tn.Nodes.Add("grp:expand", "(expand)");
            stn.Tag = "**PENDING**";

            TreeViewCancelEventArgs e = new TreeViewCancelEventArgs(tn, false, TreeViewAction.Expand);
            BeforeExpand(e, IncludePolicies);
        }

        static public Int64? GetSelectedGroupID(TreeNode tn, out bool Valid)
        {
            Valid = false;

            if (tn.Name.StartsWith("grp:") == false)
                return (null);

            if (tn.Name == "grp:root")
            {
                Valid = true;
                return (null);
            }
            else
            {
                string spl = tn.Name.Split(':')[1].Trim();
                Int64 G;
                if (Int64.TryParse(spl, out G) == false)
                    return (null);
                Valid = true;
                return (G);
            }
        }

        static public Int64? GetSelectedPolicyID(TreeNode tn, out bool Valid)
        {
            Valid = false;

            if (tn.Name.StartsWith("pol:") == false)
                return (null);

            string spl = tn.Name.Split(':')[1].Trim();
            Int64 G;
            if (Int64.TryParse(spl, out G) == false)
                return (null);
            Valid = true;
            return (G);
        }

        public static void BeforeExpand(TreeViewCancelEventArgs e, bool IncludePolicies)
        {
            if (e.Node.Name.StartsWith("grp:") == false)
                return;

            Int64? Group = null;
            Int64 G;
            if (e.Node.Name == "grp:root")
            {
                Group = null;
                G = 0;
            }
            else
            {
                string spl = e.Node.Name.Split(':')[1].Trim();
                if (Int64.TryParse(spl, out G) == false)
                    return;
                Group = G;
            }

            if (e.Action == TreeViewAction.Expand)
            {
                if (e.Node.Nodes.Count == 1)
                {
                    if (e.Node.Nodes[0].Tag is string)
                    {
                        if (Convert.ToString(e.Node.Nodes[0].Tag) != "**PENDING**")
                            return;
                        e.Node.Nodes.Clear();

                        if (IncludePolicies == true)
                        {
                            List<PolicyObject> pol = Program.net.ListPolicies(false, null, Group, false);
                            if (pol != null)
                            {
                                foreach (PolicyObject ge in pol)
                                {
                                    int IconIndex = PolicyList.GetIconIndex(ge.Type);
                                    e.Node.Nodes.Add("pol:" + ge.ID.ToString(), ge.Name + (ge.Enabled == true ? "" : " [disabled]"), IconIndex, IconIndex);
                                }
                            }
                        }

                        List<GroupElement> gel = Program.net.GetGroups(Group);
                        if (gel != null)
                        {
                            foreach (GroupElement ge in gel)
                            {
                                TreeNode gen = e.Node.Nodes.Add("grp:" + ge.ID.ToString(), ge.Name, 0, 1);
                                TreeNode stn = gen.Nodes.Add("grp:expand", "(expand)");
                                stn.Tag = "**PENDING**";
                            }
                        }
                    }
                }
            }
        }

        public static bool AfterSelect(TreeView tv, TreeViewEventArgs e, out Int64? GroupingID, out Int64? PolicyID)
        {
            GroupingID = null;
            PolicyID = null;

            if (AfterSelect(tv, e, out GroupingID) == true)
                return (true);

            if (e.Node.Name.StartsWith("pol:") == false)
                return (false);

            string spl = e.Node.Name.Split(':')[1].Trim();
            Int64 iidd;
            if (Int64.TryParse(spl, out iidd) == false)
                return (false);

            PolicyID = iidd;

            return (true);
        }

        public static bool AfterSelect(TreeView tv, TreeViewEventArgs e, out Int64? ID)
        {
            ID = null;

            if (e.Node.Name.StartsWith("grp:") == false)
                return (false);

            string spl = e.Node.Name.Split(':')[1].Trim();
            Int64 iidd;
            if (Int64.TryParse(spl, out iidd) == false)
                return (false);

            ID = iidd;

            return (true);
        }

        public static string GetFullPath(TreeView tv)
        {
            string s = "";
            TreeNode p = tv.SelectedNode;
            while (p != null)
            {
                s = p.Text + "\\" + s;
                p = p.Parent;
            }

            return (s);
        }
    }
}
