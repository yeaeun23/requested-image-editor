using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Linq;
using System.Diagnostics;

namespace ImageWork
{
    public partial class Form1 : Form
    {
        DataTable dt;
        public static StringBuilder sb = new StringBuilder(255);
        public static Photoshop.Application photoshop = null;

        string conStr = @"";
        string iconPath_caption = "";
        string iconPath_chulgo_no = "";
        string iconPath_chulgo_yes = "";
        string iconPath_photoshop = "";

        string sortAscMark = "▲";
        string sortDescMark = "▼";

        string MYUN = "전체";
        bool isMyunTotalChecked;    // 면 전체 선택 여부
        int imgPerPage = 50;        // 페이지 당 이미지 개수
        int imgTotalCnt = 0;        // 전체 이미지 개수
        int imgSelectedIdx = -1;    // 선택된 이미지 idx             

        public static string empNo = "1234567";
        public static string empName = "테스트";
        public static string empCode = "123";

        public static string epsExtMark = "_R_0.eps";
        public static string realExtMark = "_R_0.jpg";
        public static string prevExtMark = "_P_0.jpg";
        public static string thumbExtMark = "_T_0.jpg";

        public static string iniFilePath = Application.StartupPath + "\\ImageWork.ini";

        public static string downloadUrl = GetValue("URL", "DOWNLOAD");
        public static string uploadUrl = GetValue("URL", "UPLOAD");
        public static string printUrl = GetValue("URL", "PRINT");

        public static string originWorkFolderPath = GetValue("FOLDER_PATH", "ORIGIN_WORK");
        public static string realWorkFolderPath = GetValue("FOLDER_PATH", "REAL_WORK");
        public static string prevWorkFolderPath = GetValue("FOLDER_PATH", "PREV_WORK");
        public static string thumbWorkFolderPath = GetValue("FOLDER_PATH", "THUMB_WORK");

        public static string realDownloadFolderPath = GetValue("FOLDER_PATH", "REAL_DOWNLOAD");
        public static string prevDownloadFolderPath = GetValue("FOLDER_PATH", "PREV_DOWNLOAD");
        public static string thumbDownloadFolderPath = GetValue("FOLDER_PATH", "THUMB_DOWNLOAD");

        public static int IDX_OFILENAME = 0;      // 파일명
        public static int IDX_TITLE = 1;          // 제목
        public static int IDX_AUTHOR = 2;         // 작성자
        public static int IDX_PUBDATE = 3;        // 출고일
        public static int IDX_BEFOREPUBDATE = 4;  // 게재일
        public static int IDX_REGTIME = 5;        // 등록일
        public static int IDX_PAN = 6;            // 판
        public static int IDX_PAGE = 7;           // 면
        public static int IDX_BEFORESTATE = 8;    // 상태
        public static int IDX_ID = 9;             // 아이디
        public static int IDX_CAPTION = 10;       // 내용
        public static int IDX_RETOUCH = 11;       // 요청사항

        public static int IDX_THEME_BG = 0;    // 배경
        public static int IDX_THEME_ICON = 0;  // 아이콘        

        // ini 파일 쓰기
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        // ini 파일 읽기
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        // 창 활성화
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        // 창 최상위로
        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hWnd);

        public static void WriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, iniFilePath);
        }

        public static string GetValue(string Section, string Key)
        {
            GetPrivateProfileString(Section, Key, string.Empty, sb, 255, Application.StartupPath + "\\ImageWork.ini");
            return sb.ToString();
        }

        public Form1()
        {
            //Delay(2000);

            // 열려있는 앱 확인
            Process[] processList = Process.GetProcessesByName("ImageWork");
            if (processList.Length == 2)
            {
                if (processList[0].StartTime > processList[1].StartTime)
                {
                    ShowWindowAsync(processList[1].MainWindowHandle, 1);
                    SetForegroundWindow(processList[1].MainWindowHandle);
                    processList[0].Kill();
                }
                else
                {
                    ShowWindowAsync(processList[0].MainWindowHandle, 1);
                    SetForegroundWindow(processList[0].MainWindowHandle);
                    processList[1].Kill();
                }
            }
            else
            {
                // 버전 체크
                //VersionCheck();

                //if (srcVer != destVer)  // 버전이 다르면
                //{
                //    // 업데이트
                //    Process.Start(destDirName + @"\UpdateImageWork.exe");
                //    Process.GetCurrentProcess().Kill();
                //}
                //else                    // 버전이 같으면
                {
                    // 로그인
                    LoginForm loginForm = new LoginForm();

                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        Util.SaveLog("Login: " + empNo);
                        InitializeComponent();
                    }
                    else
                    {
                        Process.GetCurrentProcess().Kill();
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 메뉴 배경
            //toolStrip.Renderer = new MyRenderer();

            // 상태 표시줄
            toolStripStatusLabel1.Text = empName + "(" + empNo + ")님이 사용중입니다.";
            toolStripStatusLabel2.Text = "Ver " + GetValue("VERSION", "VER");

            // Form 크기
            if (GetValue("SIZE", "FORM_WIDTH") != "")
                Width = Convert.ToInt32(GetValue("SIZE", "FORM_WIDTH"));

            if (GetValue("SIZE", "FORM_HEIGHT") != "")
                Height = Convert.ToInt32(GetValue("SIZE", "FORM_HEIGHT"));

            // Form 크기보다 스크린이 작을 경우
            if (Screen.PrimaryScreen.Bounds.Width < Size.Width || Screen.PrimaryScreen.Bounds.Height < Size.Height)
                WindowState = FormWindowState.Maximized;    // 최대화            

            // 이미지 크기
            if (GetValue("SIZE", "LV_IMG") != "")
            {
                int size = Convert.ToInt32(GetValue("SIZE", "LV_IMG"));

                if (size < size_BAR.Minimum || size > size_BAR.Maximum)
                    size = 150;

                size_BAR.Value = size;
            }

            // 글꼴
            if (GetValue("FONT", "FONT") != "")
                fontDialog.Font = (Font)TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(GetValue("FONT", "FONT"));

            if (GetValue("FONT", "COLOR") != "")
                fontDialog.Color = (Color)TypeDescriptor.GetConverter(typeof(Color)).ConvertFromString(GetValue("FONT", "COLOR"));

            SetFont(sender, e);

            // 이미지 배경
            if (GetValue("THEME", "IDX_BG") != "")
                IDX_THEME_BG = Convert.ToInt32(GetValue("THEME", "IDX_BG"));

            // 아이콘
            if (GetValue("THEME", "IDX_ICON") != "")
                IDX_THEME_ICON = Convert.ToInt32(GetValue("THEME", "IDX_ICON"));

            SetIconPath();

            // 폴더
            if (!Directory.Exists(originWorkFolderPath))
                Directory.CreateDirectory(originWorkFolderPath);

            if (!Directory.Exists(realWorkFolderPath))
                Directory.CreateDirectory(realWorkFolderPath);

            if (!Directory.Exists(prevWorkFolderPath))
                Directory.CreateDirectory(prevWorkFolderPath);

            if (!Directory.Exists(thumbWorkFolderPath))
                Directory.CreateDirectory(thumbWorkFolderPath);

            // 스플리터     
            if (GetValue("SPLITTER", "FORM_V") != "")
                splitContainer2.SplitterDistance = Convert.ToInt32(GetValue("SPLITTER", "FORM_V"));

            if (GetValue("SPLITTER", "FORM_H1") != "")
                splitContainer3.SplitterDistance = Convert.ToInt32(GetValue("SPLITTER", "FORM_H1"));

            if (GetValue("SPLITTER", "FORM_H2") != "")
                splitContainer4.SplitterDistance = Convert.ToInt32(GetValue("SPLITTER", "FORM_H2"));

            if (GetValue("SPLITTER", "COL_HEADER") != "")
                imgGridView.ColumnHeadersHeight = Convert.ToInt32(GetValue("SPLITTER", "COL_HEADER"));

            for (int i = 0; i < imgGridView.Columns.Count; i++)
            {
                if (GetValue("SPLITTER", "COL" + i) != "")
                    imgGridView.Columns[i].Width = Convert.ToInt32(GetValue("SPLITTER", "COL" + i));
            }

            // 컨트롤 로드
            SetMediaCBList();
            SetPanCBList();
            SetMyunCBList();

            // 검색 초기화
            Init();
        }

        private void SetIconPath()
        {
            if (IDX_THEME_ICON == 1 || IDX_THEME_ICON == 2)
            {
                iconPath_caption = Application.StartupPath + "\\" + GetValue("ICON_PATH" + IDX_THEME_ICON, "RETOUCH");
                iconPath_chulgo_no = Application.StartupPath + "\\" + GetValue("ICON_PATH" + IDX_THEME_ICON, "CHULGO_NO");
                iconPath_chulgo_yes = Application.StartupPath + "\\" + GetValue("ICON_PATH" + IDX_THEME_ICON, "CHULGO_YES");
                iconPath_photoshop = Application.StartupPath + "\\" + GetValue("ICON_PATH" + IDX_THEME_ICON, "PHOTOSHOP");
            }
            else
            {
                iconPath_caption = Application.StartupPath + "\\" + GetValue("ICON_PATH0", "RETOUCH");
                iconPath_chulgo_no = Application.StartupPath + "\\" + GetValue("ICON_PATH0", "CHULGO_NO");
                iconPath_chulgo_yes = Application.StartupPath + "\\" + GetValue("ICON_PATH0", "CHULGO_YES");
                iconPath_photoshop = Application.StartupPath + "\\" + GetValue("ICON_PATH0", "PHOTOSHOP");
            }

            if (!(File.Exists(iconPath_caption) && File.Exists(iconPath_chulgo_no) && File.Exists(iconPath_chulgo_yes) && File.Exists(iconPath_photoshop)))
            {
                MessageBox.Show("아이콘 파일을 확인해 주세요.", "아이콘 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void Init()
        {
            // 컨트롤 값 초기화
            media_CB.SelectedIndex = 1;
            date_CAL1.Value = DateTime.Now.AddDays(1);
            date_CAL2.Value = DateTime.Now.AddDays(1);
            pan_CB.SelectedIndex = 0;

            myun_TB.Text = "전체";
            myun_TV.Nodes[0].Checked = true;
            myun_TV.Hide();

            state_CB.SelectedIndex = 0;

            Search();
        }

        // 검색
        public void Search()
        {
            InitPreview();
            SetPageCBList();

            if (bgWorker.IsBusy)
            {
                bgWorker.Abort();
                bgWorker.Dispose();

                bgWorker = new AbortableBackgroundWorker();
                bgWorker.DoWork += bgWorker_DoWork;
            }

            string sql = GetSearchSql();
            SetListView(sql);
            SetGridView(sql);

            // 마지막 선택 이미지
            if (imgSelectedIdx > -1 && imgSelectedIdx < imgListView.Items.Count)
            {
                // 리스트뷰 선택 & 스크롤
                imgListView.Items[imgSelectedIdx].Selected = true;
                imgListView.Items[imgSelectedIdx].Focused = true;
                imgListView.EnsureVisible(imgSelectedIdx);

                // 그리드뷰 선택 & 스크롤
                imgGridView.Rows[imgSelectedIdx].Selected = true;
                imgGridView.CurrentCell = imgGridView.Rows[imgSelectedIdx].Cells[0];

                // 프리뷰
                ShowPreview();
            }
        }

        private string GetSortSql()
        {
            //int index = 0;
            //string order = "";

            //for (int i = 0; i < imgGridView.Columns.Count; i++)
            //{
            //    if (imgGridView.Columns[i].HeaderText.Contains(sortAscMark))
            //    {
            //        index = i;
            //        order = "asc";
            //        break;
            //    }
            //    else if (imgGridView.Columns[i].HeaderText.Contains(sortDescMark))
            //    {
            //        index = i;
            //        order = "desc";
            //        break;
            //    }
            //}

            //switch (index)
            //{
            //    case 0: // 파일명
            //        return "V_OFILENAME " + order;
            //    case 1: // 제목
            //        return "V_TITLE " + order;
            //    case 2: // 작성자
            //        return "V_AUTHOR " + order;
            //    case 3: // 출고일
            //        return "D_PUBDATE " + order;
            //    case 4: // 게재일
            //        return "D_BEFOREPUBDATE " + order;
            //    case 5: // 등록일
            //        return "D_REGTIME " + order;
            //    case 6: // 판
            //        return "N_PAN " + order;
            //    case 7: // 면
            //        return "N_PAGE " + order;
            //    case 8: // 상태
            //        return "V_BEFORESTATE " + order;
            //    case 9: // ID
            //        return "ID_FIM " + order;
            //    default:
            //        return "V_OFILENAME " + order;
            //}

            return "V_OFILENAME ASC";
        }

        // 검색 쿼리 생성
        private string GetSearchSql()
        {
            string sql = "select top " + imgPerPage + " * from (select row_number() over (";

            sql += "order by " + GetSortSql();
            sql += ") as rownum, * from [CMSNS5].[dbo].[T_FLOWIMG] where ";

            if (media_CB.SelectedIndex > 0)
                sql += "ID_MECHAE = '" + media_CB.Text.Split('-')[1] + "' and ";

            sql += "(cast(" + (date_RB1.Checked ? "D_BEFOREPUBDATE" : "D_REGTIME") + " as date) between '";
            sql += date_CAL1.Value.ToString().Substring(0, 10).Replace("-", "") + "' and '";
            sql += date_CAL2.Value.ToString().Substring(0, 10).Replace("-", "") + "') ";

            if (pan_CB.SelectedIndex > 0)
                sql += "and N_PAN = '" + pan_CB.Text + "' ";

            sql += "and (" + GetMyunSql() + ") ";

            if (state_CB.Text == "출고")
                sql += "and (NM_FLOWSTATE & 256 = 256 and (NM_FLOWSTATE & 1 = 1 or ID_PUBPART = 29)) and B_DELETE = 0";
            else if (state_CB.Text == "미출고")
                sql += "and (NM_FLOWSTATE & 256 = 0 and (NM_FLOWSTATE & 1 = 1 or ID_PUBPART = 29)) and B_DELETE = 0";
            else if (state_CB.Text == "휴지통")
                sql += "and (NM_FLOWSTATE & 1 = 1 or ID_PUBPART = 29) and B_DELETE = 1";
            else
                sql += "and (NM_FLOWSTATE & 1 = 1 or ID_PUBPART = 29) and B_DELETE = 0";

            int pageNow = Convert.ToInt32(page_CB.Text);
            int pageTotal = page_CB.Items.Count;

            sql += ") t1 where t1.rownum < " + (imgTotalCnt - (imgPerPage * (pageTotal - pageNow)) + 1);
            sql += " and t1.rownum >= " + (imgTotalCnt - (imgPerPage * (pageTotal + 1 - pageNow)) + 1);

            string data = "";
            SqlConnection db = null;

            try
            {
                Cursor = Cursors.WaitCursor;

                imgListView.Items.Clear();
                imgList.Images.Clear();

                db = new SqlConnection(conStr);
                SqlCommand dbCmd = new SqlCommand(sql, db);
                db.Open();

                SqlDataReader reader = dbCmd.ExecuteReader();

                while (reader.Read())
                {
                    data += reader["V_OFILENAME"].ToString().Trim() + "∥"
                        + reader["V_TITLE"].ToString().Trim() + "∥"
                        + reader["V_AUTHOR"].ToString().Trim() + "∥"
                        + reader["D_PUBDATE"].ToString().Trim() + "∥"
                        + reader["D_BEFOREPUBDATE"].ToString().Trim().Substring(0, 10) + "∥"
                        + reader["D_REGTIME"].ToString().Trim() + "∥"
                        + reader["N_PAN"].ToString().Trim() + "∥"
                        + reader["N_PAGE"].ToString().Trim() + "∥"
                        + reader["V_BEFORESTATE"].ToString().Trim() + "∥"
                        + reader["ID_FIM"].ToString().Trim() + "∥"
                        + reader["V_CAPTION"].ToString().Trim() + "∥"
                        + reader["V_RETOUCH"].ToString().Trim() + "|";
                }

                Cursor = Cursors.Default;

                reader.Close();
                db.Close();

                Util.SaveLog(sql);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                db.Close();
                MessageBox.Show(ex.ToString(), "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return data;
        }

        // 검색 쿼리 생성 - 면
        private string GetMyunSql()
        {
            string sql = "";

            if (myun_TB.Text.Trim() == "전체" || myun_TB.Text.Trim() == "")
            {
                for (int i = 1; i < 100; i++)
                {
                    sql += "N_PAGE = " + i;

                    if (i != 99)
                        sql += " or ";
                }
            }
            else
            {
                string[] arr = MYUN.Trim().Split(new char[] { ' ' });

                for (int i = 0; i < arr.Length; i++)
                {
                    sql += "N_PAGE = " + arr[i];

                    if (i != arr.Length - 1)
                        sql += " or ";
                }
            }

            return sql;
        }

        // 리스트뷰 값 세팅
        public void SetListView(string str)
        {
            string[] article = str.Split('|');
            string[] columns = null;
            ListViewItem item = null;

            try
            {
                imgListView.BeginUpdate();

                for (int i = 0; i < article.Length - 1; i++)
                {
                    columns = article[i].Split('∥');

                    item = new ListViewItem(columns[IDX_OFILENAME]);
                    item.SubItems.Add(columns[IDX_TITLE]);
                    item.SubItems.Add(columns[IDX_AUTHOR]);
                    item.SubItems.Add(columns[IDX_PUBDATE]);
                    item.SubItems.Add(columns[IDX_BEFOREPUBDATE]);
                    item.SubItems.Add(columns[IDX_REGTIME]);
                    item.SubItems.Add(columns[IDX_PAN]);
                    item.SubItems.Add(columns[IDX_PAGE]);
                    item.SubItems.Add(columns[IDX_BEFORESTATE]);
                    item.SubItems.Add(columns[IDX_ID]);
                    item.SubItems.Add(columns[IDX_CAPTION]);
                    item.SubItems.Add(columns[IDX_RETOUCH]);

                    item.ToolTipText = item.SubItems[IDX_OFILENAME].Text;

                    imgListView.Items.Add(item);
                }

                imgListView.EndUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ViewTypeCheck();
        }

        public void SetGridView(string str)
        {
            imgGridView.Rows.Clear();

            string[] strArr1 = str.Split('|');
            string[] strArr2 = null;

            for (int i = 0; i < strArr1.Length - 1; i++)
            {
                strArr2 = strArr1[i].Split('∥');
                imgGridView.Rows.Insert(i, strArr2);
            }

            imgGridView.ClearSelection();
        }

        // 리스트뷰 보기 타입에 따른 설정
        private void ViewTypeCheck()
        {
            Cursor = Cursors.WaitCursor;

            bgWorker.RunWorkerAsync(); // 이미지 하나씩 로드                              

            Cursor = Cursors.Default;
        }

        // 이미지 하나씩 추가
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            for (int i = 0; i < imgListView.Items.Count; i++)
            {
                ListViewItem item = imgListView.Items[i];
                string date = item.SubItems[IDX_REGTIME].Text;
                string id = item.SubItems[IDX_ID].Text;
                string fileName = id + thumbExtMark;

                try
                {
                    imgList.Images.Add(ViewWebImage(string.Format("{0}/THUMB/{1}/{2}/{3}/{4}", downloadUrl, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), fileName), id));
                }
                catch (ArgumentNullException ex)
                {
                    Util.SaveLog("Image Download Fail(Thumb): " + ex);
                    continue;
                }

                imgListView.Items[i].ImageIndex = i;
            }
        }

        private void getIconFlag(ref bool flag_caption, ref bool flag_chulgo, ref bool flag_photoshop, string id)
        {
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select N_TICKETFLAG, ID_LOCKERCODE, NM_FLOWSTATE, ID_PUBPART from [CMSNS5].[dbo].[T_FLOWIMG] where ID_FIM = '{0}'", id)), "SELECT");

            if (dt.Rows[0]["N_TICKETFLAG"].ToString() == "1")
                flag_caption = true;

            if ((Convert.ToInt32(dt.Rows[0]["NM_FLOWSTATE"]) & 256) != 256)
                flag_chulgo = false;
            else
                flag_chulgo = ((Convert.ToInt32(dt.Rows[0]["NM_FLOWSTATE"]) & 1) == 1 ? true : dt.Rows[0]["ID_PUBPART"].ToString() == "29");

            if (!(dt.Rows[0]["ID_LOCKERCODE"].ToString() == "" || dt.Rows[0]["ID_LOCKERCODE"].ToString() == "0"))
                flag_photoshop = true;
        }

        // 웹 이미지 다운로드
        private Bitmap ViewWebImage(string url, string id)
        {
            try
            {
                WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(url);
                Bitmap downloadImg = Bitmap.FromStream(stream) as Bitmap;

                if (url.Contains(prevExtMark))
                {
                    return downloadImg;
                }
                else
                {
                    float imgWidth = downloadImg.Width;
                    float imgHeight = downloadImg.Height;
                    float imgRate = 0f;

                    float single = 0f;
                    float thumbWidth = 0f;
                    float thumbHeight = 0f;
                    float thumbMarginWidth = 0;
                    float thumbMarginHeight = 30;
                    float thumbUserSize = size_BAR.Value;

                    float iconPosX;
                    float iconPosY;

                    if (imgWidth <= imgHeight)
                    {
                        imgRate = imgWidth / imgHeight;
                        single = thumbUserSize - thumbMarginHeight;
                        thumbWidth = single * imgRate;
                        thumbHeight = single;
                        iconPosX = (single - thumbWidth) / 2f + (thumbMarginHeight / 2);
                        iconPosY = 0f;
                    }
                    else
                    {
                        imgRate = imgHeight / imgWidth;
                        single = thumbUserSize - thumbMarginWidth;
                        thumbWidth = single;
                        thumbHeight = single * imgRate;
                        iconPosX = thumbMarginWidth / 2;
                        iconPosY = (single - thumbHeight) / 2f;
                    }

                    Bitmap bitmap1 = new Bitmap((int)Math.Round(thumbUserSize, 0), (int)Math.Round(thumbUserSize, 0));
                    Bitmap bitmap2 = new Bitmap(downloadImg, (int)Math.Round(thumbWidth, 0), (int)Math.Round(thumbHeight, 0));

                    Graphics graphic = Graphics.FromImage(bitmap1);

                    // 배경 설정
                    if (IDX_THEME_BG == 1)
                        graphic.Clear(Color.Black);
                    else if (IDX_THEME_BG == 2)
                        graphic.Clear(Color.FromArgb(40, 40, 40));
                    else
                        graphic.Clear(Color.White);

                    graphic.InterpolationMode = InterpolationMode.Bicubic;
                    graphic.DrawImage(bitmap2, iconPosX, iconPosY, thumbWidth, thumbHeight);

                    // 아이콘 설정
                    bool flag_caption = false;
                    bool flag_chulgo = false;
                    bool flag_photoshop = false;

                    getIconFlag(ref flag_caption, ref flag_chulgo, ref flag_photoshop, id);

                    if (flag_caption)
                        graphic.DrawImage(Image.FromFile(iconPath_caption), 0, thumbUserSize - 20, 45, 20);

                    if (flag_chulgo)
                        graphic.DrawImage(Image.FromFile(iconPath_chulgo_yes), thumbUserSize - 45, thumbUserSize - 20, 45, 20);
                    else if (flag_photoshop)
                        graphic.DrawImage(Image.FromFile(iconPath_photoshop), thumbUserSize - 45, thumbUserSize - 20, 45, 20);
                    else
                        graphic.DrawImage(Image.FromFile(iconPath_chulgo_no), thumbUserSize - 45, thumbUserSize - 20, 45, 20);

                    bitmap2.Dispose();
                    graphic.Dispose();
                    downloadImg.Dispose();

                    return bitmap1;
                }
            }
            catch (Exception ex)
            {
                Util.SaveLog("Thumb Image Load Fail: 파일 ID: " + id + "\n" + ex);
                return null;
            }
        }

        // 매체 CB 리스트 세팅
        public void SetMediaCBList()
        {
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select id_mechae, name from [DAPS].[dbo].[CMS_MECHAE] order by id_mechae")), "SELECT");

            foreach (DataRow dr in dt.Rows)
                media_CB.Items.Add(dr["name"].ToString() + "-" + dr["id_mechae"].ToString()); // "서울신문-65"              
        }

        // 판 CB 리스트 세팅
        public void SetPanCBList()
        {
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select v_pan_name from [DAPS].[dbo].[CMS_PAN] where id_mechae = 65 order by n_pan_code")), "SELECT");

            foreach (DataRow dr in dt.Rows)
                pan_CB.Items.Add(dr["v_pan_name"].ToString().Replace("판", "")); // "5"
        }

        // 면 CB 리스트 세팅
        public void SetMyunCBList()
        {
            TreeNode svrNode = new TreeNode("전체");

            // 1 ~ 99면까지 추가
            for (int i = 1; i < 100; i++)
                svrNode.Nodes.Add(i.ToString(), i.ToString()); // "1"

            myun_TV.Nodes.Add(svrNode);
            myun_TV.ExpandAll();
        }

        // 페이지 CB 리스트 세팅
        public void SetPageCBList()
        {
            page_CB.Items.Clear();

            // 전체 이미지 개수 구하기
            string sql = "select count(*) as cnt from [CMSNS5].[dbo].[T_FLOWIMG] where ";

            if (media_CB.SelectedIndex > 0)
                sql += "ID_MECHAE = '" + media_CB.Text.Split('-')[1] + "' and ";

            sql += "(cast(" + (date_RB1.Checked ? "D_BEFOREPUBDATE" : "D_REGTIME") + " as date) between '";
            sql += date_CAL1.Value.ToString().Substring(0, 10).Replace("-", "") + "' and '";
            sql += date_CAL2.Value.ToString().Substring(0, 10).Replace("-", "") + "') ";

            if (pan_CB.SelectedIndex > 0)
                sql += "and N_PAN = '" + pan_CB.Text + "' ";

            sql += "and (" + GetMyunSql() + ") ";

            if (state_CB.Text == "출고")
                sql += "and (NM_FLOWSTATE & 256 = 256 and (NM_FLOWSTATE & 1 = 1 or ID_PUBPART = 29)) and B_DELETE = 0";
            else if (state_CB.Text == "미출고")
                sql += "and (NM_FLOWSTATE & 256 = 0 and (NM_FLOWSTATE & 1 = 1 or ID_PUBPART = 29)) and B_DELETE = 0";
            else if (state_CB.Text == "휴지통")
                sql += "and (NM_FLOWSTATE & 1 = 1 or ID_PUBPART = 29) and B_DELETE = 1";
            else
                sql += "and (NM_FLOWSTATE & 1 = 1 or ID_PUBPART = 29) and B_DELETE = 0";

            dt = Util.ExecuteQuery(new SqlCommand(sql), "SELECT");

            imgTotalCnt = Convert.ToInt32(dt.Rows[0].ItemArray[0]);

            // CB 추가
            for (int i = (imgTotalCnt / imgPerPage) + 1; i > 0; i--)
                page_CB.Items.Add(i);

            page_CB.SelectedIndex = 0;
        }

        private void OpenFile()
        {
            ListViewItem item = imgListView.SelectedItems[0];
            string date = item.SubItems[IDX_REGTIME].Text;
            string id = item.SubItems[IDX_ID].Text;

            bool flag_chulgo = false;
            bool flag_photoshop = false;

            try
            {
                // 출고 및 작업중 확인 
                dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select ID_LOCKERCODE, NM_FLOWSTATE, ID_PUBPART from [CMSNS5].[dbo].[T_FLOWIMG] where ID_FIM = '{0}'", id)), "SELECT");

                if ((Convert.ToInt32(dt.Rows[0]["NM_FLOWSTATE"]) & 256) != 256)
                    flag_chulgo = false;
                else
                    flag_chulgo = ((Convert.ToInt32(dt.Rows[0]["NM_FLOWSTATE"]) & 1) == 1 ? true : dt.Rows[0]["ID_PUBPART"].ToString() == "29");

                if (!(dt.Rows[0]["ID_LOCKERCODE"].ToString() == "" || dt.Rows[0]["ID_LOCKERCODE"].ToString() == "0"))
                    flag_photoshop = true;

                if (flag_chulgo)
                {
                    Search();
                    MessageBox.Show("이미 출고되었습니다.", "파일 열기", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (flag_photoshop)
                {
                    Search();
                    MessageBox.Show("이미 작업중입니다.", "파일 열기", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    // 작업창 앞으로
                    Form[] workFormList = Application.OpenForms.Cast<Form>().Where(x => x.Name == "WorkForm").ToArray();

                    if (workFormList.Length > 0)
                    {
                        workFormList[0].Activate();
                        workFormList[0].WindowState = FormWindowState.Normal;
                    }
                }
                else
                {
                    // 포토샵 앞으로
                    Process[] processList = Process.GetProcessesByName("Photoshop");

                    if (processList.Length > 0)
                    {
                        ShowWindowAsync(processList[0].MainWindowHandle, 1);
                        SetForegroundWindow(processList[0].MainWindowHandle);
                    }

                    // 포토샵 실행
                    Type type = Type.GetTypeFromProgID("Photoshop.Application");
                    photoshop = (Photoshop.Application)Activator.CreateInstance(type, true);

                    if (processList.Length == 0)
                        return;

                    // 마지막 작업 파일이 EPS 확장자인지 아닌지
                    bool isEps = false;
                    dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select top 1 C_EXTENSION from [CMSNS5].[dbo].[T_FLOWIMG] where ID_FIM = '{0}' order by D_PUBDATE desc", id)), "SELECT");

                    if (dt.Rows[0].ItemArray[0].ToString().Trim().ToLower() == "eps")
                        isEps = true;

                    // 이미지 다운로드
                    if (isEps)
                    {
                        // Real 파일 → Origin 폴더 (eps)
                        using (WebClient wc = new WebClient())
                        {
                            wc.DownloadFileAsync(new Uri(string.Format("{0}/REAL/{1}/{2}/{3}/{4}", downloadUrl, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), id + epsExtMark)), originWorkFolderPath + "\\" + id + epsExtMark);
                        }

                        // Real 파일 → Real 폴더 (eps)
                        using (WebClient wc = new WebClient())
                        {
                            wc.DownloadFileCompleted += OpenPhotoshop(realWorkFolderPath + "\\" + id + epsExtMark);
                            wc.DownloadFileAsync(new Uri(string.Format("{0}/REAL/{1}/{2}/{3}/{4}", downloadUrl, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), id + epsExtMark)), realWorkFolderPath + "\\" + id + epsExtMark);
                        }
                    }

                    // Real 파일 → Origin 폴더 (jpg)
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadFileAsync(new Uri(string.Format("{0}/REAL/{1}/{2}/{3}/{4}", downloadUrl, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), id + realExtMark)), originWorkFolderPath + "\\" + id + realExtMark);
                    }

                    // Real 파일 → Real 폴더 (jpg)
                    using (WebClient wc = new WebClient())
                    {
                        if (!isEps)
                            wc.DownloadFileCompleted += OpenPhotoshop(realWorkFolderPath + "\\" + id + realExtMark);

                        wc.DownloadFileAsync(new Uri(string.Format("{0}/REAL/{1}/{2}/{3}/{4}", downloadUrl, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), id + realExtMark)), realWorkFolderPath + "\\" + id + realExtMark);
                    }

                    // Prev 파일 → Prev 폴더 (jpg)
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadFileCompleted += OpenWorkForm(id);
                        wc.DownloadFileAsync(new Uri(string.Format("{0}/PREV/{1}/{2}/{3}/{4}", downloadUrl, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), id + prevExtMark)), prevWorkFolderPath + "\\" + id + prevExtMark);
                    }

                    // Thumb 파일 → Thumb 폴더 (jpg)
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadFileAsync(new Uri(string.Format("{0}/THUMB/{1}/{2}/{3}/{4}", downloadUrl, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), id + thumbExtMark)), thumbWorkFolderPath + "\\" + id + thumbExtMark);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("파일 열기 중 오류가 발생했습니다. IT개발부로 문의 바랍니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.SaveLog("Image Open Fail: 파일 ID: " + id + "\n" + ex);
            }
        }

        private AsyncCompletedEventHandler OpenPhotoshop(string filePath)
        {
            Action<object, AsyncCompletedEventArgs> action = (sender, e) =>
            {
                photoshop.Open(filePath, Missing.Value, Missing.Value);
            };

            return new AsyncCompletedEventHandler(action);
        }

        // 작업창 열기
        private AsyncCompletedEventHandler OpenWorkForm(string id)
        {
            Action<object, AsyncCompletedEventArgs> action = (sender, e) =>
            {
                Form[] workFormList = Application.OpenForms.Cast<Form>().Where(x => x.Name == "WorkForm").ToArray();

                if (workFormList.Length != 0)
                    workFormList[0].Close();

                Delay(1000);

                WorkForm workForm = new WorkForm(this, id, fontDialog);
                workForm.Show();

                Search();
            };

            return new AsyncCompletedEventHandler(action);
        }

        // 면 CB 트리 노드 클릭
        private void myun_TV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            int tree_myun_cnt = 0;

            MYUN = "";

            if (e.Node.Name == "")   // 전체 선택
            {
                CallRecursive(myun_TV);
            }
            else
            {
                // 체크박스 검사
                foreach (TreeNode nd in myun_TV.Nodes[0].Nodes)
                {
                    if (nd.Checked)
                    {
                        MYUN += nd.Text + " ";
                        tree_myun_cnt++;
                    }
                }

                // 전체 선택인지 체크
                if (tree_myun_cnt == myun_TV.Nodes[0].Nodes.Count)
                {
                    myun_TV.Nodes[0].Checked = true;
                    MYUN = "전체";
                }
                else
                {
                    myun_TV.Nodes[0].Checked = false;
                }

                // 글자수 체크
                if (MYUN.Length > 12)
                    myun_TB.Text = MYUN.Substring(0, 12) + "..";
                else
                    myun_TB.Text = MYUN;

                // 툴팁 내용 재설정
                toolTip1.SetToolTip(myun_TB, MYUN);
            }
        }

        private void CallRecursive(TreeView treeView)
        {
            foreach (TreeNode tv in treeView.Nodes)
                PrintRecursive(tv);
        }

        private void PrintRecursive(TreeNode treeNode)
        {
            if (treeNode.Text == "전체")
            {
                if (treeNode.Checked == true)
                {
                    isMyunTotalChecked = true;
                    myun_TB.Text = "전체";
                }
                else
                {
                    isMyunTotalChecked = false;
                    myun_TB.Text = "";
                }
            }

            treeNode.Checked = isMyunTotalChecked;
            toolTip1.SetToolTip(myun_TB, myun_TB.Text);

            foreach (TreeNode tn in treeNode.Nodes)
                PrintRecursive(tn);
        }

        // 면 CB 클릭
        private void myun_TB_Click(object sender, EventArgs e)
        {
            if (myun_TV.Visible)
            {
                hideTreeMyun(sender, e);
                myun_TB.Focus();
            }
            else
            {
                myun_TV.Show();
                myun_TV.Focus();
            }
        }

        // 면 CB 트리 숨기기
        private void hideTreeMyun(object sender, EventArgs e)
        {
            myun_TV.Hide();
        }

        private void hideTreeMyun(object sender, MouseEventArgs e)
        {
            myun_TV.Hide();
        }

        //
        private void media_CB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Search();
        }

        //
        private void date_RB1_CheckedChanged(object sender, EventArgs e)
        {
            Search();
        }

        //
        private void date_CAL1_ValueChanged(object sender, EventArgs e)
        {
            // CAL1보다 CAL2가 이후 날짜인지 
            DateTime date1 = date_CAL1.Value;
            DateTime date2 = date_CAL2.Value;

            if (date1 > date2)
                date_CAL2.Value = date1;

            Search();
        }

        private void date_CAL1_DropDown(object sender, EventArgs e)
        {
            date_CAL1.ValueChanged += date_CAL1_ValueChanged;
        }

        private void date_CAL1_CloseUp(object sender, EventArgs e)
        {
            date_CAL1.ValueChanged -= date_CAL1_ValueChanged;
        }

        //
        private void date_CAL2_ValueChanged(object sender, EventArgs e)
        {
            // CAL1보다 CAL2가 이전 날짜인지       
            DateTime date1 = date_CAL1.Value;
            DateTime date2 = date_CAL2.Value;

            if (date1 > date2)
                date_CAL1.Value = date2;

            Search();
        }

        private void date_CAL2_DropDown(object sender, EventArgs e)
        {
            date_CAL2.ValueChanged += date_CAL2_ValueChanged;
        }

        private void date_CAL2_CloseUp(object sender, EventArgs e)
        {
            date_CAL2.ValueChanged -= date_CAL2_ValueChanged;
        }

        // 판 변경
        private void pan_CB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Search();
        }

        // 면 변경
        private void myun_TB_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        // 상태 변경
        private void state_CB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Search();
        }

        // 페이지 변경
        private void page_CB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            InitPreview();

            if (bgWorker.IsBusy)
            {
                bgWorker.Abort();
                bgWorker.Dispose();

                bgWorker = new AbortableBackgroundWorker();
                bgWorker.DoWork += bgWorker_DoWork;
            }

            string sql = GetSearchSql();
            SetListView(sql);
            SetGridView(sql);

            // 마지막 선택 이미지
            if (imgSelectedIdx > -1 && imgSelectedIdx < imgListView.Items.Count)
            {
                // 리스트뷰 선택 & 스크롤
                imgListView.Items[imgSelectedIdx].Selected = true;
                imgListView.Items[imgSelectedIdx].Focused = true;
                imgListView.EnsureVisible(imgSelectedIdx);

                // 그리드뷰 선택 & 스크롤
                imgGridView.Rows[imgSelectedIdx].Selected = true;
                imgGridView.CurrentCell = imgGridView.Rows[imgSelectedIdx].Cells[0];

                // 프리뷰
                ShowPreview();
            }
        }

        // 크기 변경
        private void view_BAR_ValueChanged(object sender, EventArgs e)
        {
            imgList.ImageSize = new Size(size_BAR.Value, size_BAR.Value);
            Search();
        }

        // 리스트뷰 클릭
        private void imgListView_Click(object sender, EventArgs e)
        {
            if (imgListView.SelectedItems.Count > 0)
            {
                int index = imgListView.SelectedItems[0].Index;

                // 그리드뷰 선택 & 스크롤
                imgGridView.Rows[index].Selected = true;
                imgGridView.CurrentCell = imgGridView.Rows[index].Cells[0];

                // 프리뷰
                ShowPreview();
            }
        }

        // 그리드뷰 클릭
        private void imgGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (imgGridView.SelectedRows.Count > 0 && e.RowIndex != -1)
            {
                // 리스트뷰 선택 & 스크롤
                imgListView.Items[e.RowIndex].Selected = true;
                imgListView.EnsureVisible(e.RowIndex);

                // 프리뷰
                ShowPreview();
            }
        }

        // 리스트뷰 방향키
        private void imgListView_KeyUp(object sender, KeyEventArgs e)
        {
            if (imgListView.SelectedItems.Count > 0)
            {
                ListViewItem item = imgListView.SelectedItems[0];
                int index = item.Index;

                if (imgSelectedIdx == index)
                {
                    switch (e.KeyCode.ToString())
                    {
                        case "Left":
                            if (index > 0) index--;
                            break;
                        case "Right":
                            if (index < imgListView.Items.Count - 1) index++;
                            break;
                    }

                    // 리스트뷰 선택 & 스크롤
                    imgListView.Items[index].Selected = true;
                    imgListView.Items[index].Focused = true;
                    imgListView.EnsureVisible(index);
                }

                // 그리드뷰 선택 & 스크롤 (if문에 넣으면 X)
                imgGridView.Rows[index].Selected = true;
                imgGridView.CurrentCell = imgGridView.Rows[index].Cells[0];

                // 프리뷰
                ShowPreview();
            }
        }

        // 그리드뷰 방향키
        private void imgGridView_KeyUp(object sender, KeyEventArgs e)
        {
            if (imgGridView.SelectedRows.Count > 0)
            {
                int index = imgGridView.SelectedRows[0].Index;

                // 리스트뷰 선택 & 스크롤
                imgListView.Items[index].Selected = true;
                imgListView.EnsureVisible(index);

                // 프리뷰
                ShowPreview();
            }
        }

        // 리스트뷰 오른쪽 클릭
        private void imgListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextMenuStrip1.Show(Cursor.Position);
        }

        // 그리드뷰 오른쪽 클릭
        private void imgGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex != -1)
            {
                contextMenuStrip1.Show(Cursor.Position);

                // 그리드뷰 선택 & 스크롤
                imgGridView.Rows[e.RowIndex].Selected = true;
                imgGridView.CurrentCell = imgGridView.Rows[e.RowIndex].Cells[0];

                // 리스트뷰 선택 & 스크롤
                imgListView.Items[e.RowIndex].Selected = true;
                imgListView.EnsureVisible(e.RowIndex);

                // 프리뷰
                ShowPreview();
            }
        }

        // 리스트뷰 더블 클릭
        private void imgListView_DoubleClick(object sender, EventArgs e)
        {
            OpenFile();
        }

        // 그리드뷰 더블 클릭
        private void imgGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
                OpenFile();
        }

        private void ShowPreview()
        {
            if (imgListView.SelectedItems.Count > 0)
            {
                ListViewItem item = imgListView.SelectedItems[0];
                string date = item.SubItems[IDX_REGTIME].Text;
                string id = item.SubItems[IDX_ID].Text;
                string fileName = id + prevExtMark;

                imgPreView.Image = ViewWebImage(string.Format("{0}/PREV/{1}/{2}/{3}/{4}", downloadUrl, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), fileName), id);

                filename_TB.Text = item.SubItems[IDX_OFILENAME].Text;
                title_TB.Text = item.SubItems[IDX_TITLE].Text;
                caption_TB.Text = item.SubItems[IDX_CAPTION].Text;
                retouch_TB.Text = item.SubItems[IDX_RETOUCH].Text;

                imgSelectedIdx = item.Index;
            }
            else
            {
                InitPreview();
            }
        }

        private void InitPreview()
        {
            imgPreView.Image = null;

            filename_TB.Text = "-";
            title_TB.Text = "-";
            caption_TB.Text = "-";
            retouch_TB.Text = "-";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool bHandled = false;

            switch (keyData)
            {
                case Keys.F5:
                    Search();
                    bHandled = true;
                    break;
            }

            return bHandled;
        }

        // 검색 초기화 버튼 클릭
        private void init_BTN_Click(object sender, EventArgs e)
        {
            Init();
        }

        // 새로고침 버튼 클릭
        private void refresh_BTN_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 열린 WorkForm 닫기
            Form[] workFormList = Application.OpenForms.Cast<Form>().Where(x => x.Name == "WorkForm").ToArray();

            if (workFormList.Length != 0)
                workFormList[0].Close();

            // 환경설정 저장
            WriteValue("SIZE", "FORM_WIDTH", Size.Width.ToString());
            WriteValue("SIZE", "FORM_HEIGHT", Size.Height.ToString());
            WriteValue("SIZE", "LV_IMG", size_BAR.Value.ToString());
            WriteValue("FONT", "FONT", TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(fontDialog.Font));
            WriteValue("FONT", "COLOR", TypeDescriptor.GetConverter(typeof(Color)).ConvertToString(fontDialog.Color));
            WriteValue("THEME", "IDX_BG", IDX_THEME_BG.ToString());
            WriteValue("THEME", "IDX_ICON", IDX_THEME_ICON.ToString());
            WriteValue("FOLDER_PATH", "ORIGIN_WORK", originWorkFolderPath);
            WriteValue("FOLDER_PATH", "REAL_WORK", realWorkFolderPath);
            WriteValue("FOLDER_PATH", "PREV_WORK", prevWorkFolderPath);
            WriteValue("FOLDER_PATH", "THUMB_WORK", thumbWorkFolderPath);
            WriteValue("FOLDER_PATH", "REAL_DOWNLOAD", realDownloadFolderPath);
            WriteValue("FOLDER_PATH", "PREV_DOWNLOAD", prevDownloadFolderPath);
            WriteValue("FOLDER_PATH", "THUMB_DOWNLOAD", thumbDownloadFolderPath);
            WriteValue("SPLITTER", "FORM_V", splitContainer2.SplitterDistance.ToString());
            WriteValue("SPLITTER", "FORM_H1", splitContainer3.SplitterDistance.ToString());
            WriteValue("SPLITTER", "FORM_H2", splitContainer4.SplitterDistance.ToString());
            WriteValue("SPLITTER", "COL_HEADER", imgGridView.ColumnHeadersHeight.ToString());

            // 컬럼 헤더 크기
            for (int i = 0; i < imgGridView.Columns.Count; i++)
                WriteValue("SPLITTER", "COL" + i, imgGridView.Columns[i].Width.ToString());

            // todo: 파일 삭제?
        }

        // 글꼴 설정 버튼 클릭
        private void settings_menu1_Click(object sender, EventArgs e)
        {
            try
            {
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    SetFont(sender, e);
                }
            }
            catch (ArgumentException)
            {
                MessageBox.Show("지원되는 글꼴이 아닙니다.", "글꼴 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void SetFont(object sender, EventArgs e)
        {
            imgListView.Font = fontDialog.Font;
            imgListView.ForeColor = fontDialog.Color;

            imgGridView.Font = fontDialog.Font;
            imgGridView.ForeColor = fontDialog.Color;

            imgGridView.AlternatingRowsDefaultCellStyle.Font = fontDialog.Font;
            imgGridView.AlternatingRowsDefaultCellStyle.ForeColor = fontDialog.Color;

            filename_TB.Font = fontDialog.Font;
            filename_TB.ForeColor = fontDialog.Color;

            title_TB.Font = fontDialog.Font;
            title_TB.ForeColor = fontDialog.Color;

            caption_TB.Font = fontDialog.Font;
            caption_TB.ForeColor = fontDialog.Color;

            retouch_TB.Font = fontDialog.Font;
            retouch_TB.ForeColor = fontDialog.Color;
        }

        // Thumb 저장 버튼 클릭
        private void save_thumb_BTN_Click(object sender, EventArgs e)
        {
            DownloadFile("thumb");
        }

        // Prev 저장 버튼 클릭
        private void save_prev_BTN_Click(object sender, EventArgs e)
        {
            DownloadFile("prev");
        }

        // Real 저장 버튼 클릭
        private void save_real_BTN_Click(object sender, EventArgs e)
        {
            DownloadFile("real");
        }

        private void DownloadFile(string kind)
        {
            if (imgListView.SelectedItems.Count > 0)
            {
                ListViewItem item = imgListView.SelectedItems[0];
                string date = item.SubItems[IDX_REGTIME].Text;
                string fileName = item.SubItems[IDX_ID].Text;
                string fileName_saveAs = item.SubItems[IDX_OFILENAME].Text;
                string fileFilter = "";

                // 마지막 작업 파일이 EPS 확장자인지 아닌지
                bool isEps = false;
                dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select top 1 C_EXTENSION from T_IMAGE where ID_FIM = '{0}' order by D_PUBDATE desc", fileName)), "SELECT");

                if (dt.Rows[0].ItemArray[0].ToString().Trim().ToLower() == "eps")
                    isEps = true;

                if (kind == "real")
                {
                    saveFileDialog.InitialDirectory = realDownloadFolderPath;

                    if (!isEps)
                    {
                        fileName += realExtMark;
                        fileName_saveAs += realExtMark;
                        fileFilter = "Jpg Files (*.jpg, *.JPG)|*.jpg;*.JPG";
                    }
                    else
                    {
                        fileName += epsExtMark;
                        fileName_saveAs += epsExtMark;
                        fileFilter = "Eps Files (*.eps, *.EPS)|*.eps;*.EPS";
                    }
                }
                else if (kind == "prev")
                {
                    saveFileDialog.InitialDirectory = prevDownloadFolderPath;

                    fileName += prevExtMark;
                    fileName_saveAs += prevExtMark;
                    fileFilter = "Jpg Files (*.jpg, *.JPG)|*.jpg;*.JPG";
                }
                else if (kind == "thumb")
                {
                    saveFileDialog.InitialDirectory = thumbDownloadFolderPath;

                    fileName += thumbExtMark;
                    fileName_saveAs += thumbExtMark;
                    fileFilter = "Jpg Files (*.jpg, *.JPG)|*.jpg;*.JPG";
                }

                try
                {
                    saveFileDialog.FileName = fileName_saveAs;
                    saveFileDialog.Filter = fileFilter;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        using (WebClient wc = new WebClient())
                        {
                            wc.DownloadFileAsync(new Uri(string.Format("{0}/{1}/{2}/{3}/{4}/{5}", downloadUrl, kind, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), fileName)), saveFileDialog.FileName);
                        }

                        MessageBox.Show("저장되었습니다.", "파일 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Util.SaveLog("File Save Complete: 파일명: " + fileName);

                        if (kind == "real")
                            realDownloadFolderPath = Path.GetDirectoryName(saveFileDialog.FileName);
                        else if (kind == "prev")
                            prevDownloadFolderPath = Path.GetDirectoryName(saveFileDialog.FileName);
                        else if (kind == "thumb")
                            thumbDownloadFolderPath = Path.GetDirectoryName(saveFileDialog.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("파일 저장 중 오류가 발생했습니다. IT개발부로 문의 바랍니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Util.SaveLog("File Save Fail: 파일명: " + fileName + ", action: " + kind + "\n" + ex);
                }
            }
        }

        // 로그아웃 버튼 클릭
        private void logout_BTN_Click(object sender, EventArgs e)
        {
            hideTreeMyun(sender, e);

            DialogResult result = MessageBox.Show(this, "로그아웃되었습니다.", "화상 작업", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (result == DialogResult.OK)
                Application.Restart();
            else
                Application.Exit();

            Util.SaveLog("Logout: " + empNo);
        }

        // 배경 설정
        private void settings_menu2_1_Click(object sender, EventArgs e)
        {
            SetThemeBG(0);
        }

        private void settings_menu2_2_Click(object sender, EventArgs e)
        {
            SetThemeBG(1);
        }

        private void settings_menu2_3_Click(object sender, EventArgs e)
        {
            SetThemeBG(2);
        }

        private void SetThemeBG(int index)
        {
            IDX_THEME_BG = index;

            ToolStripItemCollection items = settings_menu2.DropDownItems;

            for (int i = 0; i < items.Count; i++)
            {
                if (i == index)
                    ((ToolStripMenuItem)items[i]).Checked = true;
                else
                    ((ToolStripMenuItem)items[i]).Checked = false;
            }

            Search();
        }

        // 아이콘 설정
        private void settings_menu3_1_Click(object sender, EventArgs e)
        {
            SetThemeIcon(0);
        }

        private void settings_menu3_2_Click(object sender, EventArgs e)
        {
            SetThemeIcon(1);
        }

        private void settings_menu3_3_Click(object sender, EventArgs e)
        {
            SetThemeIcon(2);
        }

        private void SetThemeIcon(int index)
        {
            IDX_THEME_ICON = index;

            ToolStripItemCollection items = settings_menu3.DropDownItems;

            for (int i = 0; i < items.Count; i++)
            {
                if (i == index)
                    ((ToolStripMenuItem)items[i]).Checked = true;
                else
                    ((ToolStripMenuItem)items[i]).Checked = false;
            }

            SetIconPath();
            Search();
        }

        private void print_prev_BTN_Click(object sender, EventArgs e)
        {
            if (imgListView.SelectedItems.Count > 0)
            {
                ListViewItem item = imgListView.SelectedItems[0];
                string id = item.SubItems[IDX_ID].Text;

                PrintPrevForm printPrevForm = new PrintPrevForm(id);
                printPrevForm.Show();
            }
        }

        // 파일 복사 - 일반
        private void ToolStripMenuItem1_1_Click(object sender, EventArgs e)
        {
            Search();
            MessageBox.Show("일반 복사되었습니다.", "파일 복사", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 파일 복사 - 원화상
        private void ToolStripMenuItem1_2_Click(object sender, EventArgs e)
        {
            Search();
            MessageBox.Show("원화상 복사되었습니다.", "파일 복사", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 전표 인쇄 버튼 클릭
        private void print_BTN_Click(object sender, EventArgs e)
        {
            DialogResult confirmResult = MessageBox.Show("인쇄하시겠습니까?", "전표 인쇄", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (confirmResult == DialogResult.Yes)
            {
                string id = imgListView.SelectedItems[0].SubItems[IDX_ID].Text;

                WebBrowser webBrowser = new WebBrowser();
                webBrowser.Navigate(printUrl + id);
                webBrowser.Print();
            }
        }

        private void settings_menu4_Click(object sender, EventArgs e)
        {
            FolderSetForm folderSetForm = new FolderSetForm();
            folderSetForm.ShowDialog();
        }

        private DateTime Delay(int ms)
        {
            DateTime dateTimeNow = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, ms);
            DateTime dateTimeAdd = dateTimeNow.Add(duration);

            while (dateTimeAdd >= dateTimeNow)
            {
                Application.DoEvents();
                dateTimeNow = DateTime.Now;
            }

            return DateTime.Now;
        }

        // 잠금 해제
        private void ToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            DialogResult confirmResult = MessageBox.Show("잠금 해제하시겠습니까?", "잠금 해제", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (confirmResult == DialogResult.Yes)
            {
                string id = imgListView.SelectedItems[0].SubItems[IDX_ID].Text;

                try
                {
                    // T_FLOWIMG 업데이트
                    dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [CMSNS5].[dbo].[T_FLOWIMG] set 
ID_LOCKERCODE = NULL
where ID_FIM = '{0}'", id)), "UPDATE");

                    Search();
                    MessageBox.Show("잠금 해제되었습니다.", "잠금 해제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("잠금 해제 중 오류가 발생했습니다. IT개발부로 문의 바랍니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Util.SaveLog("Image Unlock Fail: 파일 ID: " + id + "\n" + ex);
                }
            }
        }
    }
}
