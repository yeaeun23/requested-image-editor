using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ImageWork
{
    public partial class ChulgoForm : Form
    {
        DataTable dt;

        WorkForm workForm;
        string id = "";

        public ChulgoForm(WorkForm workForm, string id)
        {
            InitializeComponent();

            this.workForm = workForm;
            this.id = id;

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

        private void ChulgoForm_Load(object sender, EventArgs e)
        {
            // 컨트롤 로드
            Util.SetPanCBList(pan_CB);
            SetMyunCBList();

            // 컨트롤 초기화
            Init();
        }

        private void SetTheme(Color[] color)
        {
            BackColor = color[0];
            toolStripStatusLabel.BackColor = color[0];

            imgPreView.BackColor = color[1];

            chulgo_BTN.FlatAppearance.BorderColor = color[2];
            chulgo_BTN.FlatAppearance.MouseDownBackColor = color[2];
            chulgo_BTN.FlatAppearance.MouseOverBackColor = color[2];
            close_BTN.FlatAppearance.BorderColor = color[2];
            close_BTN.FlatAppearance.MouseDownBackColor = color[2];
            close_BTN.FlatAppearance.MouseOverBackColor = color[2];

            chulgo_BTN.BackColor = color[3];
            close_BTN.BackColor = color[3];

            groupBox1.ForeColor = color[4];
            toolStripStatusLabel.ForeColor = color[4];
            chulgo_BTN.ForeColor = color[4];
            close_BTN.ForeColor = color[4];
        }

        // 면 CB 리스트 세팅
        private void SetMyunCBList()
        {
            // 1 ~ 99면까지 추가
            for (int i = 1; i < 100; i++)
                myun_CB.Items.Add(i.ToString());
        }

        // 컨트롤 초기화
        private void Init()
        {
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select D_BEFOREPUBDATE, N_PAN, N_PAGE, V_OFILENAME from [DAPS2022].[dbo].[CMS_FLOWIMG] where ID_FIM = '{0}'", id)), "SELECT");

            // 컨트롤
            date_CAL.Value = Convert.ToDateTime(dt.Rows[0]["D_BEFOREPUBDATE"]);
            pan_CB.SelectedItem = dt.Rows[0]["N_PAN"].ToString();
            myun_CB.SelectedItem = dt.Rows[0]["N_PAGE"].ToString();

            // 타이틀
            Text = dt.Rows[0]["V_OFILENAME"].ToString();

            // 이미지
            string filePath = Form1.prevWorkFolderPath + "\\" + Text + ".jpg";

            if (File.Exists(filePath))
            {
                imgPreView.Image = workForm.LoadBitmap(filePath);
            }

            // 상태표시줄
            Util.SetStatusLabel(toolStripStatusLabel, "출고 미리보기 완료");
        }

        // 출고 버튼 클릭
        public void chulgo_BTN_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("출고하시겠습니까?", "화상 출고", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (dialog == DialogResult.Yes)
            {
                // 게재일, 판, 면 업데이트
                dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [DAPS2022].[dbo].[CMS_FLOWIMG] set D_BEFOREPUBDATE = '{0}', N_PAN = '{1}', N_PAGE = '{2}' from [DAPS2022].[dbo].[CMS_FLOWIMG] where ID_FIM = '{3}'", date_CAL.Value.ToString().Substring(0, 10), pan_CB.Text, myun_CB.Text, id)), "UPDATE");

                Thread T = new Thread(new ThreadStart(workForm.FileChulgo));
                T.Start();
            }

            Util.Delay(300);
            Close();
        }

        // 취소 버튼 클릭
        private void close_BTN_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ChulgoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            imgPreView.Image = null;
            imgPreView.Dispose();
        }
    }
}
