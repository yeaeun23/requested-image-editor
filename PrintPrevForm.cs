using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ImageWork
{
    public partial class PrintPrevForm : Form
    {
        string id = "";
        string url = "";
        string rate = "75%";

        public PrintPrevForm(string id)
        {
            InitializeComponent();

            this.id = id;
            url = Form1.printUrl + id;
        }

        private void PrintPrevForm_Load(object sender, EventArgs e)
        {
            Browse();                                 
        }

        // 웹브라우저 로딩
        private void Browse()
        {
            Cursor = Cursors.WaitCursor;
            toolStripStatusLabel.Text = DateTime.Now.ToString("(HH:mm:ss) ") + "로딩중입니다.";

            webBrowser.Navigate(url);
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser.Document.Body.Style = "zoom: " + rate + ";";

            Cursor = Cursors.Default;
            toolStripStatusLabel.Text = DateTime.Now.ToString("(HH:mm:ss) ") + "새로고침 완료";
        }

        // 브라우저 열기 버튼 클릭
        private void browser_BTN_Click(object sender, EventArgs e)
        {
            Process.Start(url);
        }

        // 새로고침 버튼 클릭
        private void refresh_BTN_Click(object sender, EventArgs e)
        {
            Browse();
        }

        // F5 키 입력
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool bHandled = false;

            switch (keyData)
            {
                case Keys.F5:
                    Browse();
                    bHandled = true;
                    break;
            }

            return bHandled;
        }

        // 인쇄하기 버튼 클릭
        private void print_BTN_Click(object sender, EventArgs e)
        {
            DialogResult confirmResult = MessageBox.Show(this, "인쇄하시겠습니까?", "전표 인쇄", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (confirmResult == DialogResult.Yes)
            {
                webBrowser.Document.Body.Style = "zoom: 100%;";
                webBrowser.Print();

                webBrowser.Document.Body.Style = "zoom: " + rate + ";";
                toolStripStatusLabel.Text = DateTime.Now.ToString("(HH:mm:ss) ") + "인쇄 요청";
            }
        }

        // 닫기 버튼 클릭
        private void close_BTN_Click(object sender, EventArgs e)
        {
            Close();
        }

        // 25%
        private void zoom_1_Click(object sender, EventArgs e)
        {
            SetZoom(sender);
        }

        // 50%
        private void zoom_2_Click(object sender, EventArgs e)
        {
            SetZoom(sender);
        }

        // 75%
        private void zoom_3_Click(object sender, EventArgs e)
        {
            SetZoom(sender);
        }

        // 100%
        private void zoom_4_Click(object sender, EventArgs e)
        {            
            SetZoom(sender);
        }

        private void SetZoom(object sender)
        {
            rate = sender.ToString();

            ToolStripItemCollection items = zoom_BTN.DropDownItems;

            for (int i = 0; i < items.Count; i++)
            {
                if (((ToolStripMenuItem)items[i]).Text == rate)
                    ((ToolStripMenuItem)items[i]).Checked = true;
                else
                    ((ToolStripMenuItem)items[i]).Checked = false;
            }

            zoom_BTN.Text = rate;
            webBrowser.Document.Body.Style = "zoom: " + rate + ";";            
        }        
    }
}
