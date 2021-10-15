using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace ImageWork
{
    public partial class WorkForm : Form
    {
        DataTable dt;

        Form1 form1;
        FontDialog fontDialog;
        
        string id = "";
        string filename_thumb = "";
        string filename_prev = "";
        string filename_real = "";
        string filename_eps = "";        

        public WorkForm(Form1 form1, string id, FontDialog fontDialog)
        {
            InitializeComponent();

            this.form1 = form1;

            this.id = id;
            filename_thumb = id + Form1.thumbExtMark;
            filename_prev = id + Form1.prevExtMark;
            filename_real = id + Form1.realExtMark;
            filename_eps = id + Form1.epsExtMark;

            this.fontDialog = fontDialog;
            
            fsw1.Path = Form1.originWorkFolderPath;
            fsw2.Path = Form1.prevWorkFolderPath;
        }

        private void WorkForm_Load(object sender, EventArgs e)
        {
            UpdateTable("open");            

            // 글꼴
            label_TB.Font = fontDialog.Font;
            label_TB.ForeColor = fontDialog.Color;

            width_TB.Font = fontDialog.Font;
            width_TB.ForeColor = fontDialog.Color;

            height_TB.Font = fontDialog.Font;
            height_TB.ForeColor = fontDialog.Color;

            retouch_TB.Font = fontDialog.Font;
            retouch_TB.ForeColor = fontDialog.Color;            
            
            // 타이틀      
            Text = filename_real;

            // 이미지, 정보, 상태       
            SetImage();             
            SetInfo();            
        }  

        private void SetImage()
        {
            if (File.Exists(Form1.originWorkFolderPath + "\\" + filename_real))
            {
                imgPreView1.Image = LoadBitmap(Form1.originWorkFolderPath + "\\" + filename_real);
                SetStausLabel("전표 미리보기 완료");
            }

            if (File.Exists(Form1.prevWorkFolderPath + "\\" + filename_prev))
            {
                imgPreView2.Image = LoadBitmap(Form1.prevWorkFolderPath + "\\" + filename_prev);
                SetStausLabel("작업 미리보기 완료");
            }
        }

        // 상세정보
        private void SetInfo()
        {
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select D_REGTIME, V_RETOUCH from [CMSNS5].[dbo].[T_FLOWIMG] where id_fim = '{0}'", id)), "SELECT");            
                        
            string retouch = dt.Rows[0].ItemArray[1].ToString().Trim();

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

            SetStausLabel("전표 상세보기 완료");
        }        

        // 저장 버튼 클릭
        private void save_BTN_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("저장하시겠습니까?", "화상 작업", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (dialog == DialogResult.Yes)
            {
                Thread T = new Thread(new ThreadStart(FileSave));
                T.Start();
            }
        }

        private void FileSave()
        {
            SetStausLabel("파일 저장 중..");            
            
            if (FileUpload(Form1.realWorkFolderPath + "\\" + filename_real) == "S")
            {
                if (FileUpload(Form1.prevWorkFolderPath + "\\" + filename_prev) == "S")
                {
                    if (FileUpload(Form1.thumbWorkFolderPath + "\\" + filename_thumb) == "S")
                    {
                        if (Form1.photoshop.ActiveDocument.Name.Split('.')[1].ToLower() == "eps")
                        {
                            if (FileUpload(Form1.realWorkFolderPath + "\\" + filename_eps) == "S")
                            {
                                if (UpdateTable("save") == "S")
                                {
                                    SetStausLabel("파일 저장 완료");
                                                                        
                                    MessageBox.Show("저장되었습니다.", "화상 작업", MessageBoxButtons.OK, MessageBoxIcon.Information);    
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
                                SetStausLabel("파일 저장 완료");
                                                                
                                MessageBox.Show("저장되었습니다.", "화상 작업", MessageBoxButtons.OK, MessageBoxIcon.Information);                                
                                Util.SaveLog("File Save Complete: 파일 ID: " + id);
                                                                
                                form1.Search();
                                return;
                            }
                        }
                    }
                }
            }

            SetStausLabel("파일 저장 실패");
            MessageBox.Show("파일 저장 중 오류가 발생했습니다. IT개발부로 문의 바랍니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private string FileUpload(string filePath)
        {
            string result = "F";
            string kind = "";

            try
            {
                if (filePath.EndsWith(Form1.realExtMark) || filePath.EndsWith(Form1.epsExtMark))
                    kind = "Real";
                else if (filePath.EndsWith(Form1.prevExtMark))
                    kind = "Prev";
                else if (filePath.EndsWith(Form1.thumbExtMark))
                    kind = "Thumb";

                using (WebClient wc = new WebClient())
                {
                    byte[] resultArray = wc.UploadFile(Form1.uploadUrl + kind, filePath);
                    result = ((char)resultArray[0]).ToString();
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();

                if (result != "S")
                    Util.SaveLog("File Upload Fail: 파일 ID: " + id + ", action: " + kind);
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
            if (Form1.photoshop.ActiveDocument.Name.Split('.')[1].ToLower() != "eps")
            {
                MessageBox.Show("출고할 수 없는 파일 형식입니다.", "화상 작업", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                DialogResult dialog = MessageBox.Show("출고하시겠습니까?", "화상 작업", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (dialog == DialogResult.Yes)
                {
                    Thread T = new Thread(new ThreadStart(FileChulgo));
                    T.Start();
                }
            }
        }

        private void FileChulgo()
        {
            SetStausLabel("파일 출고 중..");

            if (FileUpload(Form1.realWorkFolderPath + "\\" + filename_eps) == "S")
            {
                if (FileUpload(Form1.realWorkFolderPath + "\\" + filename_real) == "S")
                {
                    if (FileUpload(Form1.prevWorkFolderPath + "\\" + filename_prev) == "S")
                    {
                        if (FileUpload(Form1.thumbWorkFolderPath + "\\" + filename_thumb) == "S")
                        {
                            if (UpdateTable("chulgo") == "S")
                            {
                                SetStausLabel("파일 출고 완료");

                                // WorkForm 목록 새로고침
                                MessageBox.Show("출고되었습니다.", "화상 작업", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Util.SaveLog("File Chulgo Complete: 파일 ID: " + id);

                                form1.Search();
                                return;
                            }
                        }
                    }
                }
            }                

            SetStausLabel("파일 출고 실패");
            MessageBox.Show("파일 출고 중 오류가 발생했습니다. IT개발부로 문의 바랍니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private string UpdateTable(string action)
        {
            string result = "F";
            string pubpart = "";
            string pan = "";
            string page = "";

            try
            {
                getPanPage(ref pubpart, ref pan, ref page);

                if (action == "open")
                {
                    // T_FLOWIMG 업데이트
                    dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [CMSNS5].[dbo].[T_FLOWIMG] set 
ID_LOCKERCODE = '{0}'
where ID_FIM = '{1}'", Form1.empCode, id)), "UPDATE");

                    // T_WORKHISTORY 인서트
                    dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"insert into [CMSNS5].[dbo].[T_WORKHISTORY] 
(ID_USERCODE, D_WORKDATE, V_CONTENT, N_WORKKIND, N_WORKCODE, N_PUBPART, ID_FID, N_CONTENT_TYPE, C_APP_TYPE) values 
('{0}', getdate(), '화상 ID : {1} ({2}면, {3}판, {4}부서코드)', '화상열기', '1001', '{5}', '{6}', '2', 'A')",
Form1.empCode, id, page, pan, pubpart, pubpart, id)), "INSERT");
                }
                else if (action == "close")
                {
                    // T_FLOWIMG 업데이트
                    dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [CMSNS5].[dbo].[T_FLOWIMG] set 
ID_LOCKERCODE = NULL
where ID_FIM = '{0}'", id)), "UPDATE");

                    // T_WORKHISTORY 인서트
                    dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"insert into [CMSNS5].[dbo].[T_WORKHISTORY] 
(ID_USERCODE, D_WORKDATE, V_CONTENT, N_WORKKIND, N_WORKCODE, N_PUBPART, ID_FID, N_CONTENT_TYPE, C_APP_TYPE) values 
('{0}', getdate(), '화상 ID : {1} ({2}면, {3}판, {4}부서코드)', '화상닫기', '1001', '{5}', '{6}', '2', 'A')",
Form1.empCode, id, page, pan, pubpart, pubpart, id)), "INSERT");
                }
                else
                {
                    string filePath = Form1.realWorkFolderPath + "\\" + filename_real;
                    string ext = Form1.photoshop.ActiveDocument.Name.Split('.')[1].ToLower();
                    long size = new FileInfo(filePath).Length;

                    Image img;
                    using (var bmpTemp = new Bitmap(filePath))
                    {
                        img = new Bitmap(bmpTemp);
                    }

                    string pixelX = img.Size.Width.ToString();
                    string pixelY = img.Size.Height.ToString();
                    string width = string.Format("{0:f2}", (double.Parse(pixelX) * 2.54 / img.HorizontalResolution)).Replace(".", "");
                    string height = string.Format("{0:f2}", (double.Parse(pixelY) * 2.54 / img.VerticalResolution)).Replace(".", "");
                    float dpi = img.HorizontalResolution;
                    
                    // T_FLOWIMG 업데이트
                    dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [CMSNS5].[dbo].[T_FLOWIMG] set 
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
where ID_FIM = '{8}'", dpi, width, height, ext, pixelX, pixelY, size, Form1.empCode, id)), "UPDATE");

                    if (action == "save")
                    {
                        // T_WORKHISTORY 인서트
                        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"insert into [CMSNS5].[dbo].[T_WORKHISTORY] 
(ID_USERCODE, D_WORKDATE, V_CONTENT, N_WORKKIND, N_WORKCODE, N_PUBPART, ID_FID, N_CONTENT_TYPE, C_APP_TYPE) values 
('{0}', getdate(), '화상 ID : {1} ({2}면, {3}판, {4}부서코드)', '화상수정', '1003', '{5}', '{6}', '2', 'A')",
Form1.empCode, id, page, pan, pubpart, pubpart, id)), "INSERT");
                    }
                    else if (action == "chulgo")
                    {
                        // T_FLOWIMG 업데이트
                        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [CMSNS5].[dbo].[T_FLOWIMG] set 
D_PUBDATE = getdate(), 
NM_FLOWSTATE = '321',
NM_BEFORESTATE = '256',
V_BEFORESTATE = '제작출고',
N_PUBLISHCNT = N_PUBLISHCNT + 1 
where ID_FIM = '{0}'", id)), "UPDATE");

                        // T_IMAGE 인서트
                        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"insert into [CMSNS5].[dbo].[T_IMAGE] 
(ID_IM, ID_MECHAE, ID_PUBPART, D_REGTIME, D_PUBDATE, N_DPI, N_WIDTH, N_HEIGHT, C_EXTENSION, N_PIXELX, N_PIXELY, N_FILESIZE, ID_USERCODE, NM_FLOWSTATE, V_BEFORESTATE, 
D_BEFOREPUBDATE, V_TITLE, V_AUTHOR, N_PAN, N_PAGE, N_HO, N_COLOR, V_CAPTION, N_PHOTOFROM, V_PHOTOMAN, V_PHOTOREGION, D_PHOTODATE, V_PHOTOMEMO, V_OFILENAME, N_FROM, V_RETOUCH, N_TICKETFLAG, ID_FIM) values 
((select max(ID_IM) from [CMSNS5].[dbo].[T_IMAGE]) + 1, '65', '29', getdate(), getdate(), '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '256', '제작출고',
(select top 1 D_BEFOREPUBDATE from [CMSNS5].[dbo].[T_IMAGE] where ID_FIM = '{8}' order by ID_IM desc), 
(select top 1 V_TITLE from [CMSNS5].[dbo].[T_IMAGE] where ID_FIM = '{8}' order by ID_IM desc), 
(select top 1 V_AUTHOR from [CMSNS5].[dbo].[T_IMAGE] where ID_FIM = '{8}' order by ID_IM desc), 
(select top 1 N_PAN from [CMSNS5].[dbo].[T_IMAGE] where ID_FIM = '{8}' order by ID_IM desc), 
(select top 1 N_PAGE from [CMSNS5].[dbo].[T_IMAGE] where ID_FIM = '{8}' order by ID_IM desc), 
(select top 1 N_HO from [CMSNS5].[dbo].[T_IMAGE] where ID_FIM = '{8}' order by ID_IM desc), 
(select top 1 N_COLOR from [CMSNS5].[dbo].[T_IMAGE] where ID_FIM = '{8}' order by ID_IM desc), 
(select top 1 V_CAPTION from [CMSNS5].[dbo].[T_IMAGE] where ID_FIM = '{8}' order by ID_IM desc), 
(select top 1 N_PHOTOFROM from [CMSNS5].[dbo].[T_IMAGE] where ID_FIM = '{8}' order by ID_IM desc), 
(select top 1 V_PHOTOMAN from [CMSNS5].[dbo].[T_IMAGE] where ID_FIM = '{8}' order by ID_IM desc), 
(select top 1 V_PHOTOREGION from [CMSNS5].[dbo].[T_IMAGE] where ID_FIM = '{8}' order by ID_IM desc), 
(select top 1 D_PHOTODATE from [CMSNS5].[dbo].[T_IMAGE] where ID_FIM = '{8}' order by ID_IM desc), 
(select top 1 V_PHOTOMEMO from [CMSNS5].[dbo].[T_IMAGE] where ID_FIM = '{8}' order by ID_IM desc), 
(select top 1 V_OFILENAME from [CMSNS5].[dbo].[T_IMAGE] where ID_FIM = '{8}' order by ID_IM desc), 
(select top 1 N_FROM from [CMSNS5].[dbo].[T_IMAGE] where ID_FIM = '{8}' order by ID_IM desc), 
(select top 1 V_RETOUCH from [CMSNS5].[dbo].[T_IMAGE] where ID_FIM = '{8}' order by ID_IM desc), 
(select top 1 N_TICKETFLAG from [CMSNS5].[dbo].[T_IMAGE] where ID_FIM = '{8}' order by ID_IM desc), 
(select top 1 ID_FIM from [CMSNS5].[dbo].[T_IMAGE] where ID_FIM = '{8}' order by ID_IM desc))",
dpi, width, height, ext, pixelX, pixelY, size, Form1.empCode, id)), "INSERT");

                        // T_WORKHISTORY 인서트
                        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"insert into [CMSNS5].[dbo].[T_WORKHISTORY] 
(ID_USERCODE, D_WORKDATE, V_CONTENT, N_WORKKIND, N_WORKCODE, N_PUBPART, ID_FID, N_CONTENT_TYPE, C_APP_TYPE) values 
('{0}', getdate(), '화상 ID : {1} ({2}면, {3}판, {4}부서코드)', '화상출고', '1001', '{5}', '{6}', '2', 'A')",
Form1.empCode, id, page, pan, pubpart, pubpart, id)), "INSERT");
                    }

                    img = null;
                }

                result = "S";
            }
            catch (Exception ex)
            {                
                Util.SaveLog("DB Update Fail: 파일 ID: " + id + ", action: " + action + "\n" + ex);
            }

            return result;
        }

        private void getPanPage(ref string pubpart, ref string pan, ref string page)
        {
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select ID_PUBPART, N_PAN, N_PAGE from [CMSNS5].[dbo].[T_FLOWIMG] where ID_FIM = '{0}'", id)), "SELECT");
                
            pubpart = dt.Rows[0]["ID_PUBPART"].ToString();
            pan = dt.Rows[0]["N_PAN"].ToString();
            page = dt.Rows[0]["N_PAGE"].ToString();
        }

        private Bitmap LoadBitmap(string path)
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

            return null;
        }
        
        private void fsw1_Renamed(object sender, RenamedEventArgs e)
        {
            fsw1_Event(e);
        }

        private void fsw1_Event(FileSystemEventArgs e)
        {
            if (e.Name == filename_real)
            {
                imgPreView1.Image = null;

                if (File.Exists(Form1.originWorkFolderPath + "\\" + filename_real))
                {
                    imgPreView1.Image = LoadBitmap(Form1.originWorkFolderPath + "\\" + filename_real);
                    SetStausLabel("전표 미리보기 완료");
                }
            }
        }
        
        private void fsw2_Renamed(object sender, RenamedEventArgs e)
        {
            fsw2_Event(e);
        }

        private void fsw2_Event(FileSystemEventArgs e)
        {
            if (e.Name == filename_prev)
            {
                imgPreView2.Image = null;

                if (File.Exists(Form1.prevWorkFolderPath + "\\" + filename_prev))
                {
                    imgPreView2.Image = LoadBitmap(Form1.prevWorkFolderPath + "\\" + filename_prev);
                    SetStausLabel("작업 미리보기 완료");
                }
            }
        }

        private void SetStausLabel(string msg)
        {
            toolStripStatusLabel.Text = DateTime.Now.ToString("(HH:mm:ss) ") + msg;
        }

        private void WorkForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateTable("close");

            imgPreView1.Image = null;
            imgPreView2.Image = null;
            imgPreView1.Dispose();
            imgPreView2.Dispose();
        }

        // 닫기 버튼 클릭
        private void close_BTN_Click(object sender, EventArgs e)
        {
            Close();
            form1.Search();
        }
    }
}
