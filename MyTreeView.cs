using System;
using System.Windows.Forms;

namespace ImageWork
{
    public class MyTreeView : TreeView
    {
        public MyTreeView()
        {
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg != 515)
            {
                base.WndProc(ref m);
            }
            else
            {
                m.Result = IntPtr.Zero;
            }
        }
    }
}