using System;
using System.Diagnostics;
using System.Drawing;
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

            // 테마
            if (Form1.IDX_THEME == Form1.IDX_THEME_DARK2)
                SetTheme(Form1.themeDark2);
            else if (Form1.IDX_THEME == Form1.IDX_THEME_LIGHT1)
                SetTheme(Form1.themeLight1);
            else if (Form1.IDX_THEME == Form1.IDX_THEME_LIGHT2)
                SetTheme(Form1.themeLight2);
            else if (Form1.IDX_THEME == Form1.IDX_THEME_BLUE)
                SetTheme(Form1.themeBlue);
        }

        private void PrintPrevForm_Load(object sender, EventArgs e)
        {
            Browse();
        }

        private void SetTheme(Color[] color)
        {
            BackColor = color[0];
            toolStripStatusLabel.BackColor = color[0];
            zoom_BTN.BackColor = color[0];

            browser_BTN.FlatAppearance.BorderColor = color[2];
            browser_BTN.FlatAppearance.MouseDownBackColor = color[2];
            browser_BTN.FlatAppearance.MouseOverBackColor = color[2];
            refresh_BTN.FlatAppearance.BorderColor = color[2];
            refresh_BTN.FlatAppearance.MouseDownBackColor = color[2];
            refresh_BTN.FlatAppearance.MouseOverBackColor = color[2];
            print_BTN.FlatAppearance.BorderColor = color[2];
            print_BTN.FlatAppearance.MouseDownBackColor = color[2];
            print_BTN.FlatAppearance.MouseOverBackColor = color[2];
            close_BTN.FlatAppearance.BorderColor = color[2];
            close_BTN.FlatAppearance.MouseDownBackColor = color[2];
            close_BTN.FlatAppearance.MouseOverBackColor = color[2];

            browser_BTN.BackColor = color[3];
            refresh_BTN.BackColor = color[3];
            print_BTN.BackColor = color[3];
            close_BTN.BackColor = color[3];

            toolStripStatusLabel.ForeColor = color[4];
            browser_BTN.ForeColor = color[4];
            refresh_BTN.ForeColor = color[4];
            print_BTN.ForeColor = color[4];
            close_BTN.ForeColor = color[4];
            zoom_BTN.ForeColor = color[4];
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
