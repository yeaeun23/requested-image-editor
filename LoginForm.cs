using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageWork
{
    public partial class LoginForm : Form
    {
        DataTable dt;

        public LoginForm()
        {
            InitializeComponent();

            // 테마
            if (Form1.GetValue("THEME", "IDX_THEME") != "")
            {
                Form1.IDX_THEME = Convert.ToInt32(Form1.GetValue("THEME", "IDX_THEME"));

                if (Form1.IDX_THEME == Form1.IDX_THEME_DARK2)
                    SetTheme(Form1.themeDark2);
                else if (Form1.IDX_THEME == Form1.IDX_THEME_LIGHT1)
                    SetTheme(Form1.themeLight1);
                else if (Form1.IDX_THEME == Form1.IDX_THEME_LIGHT2)
                    SetTheme(Form1.themeLight2);
                else if (Form1.IDX_THEME == Form1.IDX_THEME_BLUE)
                    SetTheme(Form1.themeBlue);
            }
        }

        private void SetTheme(Color[] color)
        {
            BackColor = color[0];
            toolStripStatusLabel.BackColor = color[0];

            id_TB.BackColor = color[2];
            pw_TB.BackColor = color[2];
            login_BTN.FlatAppearance.BorderColor = color[2];
            login_BTN.FlatAppearance.MouseDownBackColor = color[2];
            login_BTN.FlatAppearance.MouseOverBackColor = color[2];

            login_BTN.BackColor = color[3];

            id_TB.ForeColor = color[4];
            pw_TB.ForeColor = color[4];
            login_BTN.ForeColor = color[4];
            toolStripStatusLabel.ForeColor = color[4];
            label2.ForeColor = color[4];
            label1.ForeColor = color[4];
            idSave_CHK.ForeColor = color[4];
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // 버전 라벨 세팅
            Form1.GetValue("VERSION", "VER");
            toolStripStatusLabel.Text = "Ver " + Form1.GetValue("VERSION", "VER");

            // 아이디 세팅
            if (Form1.GetValue("LOGIN", "ID_SAVE") == "TRUE")
            {
                idSave_CHK.Checked = true;

                id_TB.Text = Form1.GetValue("LOGIN", "ID");
            }

            // 포커스
            if (id_TB.Text == "")
                ActiveControl = id_TB;
            else
                ActiveControl = pw_TB;
        }

        private void login_BTN_Click(object sender, EventArgs e)
        {
            if (id_TB.Text == "")
            {
                MessageBox.Show("아이디를 입력해 주세요.", "로그인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ActiveControl = id_TB;
            }
            else if (pw_TB.Text == "")
            {
                MessageBox.Show("비밀번호를 입력해 주세요.", "로그인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ActiveControl = pw_TB;
            }
            else
            {
                dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select * from [CMSCOM].[dbo].[T_USERINFO] where B_NOTUSE = 0 and V_USERID = '" + id_TB.Text + "'")), "SELECT");

                // 아이디 확인
                if (dt.Rows.Count == 1)
                {
                    // 비밀번호 확인
                    if (MD5Create(pw_TB.Text) == dt.Rows[0]["mpassword"].ToString().ToLower())
                    {
                        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select * from [CMSCOM].[dbo].[R_USEREXE_PV] a inner join [CMSCOM].[dbo].[T_USERINFO] b on a.ID_USERCODE = b.ID_USERCODE where a.ID_SOFTCODE = 3 and b.B_NOTUSE = 0 and b.V_USERID = '" + id_TB.Text + "'")), "SELECT");

                        // 권한 확인
                        if (dt.Rows.Count == 1)
                        {
                            Form1.empNo = id_TB.Text;
                            Form1.empName = dt.Rows[0]["V_USERNAME"].ToString();
                            Form1.empCode = dt.Rows[0]["ID_USERCODE"].ToString();

                            DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            MessageBox.Show("로그인 권한이 없는 사용자입니다.", "로그인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        MessageBox.Show("비밀번호를 확인해 주세요.", "로그인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        ActiveControl = pw_TB;
                    }
                }
                else
                {
                    MessageBox.Show("아이디(사번 7자리)를 확인해 주세요.", "로그인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ActiveControl = id_TB;
                }
            }
        }

        private void id_TB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
                login_BTN_Click(sender, e);
        }

        private void pw_TB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
                login_BTN_Click(sender, e);
        }

        private string MD5Create(string pw)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(pw);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                    sb.Append(hashBytes[i].ToString("X2"));

                return sb.ToString().ToLower();
            }
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 아이디 저장 체크
            if (idSave_CHK.Checked)
            {
                Form1.WriteValue("LOGIN", "ID", id_TB.Text);
                Form1.WriteValue("LOGIN", "ID_SAVE", "TRUE");
            }
            else
            {
                Form1.WriteValue("LOGIN", "ID", "");
                Form1.WriteValue("LOGIN", "ID_SAVE", "FALSE");
            }
        }
    }
}
