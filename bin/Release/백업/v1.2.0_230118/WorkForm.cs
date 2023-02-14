using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ImageWork
{
    public partial class WorkForm : Form
    {
        DataTable dt;

        Form1 form1;
        string date = "";
        string id = "";
        string ext = "";
        string ext_o = "";
        string fileName = "";
        FontDialog fontDialog;

        string newFileName = "";

        // 이벤트
        string imgMode = "";
        int realWidth = 0;
        int realHeight = 0;
        int prevWidth = 0;
        int prevHeight = 0;
        int thumbWidth = 0;
        int thumbHeight = 0;
        bool firstEvent = true;

        public WorkForm(Form1 form1, string fileName, string ext, string ext_o, FontDialog fontDialog)
        {
            InitializeComponent();

            this.form1 = form1;
            this.fileName = fileName;
            this.ext = ext;
            this.ext_o = ext_o;
            this.fontDialog = fontDialog;

            fsw3.Path = Form1.eventWorkFolderPath;

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

        private void WorkForm_Load(object sender, EventArgs e)
        {
            // 꼬마창 위치
            string pos_x = Form1.GetValue("POS", "WORKFORM_X");
            string pos_y = Form1.GetValue("POS", "WORKFORM_Y");

            if (pos_x != "")
                Left = Convert.ToInt32(pos_x);

            if (pos_y != "")
                Top = Convert.ToInt32(pos_y);

            // 글꼴
            label_TB.Font = fontDialog.Font;
            label_TB.ForeColor = fontDialog.Color;

            width_TB.Font = fontDialog.Font;
            width_TB.ForeColor = fontDialog.Color;

            height_TB.Font = fontDialog.Font;
            height_TB.ForeColor = fontDialog.Color;

            retouch_TB.Font = fontDialog.Font;
            retouch_TB.ForeColor = fontDialog.Color;

            // 컨트롤 값 로드  
            SetImage(fileName, "");
        }

        private void SetTheme(Color[] color)
        {
            BackColor = color[0];
            toolStripStatusLabel.BackColor = color[0];

            imgPreView1.BackColor = color[1];
            imgPreView2.BackColor = color[1];

            label_TB.BackColor = color[2];
            width_TB.BackColor = color[2];
            height_TB.BackColor = color[2];
            retouch_TB.BackColor = color[2];
            save_BTN.FlatAppearance.BorderColor = color[2];
            save_BTN.FlatAppearance.MouseDownBackColor = color[2];
            save_BTN.FlatAppearance.MouseOverBackColor = color[2];
            chulgo_BTN.FlatAppearance.BorderColor = color[2];
            chulgo_BTN.FlatAppearance.MouseDownBackColor = color[2];
            chulgo_BTN.FlatAppearance.MouseOverBackColor = color[2];

            save_BTN.BackColor = color[3];
            chulgo_BTN.BackColor = color[3];

            groupBox3.ForeColor = color[4];
            groupBox1.ForeColor = color[4];
            toolStripStatusLabel.ForeColor = color[4];
            save_BTN.ForeColor = color[4];
            chulgo_BTN.ForeColor = color[4];
        }

        // 이미지 로드 (fileName: Form1에서 호출하는 경우 필요)
        public void SetImage(string fileName, string change)
        {
            this.fileName = fileName;

            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select * from [DAPS2022].[dbo].[CMS_FLOWIMG] where V_OFILENAME = '{0}'", this.fileName)), "SELECT");

            // 확장자
            ext = dt.Rows[0]["C_EXTENSION"].ToString().Trim();
            // 확장자(o)
            ext_o = dt.Rows[0]["V_TAG"].ToString().Trim();

            string filePath_org = Form1.originWorkFolderPath + "\\" + this.fileName + ".";
            string filePath_prev = Form1.prevWorkFolderPath + "\\" + this.fileName + ".";

            if (ext_o == "png" && ext == "png")
            {
                filePath_org += "png";
                filePath_prev += "png";
            }
            else
            {
                filePath_org += "jpg";
                filePath_prev += "jpg";
            }

            // 작업 파일이 존재하면
            if (File.Exists(filePath_org) && File.Exists(filePath_prev))
            {
                // 게재일
                date = dt.Rows[0]["D_BEFOREPUBDATE"].ToString().Substring(0, 10);
                // 아이디
                id = dt.Rows[0]["ID_FIM"].ToString();

                // 이미지
                imgPreView1.Image = null;
                imgPreView2.Image = null;

                imgPreView1.Image = LoadBitmap(filePath_org);
                imgPreView2.Image = LoadBitmap(filePath_prev);

                DrawCropLines();
                Util.SetStatusLabel(toolStripStatusLabel, "작업 미리보기 완료");

                SetInfo();

                // ActiveDocument 변경한 경우 새로고침 X 
                if (change == "")
                {
                    //UpdateTable("open");
                    form1.Search();
                }
            }
            else
            {
                // 존재하지 않으면 돌려놓기
                this.fileName = Text;

                dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select * from [DAPS2022].[dbo].[CMS_FLOWIMG] where V_OFILENAME = '{0}'", this.fileName)), "SELECT");

                // 확장자
                ext = dt.Rows[0]["C_EXTENSION"].ToString().Trim();
                // 확장자(o)
                ext_o = dt.Rows[0]["V_TAG"].ToString().Trim();

                MessageBox.Show(this, "화상 작업기를 통해 다시 열어주세요.", "화상 작업", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // 상세정보 로드
        private void SetInfo()
        {
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select V_RETOUCH from [DAPS2022].[dbo].[CMS_FLOWIMG] where ID_FIM = '{0}'", id)), "SELECT");

            // 리터치
            string retouch = dt.Rows[0]["V_RETOUCH"].ToString();

            if (retouch != "")
            {
                label_TB.Text = retouch.Split(':')[0];
                width_TB.Text = retouch.Split(':')[1];
                height_TB.Text = retouch.Split(':')[2];

                if (retouch.Split(':')[3] == "C")
                    color_RB1.Checked = true;
                else
                    color_RB2.Checked = true;

                retouch_TB.Text = retouch.Split(':')[4];
            }

            // 타이틀      
            Text = fileName;

            Util.SetStatusLabel(toolStripStatusLabel, "전표 상세보기 완료");
        }

        public Bitmap LoadBitmap(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader reader = new BinaryReader(stream))
                        {
                            MemoryStream ms = new MemoryStream(reader.ReadBytes((int)stream.Length));
                            return new Bitmap(ms);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.SaveLog(ex.ToString());
            }

            return null;
        }

        // 전표 안내선
        private void DrawCropLines()
        {
            imgCropLine.Image = null;

            try
            {
                // 좌표값 구하기
                dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select N_CROPLEFT, N_CROPTOP, N_CROPRIGHT, N_CROPBOTTOM from [DAPS2022].[dbo].[CMS_FLOWIMG] where id_fim = '{0}'", id)), "SELECT");

                int x1 = Convert.ToInt32(dt.Rows[0]["N_CROPLEFT"]);
                int y1 = Convert.ToInt32(dt.Rows[0]["N_CROPTOP"]);
                int x2 = Convert.ToInt32(dt.Rows[0]["N_CROPRIGHT"]);
                int y2 = Convert.ToInt32(dt.Rows[0]["N_CROPBOTTOM"]);

                // 좌표값 있으면 그리기
                if (x1 + y1 + x2 + y2 != 0)
                {
                    int real_w = imgPreView1.PreferredSize.Width;
                    int real_h = imgPreView1.PreferredSize.Height;

                    int imgpreview1_w = imgPreView1.Width;
                    int imgpreview1_h = imgPreView1.Height;

                    Bitmap bmp = new Bitmap(imgpreview1_w, imgpreview1_h);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        Pen pen = new Pen(Color.Red, 6);
                        int pic_h, pic_w;

                        if (real_w >= real_h)
                        {
                            pic_h = imgpreview1_w * real_h / real_w;
                            pic_w = imgpreview1_w;
                        }
                        else
                        {
                            pic_w = imgpreview1_h * real_w / real_h;
                            pic_h = imgpreview1_h;
                        }

                        x1 = x1 * pic_w / real_w;
                        y1 = y1 * pic_h / real_h;
                        x2 = x2 * pic_w / real_w;
                        y2 = y2 * pic_h / real_h;

                        Rectangle rect = new Rectangle(x1 + (imgpreview1_w / 2 - pic_w / 2), y1 + (imgpreview1_h / 2 - pic_h / 2), x2 - x1, y2 - y1);
                        g.DrawRectangle(pen, rect);
                    }

                    imgCropLine.Parent = imgPreView1;
                    imgCropLine.Image = bmp;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.SaveLog(ex.ToString());
            }
        }

        // 저장/출고 가능한 상태인지 체크
        private bool CheckStatus()
        {
            try
            {
                dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select ID_LOCKERCODE from [DAPS2022].[dbo].[CMS_FLOWIMG] where ID_FIM = '{0}' and NM_FLOWSTATE = 0 and B_DELETE = 0", id)), "SELECT");

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show(this, "이미 출고/삭제된 파일입니다.", "화상 작업", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                else if (dt.Rows[0]["ID_LOCKERCODE"].ToString() != Form1.empCode)
                {
                    MessageBox.Show(this, "작업중(잠금)이 해제된 파일입니다.\n\n화상 작업기를 통해 다시 열어주세요.", "화상 작업", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                if (Form1.photoshop.Documents.Count == 0)
                {
                    MessageBox.Show(this, "포토샵에서 작업중인 파일이 없습니다.\n\n화상 작업기를 통해 다시 열어주세요.", "화상 작업", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                if (Text != Path.GetFileNameWithoutExtension(Form1.photoshop.ActiveDocument.Name))
                {
                    MessageBox.Show(this, "포토샵에서 꼬마창과 동일한 파일을 선택하세요.", "화상 작업", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                if (!Form1.photoshop.ActiveDocument.Saved)
                {
                    MessageBox.Show(this, "포토샵에서 작업중인 파일을 저장(Ctrl+S)하세요.", "화상 작업", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            catch (COMException)
            {
                MessageBox.Show(this, "포토샵에서 진행중인 창이 있습니다.", "화상 작업", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.SaveLog(ex.ToString());
            }

            return true;
        }

        // 저장 버튼 클릭
        private void save_BTN_Click(object sender, EventArgs e)
        {
            if (!CheckStatus())
                return;

            try
            {
                string ext_doc = Form1.photoshop.ActiveDocument.Name.Split('.')[1].ToLower();

                if (ext_doc != "jpg" && ext_doc != "eps" && ext_doc != "psd" && ext_doc != "png")
                {
                    MessageBox.Show(this, "서버에 저장할 수 없는 파일 형식입니다.\n\n* jpg, eps, psd, png 파일만 가능", "화상 저장", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (ext_o == "png" && ext_doc == "png")
                {
                    MessageBox.Show(this, "서버에 저장할 수 없는 파일 형식입니다.\n\n* 원본 파일이 png일 경우, jpg, eps, psd 파일만 가능", "화상 저장", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    DialogResult dialog = MessageBox.Show(this, "서버에 저장하시겠습니까?", "화상 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                    if (dialog == DialogResult.Yes)
                    {
                        Thread T = new Thread(new ThreadStart(FileSave));
                        T.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.SaveLog(ex.ToString());
            }
        }

        // 파일 저장
        private void FileSave()
        {
            Util.SetStatusLabel(toolStripStatusLabel, "파일 저장 중..");

            if (FileUpload(Form1.realWorkFolderPath, "REAL", "1") == "S")
            {
                if (FileUpload(Form1.prevWorkFolderPath, "PREV", "1") == "S")
                {
                    if (FileUpload(Form1.thumbWorkFolderPath, "THUMB", "1") == "S")
                    {
                        // 마지막 작업 파일 확장자 구하기
                        string ext_doc = Form1.photoshop.ActiveDocument.Name.Split('.')[1].ToLower();

                        if (ext_doc != "jpg")
                        {
                            if (FileUpload(Form1.realWorkFolderPath, "REAL_" + ext_doc, "1") == "S")
                            {
                                if (UpdateTable("save") == "S")
                                {
                                    Util.SetStatusLabel(toolStripStatusLabel, "파일 저장 완료");

                                    MessageBox.Show(this, "저장되었습니다.", "화상 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    Util.SaveLog("File Save Complete: 파일 ID: " + id);

                                    form1.Search();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (UpdateTable("save") == "S")
                            {
                                Util.SetStatusLabel(toolStripStatusLabel, "파일 저장 완료");

                                MessageBox.Show(this, "저장되었습니다.", "화상 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Util.SaveLog("File Save Complete: 파일 ID: " + id);

                                form1.Search();
                                return;
                            }
                        }
                    }
                }
            }

            Util.SetStatusLabel(toolStripStatusLabel, "파일 저장 실패");
            MessageBox.Show(this, "파일 저장 중 오류가 발생했습니다. IT개발팀으로 문의 바랍니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // 파일 업로드
        private string FileUpload(string folderPath, string kind, string server)
        {
            string result = "F";
            string uploadPath = "";
            string filePath = "";

            // 경로 구분
            if (server == "1")
            {
                uploadPath = Form1.uploadUrl1 + "?imgkind=" + kind.Split('_')[0] + "&date=" + date;
                // Ex) ctssvr1.seoul.co.kr/seoulphoto/filesave.aspx?imgkind=REAL&date=2022-12-02 -> SSCPHOTO 폴더로     

                if (kind.StartsWith("REAL_"))
                    filePath = folderPath + "\\" + fileName + "." + kind.Split('_')[1];
                else
                    filePath = folderPath + "\\" + fileName + ".jpg";
            }
            else if (server == "2")
            {
                // 파일명 설정
                dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select N_PAN, N_PAGE from [DAPS2022].[dbo].[CMS_FLOWIMG] where ID_FIM = '{0}'", id)), "SELECT");

                newFileName = "IM" + date.Replace("-", "") + dt.Rows[0]["N_PAN"].ToString().PadLeft(2, '0') + dt.Rows[0]["N_PAGE"].ToString().PadLeft(2, '0');
                newFileName += "01"; // 지방
                newFileName += id.PadLeft(8, '0');

                uploadPath = Form1.uploadUrl2 + "?media=65&year=" + date.Substring(0, 4) + "&month=" + date.Substring(5, 2) + "&day=" + date.Substring(8, 2) + "&desFileName=" + newFileName;
                // Ex) ctssvr1.seoul.co.kr/PhotoMgr/filesave.aspx?media=65&year=2022&month=12&day=02&desFileName=IM2022120205990110000159R.EPS -> CTS 폴더로

                if (kind == "REAL_EPS")
                {
                    uploadPath += "R.EPS";
                    filePath = folderPath + "\\" + fileName + ".eps";
                }
                else if (kind == "PREV")
                {
                    uploadPath += "T.JPG"; // P.JPG로 바꾸지 말것
                    filePath = folderPath + "\\" + fileName + ".jpg";
                }
            }

            // 파일 업로드
            try
            {
                using (WebClient wc = new WebClient())
                {
                    byte[] resultArray = wc.UploadFile(uploadPath, filePath);
                    result = ((char)resultArray[0]).ToString();
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();

                if (result != "S")
                    Util.SaveLog("File Upload Fail: 파일 ID: " + id + ", action: " + kind + ", uploadPath: " + uploadPath + ", filePath: " + filePath + "\n파일 업로드(filesave.aspx)에 실패했습니다.");
            }
            catch (Exception ex)
            {
                Util.SaveLog("File Upload Fail: 파일 ID: " + id + ", action: " + kind + "\n" + ex);
            }

            return result;
        }

        // 출고 버튼 클릭
        private void chulgo_BTN_Click(object sender, EventArgs e)
        {
            if (!CheckStatus())
                return;

            try
            {
                string mode = Form1.photoshop.ActiveDocument.Mode.ToString();

                if (mode != "psCMYK" && mode != "psGrayscale")
                {
                    MessageBox.Show(this, "출고할 수 없는 파일 형식입니다.\n\n* CMYK 또는 Gray 모드만 가능", "화상 출고", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (Form1.photoshop.ActiveDocument.Name.Split('.')[1].ToLower() != "eps")
                {
                    MessageBox.Show(this, "출고할 수 없는 파일 형식입니다.\n\n* eps 파일만 가능", "화상 출고", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    ChulgoForm chulgoForm = new ChulgoForm(this, id);
                    chulgoForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.SaveLog(ex.ToString());
            }
        }

        // 파일 출고
        public void FileChulgo()
        {
            Util.SetStatusLabel(toolStripStatusLabel, "파일 출고 중..");

            if (FileUpload(Form1.realWorkFolderPath, "REAL", "1") == "S")
            {
                if (FileUpload(Form1.prevWorkFolderPath, "PREV", "1") == "S")
                {
                    if (FileUpload(Form1.thumbWorkFolderPath, "THUMB", "1") == "S")
                    {
                        if (FileUpload(Form1.realWorkFolderPath, "REAL_EPS", "2") == "S")
                        {
                            if (FileUpload(Form1.prevWorkFolderPath, "PREV", "2") == "S")
                            {
                                // 마지막 작업 파일 확장자 구하기
                                string ext_doc = Form1.photoshop.ActiveDocument.Name.Split('.')[1].ToLower();

                                if (ext_doc != "jpg")
                                {
                                    if (FileUpload(Form1.realWorkFolderPath, "REAL_" + ext_doc, "1") == "S")
                                    {
                                        if (UpdateTable("chulgo") == "S")
                                        {
                                            Util.SetStatusLabel(toolStripStatusLabel, "파일 출고 완료");

                                            MessageBox.Show(this, "출고되었습니다.", "화상 출고", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            Util.SaveLog("File Chulgo Complete: 파일 ID: " + id);

                                            form1.Search();
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    if (UpdateTable("chulgo") == "S")
                                    {
                                        Util.SetStatusLabel(toolStripStatusLabel, "파일 출고 완료");

                                        MessageBox.Show(this, "출고되었습니다.", "화상 출고", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        Util.SaveLog("File Chulgo Complete: 파일 ID: " + id);

                                        form1.Search();
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Util.SetStatusLabel(toolStripStatusLabel, "파일 출고 실패");
            MessageBox.Show(this, "파일 출고 중 오류가 발생했습니다. IT개발팀으로 문의 바랍니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // DB 인서트/업데이트
        private string UpdateTable(string action)
        {
            string result = "F";

            try
            {
                string pubpart = "";
                string pan = "";
                string page = "";

                Util.getPanPage(ref pubpart, ref pan, ref page, id);

                if (action == "close")
                {
                    // FLOWIMG 업데이트
                    dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [DAPS2022].[dbo].[CMS_FLOWIMG] 
                    set ID_LOCKERCODE = NULL, V_BEFORESTATE = NULL
                    where ID_LOCKERCODE = '{0}' and V_BEFORESTATE like '작업중%'", Form1.empCode)), "UPDATE");

                    dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [DAPS2022].[dbo].[CMS_FLOWIMG] 
                    set ID_LOCKERCODE = NULL
                    where ID_LOCKERCODE = '{0}' and V_BEFORESTATE like '제작출고%'", Form1.empCode, Form1.empName)), "UPDATE");

                    // WORKHISTORY 인서트
                    dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"insert into [DAPS2022].[dbo].[CMS_WORKHISTORY] 
                    (ID_USERCODE, D_WORKDATE, V_CONTENT, N_WORKKIND, N_WORKCODE, N_PUBPART, ID_FID, N_CONTENT_TYPE, C_APP_TYPE) values 
                    ('{0}', getdate(), '화상 ID : {1} ({2}면, {3}판, {4}부서코드)', '화상닫기', '1001', '{5}', '{6}', '2', 'A')",
                    Form1.empCode, id, page, pan, pubpart, Util.getPubpartName(Form1.empCode), id)), "INSERT");
                }
                else if (action == "save" || action == "chulgo")
                {
                    string filePath = Form1.realWorkFolderPath + "\\" + fileName + ".";

                    if (ext_o == "png" && ext == "png")
                        filePath += ext;
                    else
                        filePath += "jpg";

                    string ext_doc = Form1.photoshop.ActiveDocument.Name.Split('.')[1].ToLower();
                    long size = new FileInfo(filePath).Length;

                    // dpi 구하기
                    Image img;
                    using (var bmpTemp = new Bitmap(filePath))
                    {
                        img = new Bitmap(bmpTemp);
                    }

                    float dpi = img.HorizontalResolution;
                    img = null;

                    // width, height 구하기
                    Photoshop.PsUnits unit = Form1.photoshop.Preferences.RulerUnits; // 쓰던 단위 저장

                    Form1.photoshop.Preferences.RulerUnits = Photoshop.PsUnits.psCM;
                    int width_cm = (int)Math.Round(Form1.photoshop.ActiveDocument.Width * 100);
                    int height_cm = (int)Math.Round(Form1.photoshop.ActiveDocument.Height * 100);

                    Form1.photoshop.Preferences.RulerUnits = Photoshop.PsUnits.psPixels;
                    int width_pixel = (int)Math.Round(Form1.photoshop.ActiveDocument.Width);
                    int height_pixel = (int)Math.Round(Form1.photoshop.ActiveDocument.Height);

                    Form1.photoshop.Preferences.RulerUnits = unit; // 쓰던 단위 원복       

                    // FLOWIMG 업데이트
                    dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [DAPS2022].[dbo].[CMS_FLOWIMG] set 
                    D_LASTUPDATE = getdate(), 
                    N_DPI = '{0}',
                    N_WIDTH = '{1}',
                    N_HEIGHT = '{2}',
                    C_EXTENSION = '{3}', 
                    N_PIXELX = '{4}',
                    N_PIXELY = '{5}',
                    N_FILESIZE = '{6}', 
                    N_UPDATECNT = N_UPDATECNT + 1,
                    ID_LASTUSER = '{7}'
                    where ID_FIM = '{8}'", dpi, width_cm, height_cm, ext_doc, width_pixel, height_pixel, size, Form1.empCode, id)), "UPDATE");

                    if (action == "save")
                    {
                        // WORKHISTORY 인서트
                        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"insert into [DAPS2022].[dbo].[CMS_WORKHISTORY] 
                        (ID_USERCODE, D_WORKDATE, V_CONTENT, N_WORKKIND, N_WORKCODE, N_PUBPART, ID_FID, N_CONTENT_TYPE, C_APP_TYPE) values 
                        ('{0}', getdate(), '화상 ID : {1} ({2}면, {3}판, {4}부서코드)', '화상저장', '1003', '{5}', '{6}', '2', 'A')",
                        Form1.empCode, id, page, pan, pubpart, Util.getPubpartName(Form1.empCode), id)), "INSERT");
                    }
                    else if (action == "chulgo")
                    {
                        // CMS_IMGINFO 인서트, CMS_IMGREF 인서트
                        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"BEGIN TRAN
                        DECLARE @return_value int,
                                @nGenID int,
                                @img_id bigint,
                                @title varchar(60),
                                @imgCnt int
                        EXEC    @return_value = [DAPS2022].[dbo].[sp_GETID]
                                @nTBLKindID = 3,
                                @nGenID = @nGenID OUTPUT
                        SET     @img_id = @nGenID
                        SELECT  @title = CAST(V_TITLE AS VARCHAR(60)) FROM [DAPS2022].[dbo].[CMS_FLOWIMG] WHERE ID_FIM = '" + id + @"'
                        SELECT  @imgCnt = COUNT(*) + 1 FROM [DAPS2022].[dbo].[CMS_IMGINFO] 
                                          WHERE media = '65' AND jibang = '1' AND pan = '" + pan + @"' AND myun = '" + page + @"' AND paper_date = '" + date.Replace("-", "") + @"'

                        INSERT INTO [DAPS2022].[dbo].[CMS_IMGINFO] VALUES
                           (@img_id--img_id
                           ,'65'--media
                           ,'" + date.Substring(5, 2) + @"'--month
                           ,'" + date.Substring(8, 2) + @"'--day
                           ,'" + pan + @"'--pan
                           ,'" + page + @"'--myun
                           ,'1'--jibang
                           ,'0'--img_loc
                           ,NULL--ad_img_id
                           ,FORMAT(@imgCnt, '000/') + @title--title
                           ,'0'--kisa_id
                           ,'5'--img_type
                           ,'1'--input_type
                           ,'1'--color
                           ,NULL--onoff_c
                           ,NULL--onoff_m
                           ,NULL--onoff_y
                           ,NULL--onoff_k
                           ,'0'--hochul_stat
                           ,'0'--hochul_id
                           ,'" + id + @"'--uuid
                           ,GETDATE()--cre_date
                           ,'" + Form1.empCode + @"'--cre_user
                           ,'0'--format 
                           ,'1'--b_thumb 
                           ,'0'--thumb_sz 
                           ,NULL--dummy1
                           ,'0'--thumb_hsz 
                           ,'0'--thumb_vsz 
                           ,'" + newFileName + @"T.JPG'--thumb_file  
                           ,'" + newFileName + @"R.EPS'--print_file 
                           ,'" + Form1.empName + @"'--ad_host 
                           ,'" + width_pixel + @"'--x_pixel 
                           ,'" + height_pixel + @"'--y_pixel 
                           ,'" + dpi + @"'--x_resolution 
                           ,'" + dpi + @"'--y_resolution 
                           ,'1'--xy_units 
                           ,NULL--dummy2 
                           ,'1'--b_real 
                           ,'0'--real_sz 
                           ,'" + width_cm + @"'--real_hsz 
                           ,'" + height_cm + @"'--real_vsz 
                           ,'" + newFileName + @"R.EPS'--real_file 
                           ,@img_id--org_img_id	
                           ,NULL--save_count 
                           ,@img_id--ref_id 
                           ,'" + pan + @"'--reg_pan 
                           ,NULL--status 
                           ,'" + date.Replace("-", "") + @"'--paper_date
                           ,'0'--adgubun
                           ,'0'--isset
                        );

                        INSERT INTO [DAPS2022].[dbo].[CMS_IMGREF] VALUES
                            (@img_id--id
                            ,'" + newFileName + @"R.EPS'--real_file 
                            ,'" + newFileName + @"T.JPG'--thumb_file
                            ,NULL--preview_file
                            ,NULL--save_count
                            ,'1'--ref_count
                            ,'5'--img_type
                            ,'1'--input_type
                            ,'1'--color
                            ,NULL--onoff_c
                            ,NULL--onoff_m
                            ,NULL--onoff_y
                            ,NULL--onoff_k
                            ,GETDATE()--cre_date
                            ,'" + Form1.empCode + @"'--cre_user               
                            ,'0'--format
                            ,'1'--b_thumb
                            ,'0'--thumb_sz
                            ,'" + Form1.empName + @"'--ad_host
                            ,'" + width_pixel + @"'--x_pixel
                            ,'" + height_pixel + @"'--y_pixel
                            ,'" + dpi + @"'--x_resolution
                            ,'" + dpi + @"'--y_resolution
                            ,'1'--xy_units
                            ,'1'--b_real
                            ,'0'--real_sz
                            ,'0'--print_sz
                            ,'" + date.Substring(5, 2) + @"'--s_month       
                            ,'" + date.Substring(8, 2) + @"'--s_day       
                            ,'65'--s_media
                            ,'" + width_cm + @"'--real_hsz
                            ,'" + height_cm + @"'--real_vsz
                            ,'0'--kisasvr_id
                            ,'0'--is_backup
                            ,'" + date.Replace("-", "") + @"'--s_paper_date
                        );

                        commit;")), "INSERT");

                        // FLOWIMG 업데이트 - 위에서 오류날 수 있으니 순서 바꾸지 말기
                        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [DAPS2022].[dbo].[CMS_FLOWIMG] set D_PUBDATE = getdate(), NM_FLOWSTATE = '1', V_BEFORESTATE = '제작출고({0})', N_PUBLISHCNT = N_PUBLISHCNT + 1 where ID_FIM = '{1}'", Form1.empName, id)), "UPDATE");

                        // WORKHISTORY 인서트 - 〃
                        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"insert into [DAPS2022].[dbo].[CMS_WORKHISTORY] 
                        (ID_USERCODE, D_WORKDATE, V_CONTENT, N_WORKKIND, N_WORKCODE, N_PUBPART, ID_FID, N_CONTENT_TYPE, C_APP_TYPE) values 
                        ('{0}', getdate(), '화상 ID : {1} ({2}면, {3}판, {4}부서코드)', '화상출고', '1001', '{5}', '{6}', '2', 'A')",
                        Form1.empCode, id, page, pan, pubpart, Util.getPubpartName(Form1.empCode), id)), "INSERT");
                    }
                }

                result = "S";
            }
            catch (Exception ex)
            {
                Util.SaveLog("DB Update Fail: 파일 ID: " + id + ", action: " + action + "\n" + ex);
            }

            return result;
        }

        // 취소 버튼 클릭
        private void close_BTN_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void WorkForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateTable("close");

            // 꼬마창 위치 저장
            Form1.WriteValue("POS", "WORKFORM_X", Left.ToString());
            Form1.WriteValue("POS", "WORKFORM_Y", Top.ToString());

            // 작업 파일 삭제
            Form1.deleteWorkFile(Form1.eventWorkFolderPath);

            // 이미지
            imgPreView1.Image = null;
            imgPreView2.Image = null;
            imgCropLine.Image = null;

            imgPreView1.Dispose();
            imgPreView2.Dispose();
            imgCropLine.Dispose();

            fsw3.Dispose(); // 필수: 꼬마창 닫아도 이벤트 안사라지는 문제

            form1.Search();
        }

        // 이벤트 파일 처리
        private void fsw3_Changed(object sender, FileSystemEventArgs e)
        {
            string filename = Path.GetFileNameWithoutExtension(e.Name).Replace("=", ".");

            try
            {
                if (filename.StartsWith("save&"))           // 저장(Ctrl+S)한 경우
                {
                    imageProcessStart(filename.Replace("save&", ""));

                    File.Delete(e.FullPath);
                }
                else if (filename.StartsWith("change&"))    // ActiveDocument 변경한 경우
                {
                    if (Form1.photoshop.ActiveDocument.Name.StartsWith("zz-"))
                    {
                        fileName = Path.GetFileNameWithoutExtension(Form1.photoshop.ActiveDocument.Name);
                        // 꼬마창 다시 로드
                        SetImage(fileName, "change");

                        File.Delete(e.FullPath);
                    }
                }
            }
            catch (COMException)
            {
                // 저장 안했는데 닫는 경우
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.SaveLog(ex.ToString());
            }
        }

        // 이미지 생성 - 포토샵 깜빡임 방지
        private void imageProcessStart(string filename)
        {
            string inFileName = "";
            string outFileName = "";

            Util.SaveLog("imageProcessStart: " + filename);

            try
            {
                imgMode = Form1.photoshop.ActiveDocument.Mode.ToString();

                // real 크기 계산
                Photoshop.PsUnits unit = Form1.photoshop.Preferences.RulerUnits; // 쓰던 단위 저장

                Form1.photoshop.Preferences.RulerUnits = Photoshop.PsUnits.psPixels;
                realWidth = Convert.ToInt32(Form1.photoshop.ActiveDocument.Width);
                realHeight = Convert.ToInt32(Form1.photoshop.ActiveDocument.Height);

                Form1.photoshop.Preferences.RulerUnits = unit; // 쓰던 단위 원복

                // prev, thumb 크기 계산
                getImageSize();

                // real 이미지 생성
                inFileName = Form1.realWorkFolderPath + "\\" + filename;
                outFileName = Form1.realWorkFolderPath + "\\" + Path.GetFileNameWithoutExtension(filename);
                imageProcess(inFileName, outFileName, realWidth, realHeight, "real");

                // prev 이미지 생성
                if (firstEvent && ext_o == "png" && ext == "png")
                    inFileName = Form1.realWorkFolderPath + "\\" + Path.GetFileNameWithoutExtension(filename) + "." + ext_o;
                else
                    inFileName = Form1.realWorkFolderPath + "\\" + Path.GetFileNameWithoutExtension(filename) + ".jpg";

                outFileName = Form1.prevWorkFolderPath + "\\" + Path.GetFileNameWithoutExtension(filename);
                imageProcess(inFileName, outFileName, prevWidth, prevHeight, "prev");

                // thumb 이미지 생성
                if (firstEvent && ext_o == "png" && ext == "png")
                    inFileName = Form1.realWorkFolderPath + "\\" + Path.GetFileNameWithoutExtension(filename) + "." + ext_o;
                else
                    inFileName = Form1.realWorkFolderPath + "\\" + Path.GetFileNameWithoutExtension(filename) + ".jpg";

                outFileName = Form1.thumbWorkFolderPath + "\\" + Path.GetFileNameWithoutExtension(filename);
                imageProcess(inFileName, outFileName, thumbWidth, thumbHeight, "thumb");

                // 꼬마창에 prev 이미지 다시 로드
                string filePath = Form1.prevWorkFolderPath + "\\" + Path.GetFileNameWithoutExtension(filename) + ".jpg";

                if (File.Exists(filePath))
                {
                    imgPreView2.Image = null;
                    imgPreView2.Image = LoadBitmap(filePath);
                    Util.SetStatusLabel(toolStripStatusLabel, "작업 미리보기 완료");
                }

                firstEvent = false;
            }
            catch (Exception)
            {
            }
        }

        private void imageProcess(string inFileName, string outFileName, int nWidth, int nHeight, string type)
        {
            string excuteName = "";
            string strArg = "";

            if (Path.GetExtension(inFileName).ToLower() == ".eps")
            {
                excuteName = @".\gswin64c.exe";

                if (type == "real")
                {
                    strArg = string.Format("-sDEVICE=jpeg -dJPEGQ=100 -dNOPAUSE -dEPSCrop -dBATCH -dSAFER -r96 -sOutputFile={0}.jpg {1}", outFileName, inFileName);
                }
                else
                {
                    string path = Path.GetDirectoryName(inFileName) + "\\" + Path.GetFileNameWithoutExtension(inFileName);

                    strArg = string.Format("-sDEVICE=jpeg -dJPEGQ=100 -dNOPAUSE -dEPSCrop -dBATCH -dSAFER -r96 -g{0}x{1} -sOutputFile={2}.jpg {3}.jpg", nWidth.ToString(), nHeight.ToString(), outFileName, path);
                }

                imgMode = "psRGB"; // real 처리 후 변경
            }
            else
            {
                excuteName = @".\magick.exe";

                if (type == "real" && Path.GetExtension(inFileName).ToLower() == ".jpg")
                {
                    // real JPG인 경우, 이미지 리사이징 SKIP
                }
                else
                {
                    if (imgMode == "psCMYK")
                    {
                        strArg = string.Format("{0} -profile {1}\\sRGB.icc -density 96 -resize {2}x{3} {4}.jpg", inFileName, Path.GetDirectoryName(excuteName), nWidth.ToString(), nHeight.ToString(), outFileName);
                    }
                    else
                    {
                        strArg = string.Format("{0} -density 96 -resize {1}x{2} {3}.jpg", inFileName, nWidth.ToString(), nHeight.ToString(), outFileName);
                    }
                }
            }

            ProcessStartInfo psi = new ProcessStartInfo { FileName = excuteName };
            Process p = new Process();
            bool success = true;

            try
            {
                psi.CreateNoWindow = false;
                psi.UseShellExecute = true;
                psi.Arguments = strArg;

                p.StartInfo = psi;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                p.Start();
                p.WaitForExit();

                if (p.ExitCode != 0)
                    success = false;    // 변환 실패

                p.Kill();
            }
            catch (Exception)
            {
            }
            finally
            {
                p.Dispose();
            }
        }

        private void getImageSize()
        {
            float rate = 0;

            if (realWidth >= realHeight)
            {
                rate = (float)realHeight / realWidth;

                prevWidth = 640;
                prevHeight = (int)(640 * rate);

                thumbWidth = 128;
                thumbHeight = (int)(128 * rate);
            }
            else
            {
                rate = (float)realWidth / realHeight;

                prevWidth = (int)(640 * rate);
                prevHeight = 640;

                thumbWidth = (int)(128 * rate);
                thumbHeight = 128;
            }
        }
    }
}
