using System;
using System.IO;
using System.Windows.Forms;

namespace ImageWork
{
    public partial class FolderSetForm : Form
    {
        public FolderSetForm()
        {
            InitializeComponent();
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
