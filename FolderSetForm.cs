using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ImageWork
{
    public partial class FolderSetForm : Form
    {
        FontDialog fontDialog;

        public FolderSetForm(FontDialog fontDialog)
        {
            InitializeComponent();

            this.fontDialog = fontDialog;

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

        private void FolderSetForm_Load(object sender, EventArgs e)
        {
            originWorkFolderPath_TB.Text = Form1.originWorkFolderPath;
            realWorkFolderPath_TB.Text = Form1.realWorkFolderPath;
            prevWorkFolderPath_TB.Text = Form1.prevWorkFolderPath;
            thumbWorkFolderPath_TB.Text = Form1.thumbWorkFolderPath;
            realDownloadFolderPath_TB.Text = Form1.realDownloadFolderPath;
            prevDownloadFolderPath_TB.Text = Form1.prevDownloadFolderPath;
            thumbDownloadFolderPath_TB.Text = Form1.thumbDownloadFolderPath;

            toolTip1.SetToolTip(originWorkFolderBrowse_BTN, "폴더 찾아보기");
            toolTip1.SetToolTip(realWorkFolderBrowse_BTN, "폴더 찾아보기");
            toolTip1.SetToolTip(prevWorkFolderBrowse_BTN, "폴더 찾아보기");
            toolTip1.SetToolTip(thumbWorkFolderBrowse_BTN, "폴더 찾아보기");
            toolTip1.SetToolTip(realDownloadFolderBrowse_BTN, "폴더 찾아보기");
            toolTip1.SetToolTip(prevDownloadFolderBrowse_BTN, "폴더 찾아보기");
            toolTip1.SetToolTip(thumbDownloadFolderBrowse_BTN, "폴더 찾아보기");

            // 글꼴
            originWorkFolderPath_TB.Font = fontDialog.Font;
            originWorkFolderPath_TB.ForeColor = fontDialog.Color;
            realWorkFolderPath_TB.Font = fontDialog.Font;
            realWorkFolderPath_TB.ForeColor = fontDialog.Color;
            prevWorkFolderPath_TB.Font = fontDialog.Font;
            prevWorkFolderPath_TB.ForeColor = fontDialog.Color;
            thumbWorkFolderPath_TB.Font = fontDialog.Font;
            thumbWorkFolderPath_TB.ForeColor = fontDialog.Color;

            realDownloadFolderPath_TB.Font = fontDialog.Font;
            realDownloadFolderPath_TB.ForeColor = fontDialog.Color;
            prevDownloadFolderPath_TB.Font = fontDialog.Font;
            prevDownloadFolderPath_TB.ForeColor = fontDialog.Color;
            thumbDownloadFolderPath_TB.Font = fontDialog.Font;
            thumbDownloadFolderPath_TB.ForeColor = fontDialog.Color;
        }

        private void SetTheme(Color[] color)
        {
            BackColor = color[0];

            originWorkFolderPath_TB.BackColor = color[2];
            realWorkFolderPath_TB.BackColor = color[2];
            prevWorkFolderPath_TB.BackColor = color[2];
            thumbWorkFolderPath_TB.BackColor = color[2];
            realDownloadFolderPath_TB.BackColor = color[2];
            prevDownloadFolderPath_TB.BackColor = color[2];
            thumbDownloadFolderPath_TB.BackColor = color[2];
            originWorkFolderBrowse_BTN.FlatAppearance.BorderColor = color[2];
            originWorkFolderBrowse_BTN.FlatAppearance.MouseDownBackColor = color[2];
            originWorkFolderBrowse_BTN.FlatAppearance.MouseOverBackColor = color[2];
            realWorkFolderBrowse_BTN.FlatAppearance.BorderColor = color[2];
            realWorkFolderBrowse_BTN.FlatAppearance.MouseDownBackColor = color[2];
            realWorkFolderBrowse_BTN.FlatAppearance.MouseOverBackColor = color[2];
            prevWorkFolderBrowse_BTN.FlatAppearance.BorderColor = color[2];
            prevWorkFolderBrowse_BTN.FlatAppearance.MouseDownBackColor = color[2];
            prevWorkFolderBrowse_BTN.FlatAppearance.MouseOverBackColor = color[2];
            thumbWorkFolderBrowse_BTN.FlatAppearance.BorderColor = color[2];
            thumbWorkFolderBrowse_BTN.FlatAppearance.MouseDownBackColor = color[2];
            thumbWorkFolderBrowse_BTN.FlatAppearance.MouseOverBackColor = color[2];
            realDownloadFolderBrowse_BTN.FlatAppearance.BorderColor = color[2];
            realDownloadFolderBrowse_BTN.FlatAppearance.MouseDownBackColor = color[2];
            realDownloadFolderBrowse_BTN.FlatAppearance.MouseOverBackColor = color[2];
            prevDownloadFolderBrowse_BTN.FlatAppearance.BorderColor = color[2];
            prevDownloadFolderBrowse_BTN.FlatAppearance.MouseDownBackColor = color[2];
            prevDownloadFolderBrowse_BTN.FlatAppearance.MouseOverBackColor = color[2];
            thumbDownloadFolderBrowse_BTN.FlatAppearance.BorderColor = color[2];
            thumbDownloadFolderBrowse_BTN.FlatAppearance.MouseDownBackColor = color[2];
            thumbDownloadFolderBrowse_BTN.FlatAppearance.MouseOverBackColor = color[2];
            ok_BTN.FlatAppearance.BorderColor = color[2];
            ok_BTN.FlatAppearance.MouseDownBackColor = color[2];
            ok_BTN.FlatAppearance.MouseOverBackColor = color[2];
            cancel_BTN.FlatAppearance.BorderColor = color[2];
            cancel_BTN.FlatAppearance.MouseDownBackColor = color[2];
            cancel_BTN.FlatAppearance.MouseOverBackColor = color[2];

            originWorkFolderBrowse_BTN.BackColor = color[3];
            realWorkFolderBrowse_BTN.BackColor = color[3];
            prevWorkFolderBrowse_BTN.BackColor = color[3];
            thumbWorkFolderBrowse_BTN.BackColor = color[3];
            realDownloadFolderBrowse_BTN.BackColor = color[3];
            prevDownloadFolderBrowse_BTN.BackColor = color[3];
            thumbDownloadFolderBrowse_BTN.BackColor = color[3];
            ok_BTN.BackColor = color[3];
            cancel_BTN.BackColor = color[3];

            groupBox1.ForeColor = color[4];
            groupBox2.ForeColor = color[4];
            label1.ForeColor = color[4];
            originWorkFolderBrowse_BTN.ForeColor = color[4];
            realWorkFolderBrowse_BTN.ForeColor = color[4];
            prevWorkFolderBrowse_BTN.ForeColor = color[4];
            thumbWorkFolderBrowse_BTN.ForeColor = color[4];
            realDownloadFolderBrowse_BTN.ForeColor = color[4];
            prevDownloadFolderBrowse_BTN.ForeColor = color[4];
            thumbDownloadFolderBrowse_BTN.ForeColor = color[4];
            ok_BTN.ForeColor = color[4];
            cancel_BTN.ForeColor = color[4];
        }

        private void originWorkFolderBrowse_BTN_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = originWorkFolderPath_TB.Text;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                originWorkFolderPath_TB.Text = folderBrowserDialog.SelectedPath;
        }

        private void realWorkFolderBrowse_BTN_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = realWorkFolderPath_TB.Text;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                realWorkFolderPath_TB.Text = folderBrowserDialog.SelectedPath;
        }

        private void prevWorkFolderBrowse_BTN_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = prevWorkFolderPath_TB.Text;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                prevWorkFolderPath_TB.Text = folderBrowserDialog.SelectedPath;
        }

        private void thumbWorkFolderBrowse_BTN_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = thumbWorkFolderPath_TB.Text;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                thumbWorkFolderPath_TB.Text = folderBrowserDialog.SelectedPath;
        }

        private void realDownloadFolderBrowse_BTN_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = realDownloadFolderPath_TB.Text;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                realDownloadFolderPath_TB.Text = folderBrowserDialog.SelectedPath;
        }

        private void prevDownloadFolderBrowse_BTN_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = prevDownloadFolderPath_TB.Text;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                prevDownloadFolderPath_TB.Text = folderBrowserDialog.SelectedPath;
        }

        private void thumbDownloadFolderBrowse_BTN_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = thumbDownloadFolderPath_TB.Text;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                thumbDownloadFolderPath_TB.Text = folderBrowserDialog.SelectedPath;
        }

        private void ok_BTN_Click(object sender, EventArgs e)
        {
            if (originWorkFolderPath_TB.Text == "")
            {
                MessageBox.Show(this, "작업 폴더 경로를 설정해 주세요.", "폴더 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                originWorkFolderBrowse_BTN.Focus();
            }
            else if (realWorkFolderPath_TB.Text == "")
            {
                MessageBox.Show(this, "작업 폴더 경로를 설정해 주세요.", "폴더 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                realWorkFolderBrowse_BTN.Focus();
            }
            else if (prevWorkFolderPath_TB.Text == "")
            {
                MessageBox.Show(this, "작업 폴더 경로를 설정해 주세요.", "폴더 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                prevWorkFolderBrowse_BTN.Focus();
            }
            else if (thumbWorkFolderPath_TB.Text == "")
            {
                MessageBox.Show(this, "작업 폴더 경로를 설정해 주세요.", "폴더 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                thumbWorkFolderBrowse_BTN.Focus();
            }
            else if (realDownloadFolderPath_TB.Text == "")
            {
                MessageBox.Show(this, "다운로드 폴더 경로를 설정해 주세요.", "폴더 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                realDownloadFolderBrowse_BTN.Focus();
            }
            else if (prevDownloadFolderPath_TB.Text == "")
            {
                MessageBox.Show(this, "다운로드 폴더 경로를 설정해 주세요.", "폴더 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                prevDownloadFolderBrowse_BTN.Focus();
            }
            else if (thumbDownloadFolderPath_TB.Text == "")
            {
                MessageBox.Show(this, "다운로드 폴더 경로를 설정해 주세요.", "폴더 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                thumbDownloadFolderBrowse_BTN.Focus();
            }
            else
            {
                if (!Directory.Exists(originWorkFolderPath_TB.Text))
                {
                    MessageBox.Show(this, "작업 폴더 '" + originWorkFolderPath_TB.Text + "'을(를) 찾을 수 없습니다.", "폴더 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    originWorkFolderBrowse_BTN.Focus();
                }
                else if (!Directory.Exists(realWorkFolderPath_TB.Text))
                {
                    MessageBox.Show(this, "작업 폴더 '" + realWorkFolderPath_TB.Text + "'을(를) 찾을 수 없습니다.", "폴더 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    realWorkFolderBrowse_BTN.Focus();
                }
                else if (!Directory.Exists(prevWorkFolderPath_TB.Text))
                {
                    MessageBox.Show(this, "작업 폴더 '" + prevWorkFolderPath_TB.Text + "'을(를) 찾을 수 없습니다.", "폴더 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    prevWorkFolderBrowse_BTN.Focus();
                }
                else if (!Directory.Exists(thumbWorkFolderPath_TB.Text))
                {
                    MessageBox.Show(this, "작업 폴더 '" + thumbWorkFolderPath_TB.Text + "'을(를) 찾을 수 없습니다.", "폴더 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    thumbWorkFolderBrowse_BTN.Focus();
                }
                else if (!Directory.Exists(realDownloadFolderPath_TB.Text))
                {
                    MessageBox.Show(this, "다운로드 폴더 '" + realDownloadFolderPath_TB.Text + "'을(를) 찾을 수 없습니다.", "폴더 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    realDownloadFolderBrowse_BTN.Focus();
                }
                else if (!Directory.Exists(prevDownloadFolderPath_TB.Text))
                {
                    MessageBox.Show(this, "다운로드 폴더 '" + prevDownloadFolderPath_TB.Text + "'을(를) 찾을 수 없습니다.", "폴더 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    prevDownloadFolderBrowse_BTN.Focus();
                }
                else if (!Directory.Exists(thumbDownloadFolderPath_TB.Text))
                {
                    MessageBox.Show(this, "다운로드 폴더 '" + thumbDownloadFolderPath_TB.Text + "'을(를) 찾을 수 없습니다.", "폴더 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    thumbDownloadFolderBrowse_BTN.Focus();
                }
                else
                {
                    Form1.originWorkFolderPath = originWorkFolderPath_TB.Text;
                    Form1.realWorkFolderPath = realWorkFolderPath_TB.Text;
                    Form1.prevWorkFolderPath = prevWorkFolderPath_TB.Text;
                    Form1.thumbWorkFolderPath = thumbWorkFolderPath_TB.Text;
                    Form1.realDownloadFolderPath = realDownloadFolderPath_TB.Text;
                    Form1.prevDownloadFolderPath = prevDownloadFolderPath_TB.Text;
                    Form1.thumbDownloadFolderPath = thumbDownloadFolderPath_TB.Text;

                    DialogResult = DialogResult.OK;
                }
            }
        }

        private void cancel_BTN_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
