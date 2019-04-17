using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoxSDC_MGMT
{
    internal class ListViewItemComparer : IComparer
    {
        private int col;
        public ListViewItemComparer()
        {
            col = 0;
        }
        public ListViewItemComparer(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            return (String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text));
        }
    }

    internal class ListViewItemComparerNumeric : IComparer
    {
        private int col;
        public ListViewItemComparerNumeric()
        {
            col = 0;
        }
        public ListViewItemComparerNumeric(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            string X = ((ListViewItem)x).SubItems[col].Text;
            string Y = ((ListViewItem)y).SubItems[col].Text;
            try
            {
                return (Convert.ToInt64(X).CompareTo(Convert.ToInt64(Y)));
            }
            catch
            {
                return (String.Compare(X, Y));
            }
        }
    }

    internal class ListViewItemComparerDecimal : IComparer
    {
        private int col;
        public ListViewItemComparerDecimal()
        {
            col = 0;
        }
        public ListViewItemComparerDecimal(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            string X = ((ListViewItem)x).SubItems[col].Text;
            string Y = ((ListViewItem)y).SubItems[col].Text;
            try
            {
                return (Convert.ToDecimal(X).CompareTo(Convert.ToDecimal(Y)));
            }
            catch
            {
                return (String.Compare(X, Y));
            }
        }
    }

    internal class ListViewItemComparerDateTimeTAG : IComparer
    {
        private int col;
        public ListViewItemComparerDateTimeTAG()
        {
            col = 0;
        }
        public ListViewItemComparerDateTimeTAG(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            try
            {
                DateTime X = (DateTime)((ListViewItem)x).SubItems[col].Tag;
                DateTime Y = (DateTime)((ListViewItem)y).SubItems[col].Tag;
                return (X.CompareTo(Y));
            }
            catch
            {
                return (0);
            }
        }
    }
}
