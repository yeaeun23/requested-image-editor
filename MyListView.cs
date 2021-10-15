using System.Windows.Forms;
using System.Drawing;

namespace ImageWork
{
    public class MyListView : ListView
    {
        protected override void WndProc(ref Message msg)
        {
            // Ignore mouse messages not in the client area 
            if (msg.Msg >= 0x201 && msg.Msg <= 0x209)
            {
                Point point = new Point(msg.LParam.ToInt32() & 0xffff, msg.LParam.ToInt32() >> 16);
                ListViewHitTestInfo info = HitTest(point);

                switch (info.Location)
                {
                    case ListViewHitTestLocations.AboveClientArea:
                    case ListViewHitTestLocations.BelowClientArea:
                    case ListViewHitTestLocations.LeftOfClientArea:
                    case ListViewHitTestLocations.RightOfClientArea:
                    case ListViewHitTestLocations.None:
                        return;
                }
            }

            base.WndProc(ref msg);
        }
    }
}
