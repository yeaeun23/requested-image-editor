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

        //string sortAscMark = "▲";
        //string sortDescMark = "▼";

        string MYUN = "전체";
        bool isMyunTotalChecked;        // 면 전체 선택 여부
        int imgPerPage = 18;            // 페이지 당 이미지 개수
        int imgTotalCnt = 0;            // 전체 이미지 개수
        int imgSelectedIdx = -1;        // 선택된 이미지 idx             
        int pageSelectedIdx = -1;       // 선택된 페이지 idx   
        bool isInitBtnClicked = false;  // 검색 초기화 버튼 클릭 여부: Search() 여러번 방지

        public static string empNo = "1234567";
        public static string empName = "테스트";
        public static string empCode = "123";

        public static string iniFilePath = Application.StartupPath + "\\ImageWork.ini";

        public static string downloadUrl = GetValue("URL", "DOWNLOAD");
        public static string uploadUrl1 = GetValue("URL", "UPLOAD1");
        public static string uploadUrl2 = GetValue("URL", "UPLOAD2");
        public static string copyUrl = GetValue("URL", "COPY");
        public static string printUrl = GetValue("URL", "PRINT");

        public static string originWorkFolderPath = GetValue("FOLDER_PATH", "ORIGIN_WORK");
        public static string realWorkFolderPath = GetValue("FOLDER_PATH", "REAL_WORK");
        public static string prevWorkFolderPath = GetValue("FOLDER_PATH", "PREV_WORK");
        public static string thumbWorkFolderPath = GetValue("FOLDER_PATH", "THUMB_WORK");
        public static string eventWorkFolderPath = GetValue("FOLDER_PATH", "EVENT_WORK");

        public static string realDownloadFolderPath = GetValue("FOLDER_PATH", "REAL_DOWNLOAD");
        public static string prevDownloadFolderPath = GetValue("FOLDER_PATH", "PREV_DOWNLOAD");
        public static string thumbDownloadFolderPath = GetValue("FOLDER_PATH", "THUMB_DOWNLOAD");

        public static int IDX_OFILENAME = 0;        // 파일명
        public static int IDX_TITLE = 1;            // 제목
        public static int IDX_EXTENSION = 2;        // 확장자
        public static int IDX_EXTENSION_ORG = 3;    // 확장자(o)
        public static int IDX_AUTHOR = 4;           // 등록자
        public static int IDX_PUBDATE = 5;          // 출고일
        public static int IDX_BEFOREPUBDATE = 6;    // 게재일
        public static int IDX_REGTIME = 7;          // 등록일
        public static int IDX_PAN = 8;              // 판
        public static int IDX_PAGE = 9;             // 면
        public static int IDX_BEFORESTATE = 10;     // 상태
        public static int IDX_ID = 11;              // 아이디
        public static int IDX_CAPTION = 12;         // 내용
        public static int IDX_RETOUCH = 13;         // 요청사항

        public static int IDX_THEME_BG = 0;         // 배경
        public static int IDX_THEME_ICON = 0;       // 아이콘   
        public static int IDX_THEME = 0;            // 테마
        public static int IDX_THEME_DARK1 = 0;
        public static int IDX_THEME_DARK2 = 1;
        public static int IDX_THEME_LIGHT1 = 2;
        public static int IDX_THEME_LIGHT2 = 3;
        public static int IDX_THEME_BLUE = 4;
        public static Color[] themeDark1 = {
            Color.FromArgb(83, 83, 83),
            Color.FromArgb(40, 40, 40),
            Color.FromArgb(56, 56, 56),
            Color.FromArgb(69, 69, 69),
            Color.FromArgb(240, 240, 240) // 글자색
        };
        public static Color[] themeDark2 = {
            Color.FromArgb(50, 50, 50),
            Color.FromArgb(25, 25, 25),
            Color.FromArgb(31, 31, 31),
            Color.FromArgb(41, 41, 41),
            Color.FromArgb(225, 225, 225)
        };
        public static Color[] themeLight1 = {
            Color.FromArgb(240, 240, 240),
            Color.FromArgb(147, 147, 147),
            Color.FromArgb(191, 191, 191),
            Color.FromArgb(252, 252, 252),
            Color.FromArgb(41, 41, 41)
        };
        public static Color[] themeLight2 = {
            Color.FromArgb(184, 184, 184),
            Color.FromArgb(108, 108, 108),
            Color.FromArgb(128, 128, 128),
            Color.FromArgb(209, 209, 209),
            Color.FromArgb(51, 51, 51)
        };
        public static Color[] themeBlue = {
            Color.FromArgb(57, 88, 109),
            Color.FromArgb(29, 43, 54),
            Color.FromArgb(40, 59, 74),
            Color.FromArgb(40, 59, 74),
            Color.FromArgb(240, 240, 240)
        };

        WorkForm workForm = null;                   // 꼬마창

        string srcDirName = @"\\dapsn1.seoul.co.kr\PatchImageWork";
        string destDirName = @"C:\화상작업기";
        string srcVer = "";
        string destVer = "";

        // ini 파일 쓰기
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        // ini 파일 읽기
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        // 창 활성화
        // nCmdShow: 1-NORMAL, 2-MINIMIZED, 3-MAXIMIZED
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        // 창 최상위로
        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hWnd);

        // 업데이트 로직
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct NETRESOURCE
        {
            public uint dwScope;
            public uint dwType;
            public uint dwDisplayType;
            public uint dwUsage;
            public string lpLocalName;
            public string lpRemoteName;
            public string lpComment;
            public string lpProvider;
        }
        [DllImport("mpr.dll", CharSet = CharSet.Auto)]
        public static extern int WNetUseConnection(IntPtr hwndOwner, [MarshalAs(UnmanagedType.Struct)] ref NETRESOURCE lpNetResource, string lpPassword, string lpUserID, uint dwFlags, StringBuilder lpAccessName, ref int lpBufferSize, out uint lpResult);

        public static void WriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, iniFilePath);
        }

        public static string GetValue(string Section, string Key)
        {
            GetPrivateProfileString(Section, Key, string.Empty, sb, 255, iniFilePath);
            return sb.ToString();
        }

        public Form1()
        {
            Util.Delay(2000);

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
                VersionCheck();

                if (srcVer != destVer)  // 버전이 다르면
                {
                    // 업데이트
                    Process.Start(destDirName + @"\UpdateImageWork.exe");
                    Process.GetCurrentProcess().Kill();
                }
                else                    // 버전이 같으면
                {
                    // 로그인
                    LoginForm loginForm = new LoginForm();

                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        Util.SaveLog("Login: " + empNo);
                        InitializeComponent();

                        // 테마                        
                        if (IDX_THEME == IDX_THEME_DARK2)
                            SetTheme(IDX_THEME_DARK2, themeDark2, false);
                        else if (IDX_THEME == IDX_THEME_LIGHT1)
                            SetTheme(IDX_THEME_LIGHT1, themeLight1, false);
                        else if (IDX_THEME == IDX_THEME_LIGHT2)
                            SetTheme(IDX_THEME_LIGHT2, themeLight2, false);
                        else if (IDX_THEME == IDX_THEME_BLUE)
                            SetTheme(IDX_THEME_BLUE, themeBlue, false);
                    }
                    else
                    {
                        Process.GetCurrentProcess().Kill();
                    }
                }
            }
        }

        private void VersionCheck()
        {
            int res = netUse();

            if (res != 0 && res != 1219)
            {
                MessageBox.Show(new Form { TopMost = true }, "서버에 연결할 수 없습니다. (Error Code: " + res.ToString() + ")", "업데이트 확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                srcVer = File.ReadAllText(srcDirName + @"\ImageWork_Ver.txt");

                GetPrivateProfileString("VERSION", "VER", "", sb, sb.Capacity, destDirName + @"\ImageWork.ini");
                if (sb.ToString() != "")
                    destVer = sb.ToString();
            }
        }

        private int netUse()
        {
            int capacity = 64;
            uint resultFlags = 0;
            StringBuilder sb = new StringBuilder(capacity);
            NETRESOURCE ns = new NETRESOURCE();

            ns.dwType = 1;           // 공유 디스크
            ns.lpLocalName = null;   // 로컬 드라이브
            ns.lpRemoteName = srcDirName;
            ns.lpProvider = null;

            return WNetUseConnection(IntPtr.Zero, ref ns, "!!updateuser@@", "dapsn1\\updateuser", 0, sb, ref capacity, out resultFlags);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 포토샵 실행
            Type type = Type.GetTypeFromProgID("Photoshop.Application");
            photoshop = (Photoshop.Application)Activator.CreateInstance(type, true);

            // 상태표시줄
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
            {
                IDX_THEME_BG = Convert.ToInt32(GetValue("THEME", "IDX_BG"));
                SetThemeBG(IDX_THEME_BG, false);
            }

            // 아이콘
            if (GetValue("THEME", "IDX_ICON") != "")
            {
                IDX_THEME_ICON = Convert.ToInt32(GetValue("THEME", "IDX_ICON"));
                SetThemeIcon(IDX_THEME_ICON, false);
            }

            // 폴더
            if (!Directory.Exists(originWorkFolderPath))
                Directory.CreateDirectory(originWorkFolderPath);

            if (!Directory.Exists(realWorkFolderPath))
                Directory.CreateDirectory(realWorkFolderPath);

            if (!Directory.Exists(prevWorkFolderPath))
                Directory.CreateDirectory(prevWorkFolderPath);

            if (!Directory.Exists(thumbWorkFolderPath))
                Directory.CreateDirectory(thumbWorkFolderPath);

            if (!Directory.Exists(eventWorkFolderPath))
                Directory.CreateDirectory(eventWorkFolderPath);

            // 작업 파일 삭제
            deleteWorkFile(originWorkFolderPath);
            deleteWorkFile(realWorkFolderPath);
            deleteWorkFile(prevWorkFolderPath);
            deleteWorkFile(thumbWorkFolderPath);

            // 스플리터     
            if (GetValue("SPLITTER", "FORM_V") != "")
                splitContainer2.SplitterDistance = Convert.ToInt32(GetValue("SPLITTER", "FORM_V"));

            if (GetValue("SPLITTER", "FORM_H1") != "")
                splitContainer3.SplitterDistance = Convert.ToInt32(GetValue("SPLITTER", "FORM_H1"));

            if (GetValue("SPLITTER", "FORM_H2") != "")
                splitContainer4.SplitterDistance = Convert.ToInt32(GetValue("SPLITTER", "FORM_H2"));

            if (GetValue("SPLITTER", "COL_HEADER") != "")
                imgGridView.ColumnHeadersHeight = Convert.ToInt32(GetValue("SPLITTER", "COL_HEADER"));

            if (GetValue("SPLITTER", "COL_HEADER2") != "")
                imgGridView2.ColumnHeadersHeight = Convert.ToInt32(GetValue("SPLITTER", "COL_HEADER2"));

            for (int i = 0; i < imgGridView.Columns.Count; i++)
            {
                if (GetValue("SPLITTER", "COL" + i) != "")
                    imgGridView.Columns[i].Width = Convert.ToInt32(GetValue("SPLITTER", "COL" + i));
            }

            for (int i = 0; i < imgGridView2.Columns.Count; i++)
            {
                if (GetValue("SPLITTER", "COL" + (i + 20)) != "")
                    imgGridView2.Columns[i].Width = Convert.ToInt32(GetValue("SPLITTER", "COL" + (i + 20)));
            }

            // 꼬마창 위치 초기화
            WriteValue("POS", "WORKFORM_X", "50");
            WriteValue("POS", "WORKFORM_Y", "50");

            // 컨트롤 로드
            SetMediaCBList();
            Util.SetPanCBList(pan_CB);
            SetMyunCBList();

            // 검색 초기화
            Init();
        }

        // 아이콘 경로 설정
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
                MessageBox.Show(new Form { TopMost = true }, "아이콘 파일을 확인해 주세요.", "아이콘 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // 초기화
        private void Init()
        {
            // 컨트롤 값 초기화
            media_CB.SelectedIndex = 0;
            date_RB1.Checked = true;
            date_CAL1.Value = DateTime.Now.AddDays(1);
            date_CAL2.Value = DateTime.Now.AddDays(1);
            pan_CB.SelectedIndex = 0;

            myun_TB.Text = "전체";
            myun_TV.Nodes[0].Checked = true;
            myun_TV.Hide();

            state_CB.SelectedIndex = 0;

            imgSelectedIdx = -1;
            pageSelectedIdx = -1;

            Search();
        }

        // 검색
        public void Search()
        {
            InitPreview();
            SetPageCBList();

            if (bgWorker.IsBusy)
                return;

            string sql = GetSearchSql();

            //if (sql != "")
            //{
            SetListView(sql);
            SetGridView(sql);
            SetGridView2();

            bgWorker.RunWorkerAsync();

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
            //}
            //else
            //{
            //    return;
            //}
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
            //    case 2: // 등록자
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

            return "ID_FIM DESC";
        }

        // 검색 쿼리 생성
        private string GetSearchSql()
        {
            string sql = "select top " + imgPerPage + " * from (select row_number() over (";

            sql += "order by " + GetSortSql();
            sql += ") as rownum, * from [DAPS2022].[dbo].[CMS_FLOWIMG] where ";

            if (media_CB.SelectedIndex > 0)
                sql += "ID_MECHAE = '" + media_CB.Text.Split('-')[1] + "' and ";

            sql += "(cast(" + (date_RB1.Checked ? "D_BEFOREPUBDATE" : "D_REGTIME") + " as date) between '";
            sql += date_CAL1.Value.ToString().Substring(0, 10).Replace("-", "") + "' and '";
            sql += date_CAL2.Value.ToString().Substring(0, 10).Replace("-", "") + "') ";

            if (pan_CB.SelectedIndex > 0)
                sql += "and N_PAN = '" + pan_CB.Text + "' ";

            sql += "and (" + GetMyunSql() + ") ";

            if (state_CB.Text == "출고")
                sql += "and NM_FLOWSTATE = 1 and B_DELETE = 0";
            else if (state_CB.Text == "미출고")
                sql += "and NM_FLOWSTATE = 0 and B_DELETE = 0";
            else if (state_CB.Text == "휴지통")
                sql += "and (NM_FLOWSTATE = 0 or NM_FLOWSTATE = 1) and B_DELETE = 1";
            else // 전체
                sql += "and (NM_FLOWSTATE = 0 or NM_FLOWSTATE = 1) and B_DELETE = 0";

            int pageNow = Convert.ToInt32(page_CB.Text);
            int pageTotal = page_CB.Items.Count;

            if (pageTotal == 1)
            {
                sql += ") t1";
            }
            else
            {
                sql += ") t1 where t1.rownum < " + (imgTotalCnt - (imgPerPage * (pageTotal - pageNow)) + 1);
                sql += " and t1.rownum >= " + (imgTotalCnt - (imgPerPage * (pageTotal + 1 - pageNow)) + 1);
            }

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
                        + reader["C_EXTENSION"].ToString().Trim() + "∥"
                        + reader["V_TAG"].ToString().Trim() + "∥"
                        + reader["V_AUTHOR"].ToString().Trim() + "∥"
                        + reader["D_REGTIME"].ToString().Trim() + "∥"
                        + reader["D_BEFOREPUBDATE"].ToString().Trim().Substring(0, 10) + "∥"
                        + reader["N_PAN"].ToString().Trim() + "∥"
                        + reader["N_PAGE"].ToString().Trim() + "∥"
                        + reader["V_BEFORESTATE"].ToString().Trim() + "∥"
                        + reader["D_PUBDATE"].ToString().Trim() + "∥"
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
                MessageBox.Show(new Form { TopMost = true }, ex.ToString(), "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // 리스트뷰 세팅
        private void SetListView(string str)
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
                    item.SubItems.Add(columns[IDX_EXTENSION]);
                    item.SubItems.Add(columns[IDX_EXTENSION_ORG]);
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
                MessageBox.Show(new Form { TopMost = true }, ex.ToString(), "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 그리드뷰 세팅 - 화상 목록
        private void SetGridView(string str)
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

        // 그리드뷰 세팅 - 작업 내역
        private void SetGridView2()
        {
            // todo: 자동 새로고침 안됨
            if (imgListView.SelectedItems.Count > 0 && tabControl.SelectedIndex == 1)
            {
                imgGridView2.Rows.Clear();

                ListViewItem item = imgListView.SelectedItems[0];
                string id = item.SubItems[IDX_ID].Text;
                string work_time = "";
                string work_dept = "";
                string work_name = "";
                string work_kind = "";
                string work_content = "";

                dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select D_WORKDATE as work_time, 
                    (select V_NAME from [CMSCOM].[dbo].[T_FOLDER] where ID_PUBPART = a.N_PUBPART) as work_dept,
                    (select V_USERNAME from [CMSCOM].[dbo].[T_USERINFO] where ID_USERCODE = a.ID_USERCODE) as work_name,
                    N_WORKKIND as work_kind,
                    V_CONTENT as work_content  
                    from [DAPS2022].[dbo].[CMS_WORKHISTORY] a where ID_FID = '{0}' 
                    order by ID_WORKSEQ desc", id)), "SELECT");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    work_time = dt.Rows[i]["work_time"].ToString();
                    work_dept = dt.Rows[i]["work_dept"].ToString();
                    work_name = dt.Rows[i]["work_name"].ToString();
                    work_kind = dt.Rows[i]["work_kind"].ToString();
                    work_content = dt.Rows[i]["work_content"].ToString();

                    imgGridView2.Rows.Insert(i, work_time, work_dept, work_name, work_kind, work_content);
                }

                imgGridView2.ClearSelection();
            }
        }

        // 이미지 하나씩 추가
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            for (int i = 0; i < imgListView.Items.Count; i++)
            {
                ListViewItem item = imgListView.Items[i];
                string date = item.SubItems[IDX_BEFOREPUBDATE].Text;
                string id = item.SubItems[IDX_ID].Text;
                string ext = item.SubItems[IDX_EXTENSION].Text;
                string ext_o = item.SubItems[IDX_EXTENSION_ORG].Text;
                string fileName = item.SubItems[IDX_OFILENAME].Text;

                try
                {
                    if (ext_o == "png" && ext == "png")
                    {
                        imgList.Images.Add(ViewWebImage(string.Format("{0}/THUMB/{1}/{2}/{3}/{4}", downloadUrl, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), fileName + "." + ext), id));
                    }
                    else
                    {
                        imgList.Images.Add(ViewWebImage(string.Format("{0}/THUMB/{1}/{2}/{3}/{4}", downloadUrl, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), fileName + ".jpg"), id));
                    }
                }
                catch (ArgumentNullException ex)
                {
                    Util.SaveLog("Image Download Fail(Thumb): " + ex);
                    continue;
                }

                imgListView.Items[i].ImageIndex = i;
            }
        }

        // 아이콘
        private void getIconFlag(ref bool flag_caption, ref bool flag_chulgo, ref bool flag_photoshop, string id)
        {
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select N_TICKETFLAG, ID_LOCKERCODE, NM_FLOWSTATE from [DAPS2022].[dbo].[CMS_FLOWIMG] where ID_FIM = '{0}'", id)), "SELECT");

            if (dt.Rows[0]["N_TICKETFLAG"].ToString() == "1")
                flag_caption = true;

            if (dt.Rows[0]["NM_FLOWSTATE"].ToString() == "0")
                flag_chulgo = false;
            else if (dt.Rows[0]["NM_FLOWSTATE"].ToString() == "1")
                flag_chulgo = true;

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

                if (url.Contains("PREV"))   // 오른쪽 프리뷰
                {
                    return downloadImg;
                }
                else                        // 왼쪽 썸네일 리스트(+아이콘)
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
                    if (IDX_THEME_BG == 0)
                    {
                        graphic.Clear(Color.White);
                    }
                    else if (IDX_THEME_BG == 1)
                    {
                        graphic.Clear(Color.Black);
                    }
                    else
                    {
                        if (IDX_THEME == IDX_THEME_DARK1)
                            graphic.Clear(themeDark1[1]);
                        else if (IDX_THEME == IDX_THEME_DARK2)
                            graphic.Clear(themeDark2[1]);
                        else if (IDX_THEME == IDX_THEME_LIGHT1)
                            graphic.Clear(themeLight1[1]);
                        else if (IDX_THEME == IDX_THEME_LIGHT2)
                            graphic.Clear(themeLight2[1]);
                        else
                            graphic.Clear(themeBlue[1]);
                    }

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
                Util.SaveLog("Thumb Image Load Fail: 파일 ID: " + id + ", URL: " + url + "\n" + ex);
                return null;
            }
        }

        // 매체 CB 리스트 세팅
        private void SetMediaCBList()
        {
            //dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select id_mechae, name from [DAPS2022].[dbo].[CMS_MECHAE] order by id_mechae")), "SELECT");

            //foreach (DataRow dr in dt.Rows)
            {
                //media_CB.Items.Add(dr["name"].ToString() + "-" + dr["id_mechae"].ToString()); // "서울신문-65"      
                media_CB.Items.Add("서울신문-65");
            }
        }

        // 면 CB 리스트 세팅
        private void SetMyunCBList()
        {
            TreeNode svrNode = new TreeNode("전체");

            // 1 ~ 99면까지 추가
            for (int i = 1; i < 100; i++)
                svrNode.Nodes.Add(i.ToString(), i.ToString()); // "1"

            myun_TV.Nodes.Add(svrNode);
            myun_TV.ExpandAll();
        }

        // 페이지 CB 리스트 세팅
        private void SetPageCBList()
        {
            page_CB.Items.Clear();

            // 전체 이미지 개수 구하기
            string sql = "select count(*) as cnt from [DAPS2022].[dbo].[CMS_FLOWIMG] where ";

            if (media_CB.SelectedIndex > 0)
                sql += "ID_MECHAE = '" + media_CB.Text.Split('-')[1] + "' and ";

            sql += "(cast(" + (date_RB1.Checked ? "D_BEFOREPUBDATE" : "D_REGTIME") + " as date) between '";
            sql += date_CAL1.Value.ToString().Substring(0, 10).Replace("-", "") + "' and '";
            sql += date_CAL2.Value.ToString().Substring(0, 10).Replace("-", "") + "') ";

            if (pan_CB.SelectedIndex > 0)
                sql += "and N_PAN = '" + pan_CB.Text + "' ";

            sql += "and (" + GetMyunSql() + ") ";

            if (state_CB.Text == "출고")
                sql += "and NM_FLOWSTATE = 1 and B_DELETE = 0";
            else if (state_CB.Text == "미출고")
                sql += "and NM_FLOWSTATE = 0 and B_DELETE = 0";
            else if (state_CB.Text == "휴지통")
                sql += "and (NM_FLOWSTATE = 0 or NM_FLOWSTATE = 1) and B_DELETE = 1";
            else // 전체
                sql += "and (NM_FLOWSTATE = 0 or NM_FLOWSTATE = 1) and B_DELETE = 0";

            dt = Util.ExecuteQuery(new SqlCommand(sql), "SELECT");

            imgTotalCnt = Convert.ToInt32(dt.Rows[0].ItemArray[0]);

            // CB 추가
            if (imgTotalCnt == 0)
            {
                page_CB.Items.Add(1);
            }
            else if (imgTotalCnt % imgPerPage == 0)
            {
                for (int i = (imgTotalCnt / imgPerPage); i > 0; i--)
                    page_CB.Items.Add(i);
            }
            else
            {
                for (int i = (imgTotalCnt / imgPerPage) + 1; i > 0; i--)
                    page_CB.Items.Add(i);
            }

            // 선택된 페이지
            if (pageSelectedIdx == -1)
                page_CB.SelectedIndex = 0;
            else if (pageSelectedIdx <= page_CB.Items.Count - 1)
                page_CB.SelectedIndex = pageSelectedIdx;
            else
                page_CB.SelectedIndex = page_CB.Items.Count - 1;
        }

        private void OpenFile()
        {
            if (imgListView.SelectedItems.Count > 0)
            {
                ListViewItem item = imgListView.SelectedItems[0];
                string date = item.SubItems[IDX_BEFOREPUBDATE].Text;
                string id = item.SubItems[IDX_ID].Text;
                string ext = item.SubItems[IDX_EXTENSION].Text;
                string ext_o = item.SubItems[IDX_EXTENSION_ORG].Text;
                string fileName = item.SubItems[IDX_OFILENAME].Text;
                bool isJPG = (ext == "jpg") ? true : false;

                bool flag_chulgo = false;
                bool flag_photoshop = false;
                string lockercode = "";

                try
                {
                    // 출고 및 작업중 확인 
                    dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select ID_LOCKERCODE, NM_FLOWSTATE from [DAPS2022].[dbo].[CMS_FLOWIMG] where ID_FIM = '{0}'", id)), "SELECT");

                    if (dt.Rows[0]["NM_FLOWSTATE"].ToString() == "0")
                        flag_chulgo = false;
                    else if (dt.Rows[0]["NM_FLOWSTATE"].ToString() == "1")
                        flag_chulgo = true;

                    if (!(dt.Rows[0]["ID_LOCKERCODE"].ToString() == "" || dt.Rows[0]["ID_LOCKERCODE"].ToString() == "0"))
                    {
                        flag_photoshop = true;
                        lockercode = dt.Rows[0]["ID_LOCKERCODE"].ToString();
                    }

                    //
                    if (flag_chulgo)
                    {
                        Search();
                        MessageBox.Show(new Form { TopMost = true }, "이미 출고되었습니다.", "파일 열기", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (flag_photoshop)
                    {
                        Search();
                        MessageBox.Show(new Form { TopMost = true }, "이미 작업중입니다. (작업자: " + Util.getUserName(lockercode) + ")", "파일 열기", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        string pubpart = "";
                        string pan = "";
                        string page = "";

                        Util.getPanPage(ref pubpart, ref pan, ref page, id);

                        // FLOWIMG 업데이트
                        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [DAPS2022].[dbo].[CMS_FLOWIMG] set 
                    ID_LOCKERCODE = '{0}', V_BEFORESTATE = '작업중({1})' where ID_FIM = '{2}'", empCode, empName, id)), "UPDATE");

                        // WORKHISTORY 인서트
                        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"insert into [DAPS2022].[dbo].[CMS_WORKHISTORY] 
                    (ID_USERCODE, D_WORKDATE, V_CONTENT, N_WORKKIND, N_WORKCODE, N_PUBPART, ID_FID, N_CONTENT_TYPE, C_APP_TYPE) values 
                    ('{0}', getdate(), '화상 ID : {1} ({2}면, {3}판, {4}부서코드)', '화상열기', '1001', '{5}', '{6}', '2', 'A')",
                        empCode, id, page, pan, pubpart, Util.getPubpartName(empCode), id)), "INSERT");

                        // 포토샵 열기
                        Process[] processList = Process.GetProcessesByName("Photoshop");

                        if (processList.Length == 0)
                        {
                            Type type = Type.GetTypeFromProgID("Photoshop.Application");
                            photoshop = (Photoshop.Application)Activator.CreateInstance(type, true);
                        }
                        else
                        {
                            // 포토샵 앞으로
                            ShowWindowAsync(processList[0].MainWindowHandle, 3);
                            SetForegroundWindow(processList[0].MainWindowHandle);
                        }

                        // 이미지 다운로드
                        if (!isJPG)
                        {
                            // Real 파일 → Real 폴더 (eps/psd/png)
                            using (WebClient wc = new WebClient())
                            {
                                wc.DownloadFileCompleted += OpenPhotoshop(realWorkFolderPath + "\\" + fileName + "." + ext);
                                wc.DownloadFileAsync(new Uri(string.Format("{0}/REAL/{1}/{2}/{3}/{4}", downloadUrl, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), fileName + "." + ext)), realWorkFolderPath + "\\" + fileName + "." + ext);
                            }
                        }

                        string ext_down = ".";

                        if (ext_o == "png" && ext == "png")
                            ext_down += ext;
                        else
                            ext_down += "jpg";

                        // Real 파일 → Origin 폴더 (jpg/png)
                        using (WebClient wc = new WebClient())
                        {
                            wc.DownloadFileAsync(new Uri(string.Format("{0}/REAL/{1}/{2}/{3}/{4}", downloadUrl, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), fileName + ext_down)), originWorkFolderPath + "\\" + fileName + ext_down);
                        }

                        // Real 파일 → Real 폴더 (jpg/png)
                        using (WebClient wc = new WebClient())
                        {
                            if (isJPG)
                                wc.DownloadFileCompleted += OpenPhotoshop(realWorkFolderPath + "\\" + fileName + "." + ext);

                            wc.DownloadFileAsync(new Uri(string.Format("{0}/REAL/{1}/{2}/{3}/{4}", downloadUrl, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), fileName + ext_down)), realWorkFolderPath + "\\" + fileName + ext_down);
                        }

                        // Prev 파일 → Prev 폴더 (jpg/png)
                        using (WebClient wc = new WebClient())
                        {
                            wc.DownloadFileCompleted += OpenWorkForm(fileName, ext, ext_o);
                            wc.DownloadFileAsync(new Uri(string.Format("{0}/PREV/{1}/{2}/{3}/{4}", downloadUrl, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), fileName + ext_down)), prevWorkFolderPath + "\\" + fileName + ext_down);
                        }

                        // Thumb 파일 → Thumb 폴더 (jpg/png)
                        using (WebClient wc = new WebClient())
                        {
                            wc.DownloadFileAsync(new Uri(string.Format("{0}/THUMB/{1}/{2}/{3}/{4}", downloadUrl, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), fileName + ext_down)), thumbWorkFolderPath + "\\" + fileName + ext_down);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(new Form { TopMost = true }, "파일 열기 중 오류가 발생했습니다. IT개발팀으로 문의 바랍니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Util.SaveLog("Image Open Fail: 파일 ID: " + id + "\n" + ex);
                }
            }
        }

        private AsyncCompletedEventHandler OpenPhotoshop(string filePath)
        {
            Action<object, AsyncCompletedEventArgs> action = (sender, e) =>
            {
                try
                {
                    photoshop.Open(filePath);
                }
                catch (COMException)
                {
                }
            };

            return new AsyncCompletedEventHandler(action);
        }

        // 작업창 열기
        private AsyncCompletedEventHandler OpenWorkForm(string fileName, string ext, string ext_o)
        {
            Action<object, AsyncCompletedEventArgs> action = (sender, e) =>
            {
                Form[] workFormList = Application.OpenForms.Cast<Form>().Where(x => x.Name == "WorkForm").ToArray();

                // 꼬마창 열려있으면 다시 로드
                if (workFormList.Length != 0)
                {
                    deleteWorkFile(eventWorkFolderPath);
                    workForm.SetImage(fileName, "");
                }
                else
                {
                    Util.Delay(1000);

                    workForm = new WorkForm(this, fileName, ext, ext_o, fontDialog);
                    workForm.Show();
                }
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

        // 검색일 기준 변경 - 게재일/등록일
        private void date_RB1_CheckedChanged(object sender, EventArgs e)
        {
            if (!isInitBtnClicked)
                Search();
        }

        // 검색 시작일 변경
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

        // 검색 종료일 변경
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
            if (!isInitBtnClicked)
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
                return;

            string sql = GetSearchSql();

            if (sql != "")
            {
                SetListView(sql);
                SetGridView(sql);
                SetGridView2();

                bgWorker.RunWorkerAsync();

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

                pageSelectedIdx = page_CB.SelectedIndex;
            }
            else
            {
                return;
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

                SetGridView2();

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

                SetGridView2();

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

        // 프리뷰 보기
        private void ShowPreview()
        {
            if (imgListView.SelectedItems.Count > 0)
            {
                ListViewItem item = imgListView.SelectedItems[0];
                string date = item.SubItems[IDX_BEFOREPUBDATE].Text;
                string id = item.SubItems[IDX_ID].Text;
                string ext = item.SubItems[IDX_EXTENSION].Text;
                string ext_o = item.SubItems[IDX_EXTENSION_ORG].Text;
                string fileName = item.SubItems[IDX_OFILENAME].Text;

                if (ext_o == "png" && ext == "png")
                {
                    imgPreView.Image = ViewWebImage(string.Format("{0}/PREV/{1}/{2}/{3}/{4}", downloadUrl, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), fileName + "." + ext), id);
                }
                else
                {
                    imgPreView.Image = ViewWebImage(string.Format("{0}/PREV/{1}/{2}/{3}/{4}", downloadUrl, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), fileName + ".jpg"), id);
                }

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

        // 프리뷰 초기화
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
                case Keys.Delete:
                    Delete();
                    bHandled = true;
                    break;
            }

            return bHandled;
        }

        // 검색 초기화 버튼 클릭
        private void init_BTN_Click(object sender, EventArgs e)
        {
            isInitBtnClicked = true;
            Init();
            isInitBtnClicked = false;
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
            WriteValue("THEME", "IDX_THEME", IDX_THEME.ToString());
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
            WriteValue("SPLITTER", "COL_HEADER2", imgGridView2.ColumnHeadersHeight.ToString());

            // 컬럼 헤더 크기
            for (int i = 0; i < imgGridView.Columns.Count; i++)
                WriteValue("SPLITTER", "COL" + i, imgGridView.Columns[i].Width.ToString());

            for (int i = 0; i < imgGridView2.Columns.Count; i++)
                WriteValue("SPLITTER", "COL" + (i + 20), imgGridView2.Columns[i].Width.ToString());
        }

        // 작업 파일 삭제
        public static void deleteWorkFile(string folderPath)
        {
            foreach (string file in Directory.GetFiles(folderPath))
            {
                FileInfo fi = new FileInfo(file);

                if (fi.Name.StartsWith("zz-") && fi.CreationTime.Date < DateTime.Today.Date)
                    fi.Delete(); // '만든 날짜' 어제 날짜까지 다 지우기

                if (fi.Name.EndsWith(".evt"))
                    fi.Delete(); // 다 지우기
            }
        }

        // 글꼴 설정 버튼 클릭
        private void settings_menu1_Click(object sender, EventArgs e)
        {
            try
            {
                if (fontDialog.ShowDialog() == DialogResult.OK)
                    SetFont(sender, e);
            }
            catch (ArgumentException)
            {
                MessageBox.Show(new Form { TopMost = true }, "지원되는 글꼴이 아닙니다.", "글꼴 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // 글꼴 세팅
        private void SetFont(object sender, EventArgs e)
        {
            imgListView.Font = fontDialog.Font;
            imgListView.ForeColor = fontDialog.Color;

            imgGridView.Font = fontDialog.Font;
            imgGridView2.Font = fontDialog.Font;
            imgGridView.ForeColor = fontDialog.Color;
            imgGridView2.ForeColor = fontDialog.Color;

            imgGridView.AlternatingRowsDefaultCellStyle.Font = fontDialog.Font;
            imgGridView2.AlternatingRowsDefaultCellStyle.Font = fontDialog.Font;
            imgGridView.AlternatingRowsDefaultCellStyle.ForeColor = fontDialog.Color;
            imgGridView2.AlternatingRowsDefaultCellStyle.ForeColor = fontDialog.Color;

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
            DownloadFile("THUMB");
        }

        // Prev 저장 버튼 클릭
        private void save_prev_BTN_Click(object sender, EventArgs e)
        {
            DownloadFile("PREV");
        }

        // Real 저장 버튼 클릭
        private void save_real_BTN_Click(object sender, EventArgs e)
        {
            DownloadFile("REAL");
        }

        // 이미지 다운로드
        private void DownloadFile(string kind)
        {
            if (imgListView.SelectedItems.Count > 0)
            {
                ListViewItem item = imgListView.SelectedItems[0];
                string date = item.SubItems[IDX_BEFOREPUBDATE].Text;
                string id = item.SubItems[IDX_ID].Text;
                string ext = item.SubItems[IDX_EXTENSION].Text;
                string ext_o = item.SubItems[IDX_EXTENSION_ORG].Text;
                string fileName = item.SubItems[IDX_OFILENAME].Text;

                string saveFileName = "";
                string fileFilter = "";

                // 저장 폴더, 확장자, 파일명 세팅
                if (kind == "REAL")
                {
                    saveFileDialog.InitialDirectory = realDownloadFolderPath;

                    if (ext == "jpg")
                        fileFilter = "Jpg Files (*.jpg, *.JPG)|*.jpg;*.JPG";
                    else if (ext == "eps")
                        fileFilter = "EPS (*.eps, *.EPS)|*.eps;*.EPS";
                    else if (ext == "psd")
                        fileFilter = "Photoshop (*.psd, *.PSD)|*.psd;*.PSD";
                    else if (ext == "png")
                        fileFilter = "PNG (*.png, *.PNG)|*.png;*.PNG";

                    saveFileName = fileName + "_R." + ext;
                }
                else if (kind == "PREV")
                {
                    saveFileDialog.InitialDirectory = prevDownloadFolderPath;

                    if (ext_o == "png" && ext == "png")
                    {
                        fileFilter = "PNG (*.png, *.PNG)|*.png;*.PNG";
                        saveFileName = fileName + "_P.png";
                    }
                    else
                    {
                        fileFilter = "Jpg Files (*.jpg, *.JPG)|*.jpg;*.JPG";
                        saveFileName = fileName + "_P.jpg";
                    }
                }
                else if (kind == "THUMB")
                {
                    saveFileDialog.InitialDirectory = thumbDownloadFolderPath;

                    if (ext_o == "png" && ext == "png")
                    {
                        fileFilter = "PNG (*.png, *.PNG)|*.png;*.PNG";
                        saveFileName = fileName + "_T.png";
                    }
                    else
                    {
                        fileFilter = "Jpg Files (*.jpg, *.JPG)|*.jpg;*.JPG";
                        saveFileName = fileName + "_T.jpg";
                    }
                }

                // 파일 저장
                try
                {
                    saveFileDialog.FileName = saveFileName;
                    saveFileDialog.Filter = fileFilter;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        using (WebClient wc = new WebClient())
                        {
                            if (kind == "REAL")
                            {
                                wc.DownloadFileAsync(new Uri(string.Format("{0}/{1}/{2}/{3}/{4}/{5}", downloadUrl, kind, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), fileName + "." + ext)), saveFileDialog.FileName);
                            }
                            else
                            {
                                if (ext_o == "png" && ext == "png")
                                {
                                    wc.DownloadFileAsync(new Uri(string.Format("{0}/{1}/{2}/{3}/{4}/{5}", downloadUrl, kind, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), fileName + "." + ext)), saveFileDialog.FileName);
                                }
                                else
                                {
                                    wc.DownloadFileAsync(new Uri(string.Format("{0}/{1}/{2}/{3}/{4}/{5}", downloadUrl, kind, date.Substring(0, 4), date.Substring(5, 2), date.Substring(8, 2), fileName + ".jpg")), saveFileDialog.FileName);
                                }
                            }
                        }

                        MessageBox.Show(new Form { TopMost = true }, "저장되었습니다.", "파일 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Util.SaveLog("File Save Complete: 파일명: " + saveFileDialog.FileName);

                        if (kind == "REAL")
                            realDownloadFolderPath = Path.GetDirectoryName(saveFileDialog.FileName);
                        else if (kind == "PREV")
                            prevDownloadFolderPath = Path.GetDirectoryName(saveFileDialog.FileName);
                        else if (kind == "THUMB")
                            thumbDownloadFolderPath = Path.GetDirectoryName(saveFileDialog.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(new Form { TopMost = true }, "파일 저장 중 오류가 발생했습니다. IT개발팀으로 문의 바랍니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Util.SaveLog("File Save Fail: 파일명: " + saveFileDialog.FileName + ", action: " + kind + "\n" + ex);
                }
            }
        }

        // 로그아웃 버튼 클릭
        private void logout_BTN_Click(object sender, EventArgs e)
        {
            hideTreeMyun(sender, e);

            var confirmResult = MessageBox.Show(new Form { TopMost = true }, "로그아웃 완료.\n\n프로그램을 재실행하시겠습니까?", "로그아웃", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

            if (confirmResult == DialogResult.Yes)
            {
                Util.SaveLog("Logout: " + empNo + ", Restart");
                Application.Restart();
            }
            else
            {
                Util.SaveLog("Logout: " + empNo + ", Exit");
                Application.Exit();
            }
        }

        // 배경 설정 메뉴 클릭
        private void settings_menu2_1_Click(object sender, EventArgs e)
        {
            SetThemeBG(0, true);
        }

        private void settings_menu2_2_Click(object sender, EventArgs e)
        {
            SetThemeBG(1, true);
        }

        private void settings_menu2_3_Click(object sender, EventArgs e)
        {
            SetThemeBG(2, true);
        }

        // 배경 세팅 
        private void SetThemeBG(int index, bool refresh)
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

            if (refresh)
                Search();
        }

        // 아이콘 설정 메뉴 클릭
        private void settings_menu3_1_Click(object sender, EventArgs e)
        {
            SetThemeIcon(0, true);
        }

        private void settings_menu3_2_Click(object sender, EventArgs e)
        {
            SetThemeIcon(1, true);
        }

        private void settings_menu3_3_Click(object sender, EventArgs e)
        {
            SetThemeIcon(2, true);
        }

        // 아이콘 세팅
        private void SetThemeIcon(int index, bool refresh)
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

            if (refresh)
                Search();
        }

        // 전표 보기 버튼 클릭
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

        // 파일 복사 메뉴 클릭 - 일반
        private void ToolStripMenuItem1_1_Click(object sender, EventArgs e)
        {
            if (imgListView.SelectedItems.Count > 0)
            {
                DialogResult confirmResult = MessageBox.Show(new Form { TopMost = true }, "일반 복사하시겠습니까?", "파일 복사", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (confirmResult == DialogResult.Yes)
                    CopyFile("S");
            }
        }

        // 파일 복사 메뉴 클릭 - 원화상
        private void ToolStripMenuItem1_2_Click(object sender, EventArgs e)
        {
            if (imgListView.SelectedItems.Count > 0)
            {
                DialogResult confirmResult = MessageBox.Show(new Form { TopMost = true }, "원화상 복사하시겠습니까?", "파일 복사", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (confirmResult == DialogResult.Yes)
                    CopyFile("R");
            }
        }

        // 파일 복사
        private void CopyFile(string type)
        {
            ListViewItem item = imgListView.SelectedItems[0];
            string id = item.SubItems[IDX_ID].Text;
            string copyPath = copyUrl + "?TYPE=" + type + "&ID=" + id;
            // Ex) ctssvr1.seoul.co.kr/PhotoMgr/filecopy.aspx?TYPE=S&ID=10000249   

            WebRequest request = WebRequest.Create(copyPath);
            request.Method = "GET";

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            string result = reader.ReadToEnd();

            if (result == "S")
            {
                string pubpart = "";
                string pan = "";
                string page = "";

                Util.getPanPage(ref pubpart, ref pan, ref page, id);

                // WORKHISTORY 인서트
                dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"insert into [DAPS2022].[dbo].[CMS_WORKHISTORY] 
                    (ID_USERCODE, D_WORKDATE, V_CONTENT, N_WORKKIND, N_WORKCODE, N_PUBPART, ID_FID, N_CONTENT_TYPE, C_APP_TYPE) values 
                    ('{0}', getdate(), '화상 ID : {1} ({2}면, {3}판, {4}부서코드)', '화상복사', '1001', '{5}', '{6}', '2', 'A')",
                empCode, id, page, pan, pubpart, Util.getPubpartName(empCode), id)), "INSERT");

                MessageBox.Show(new Form { TopMost = true }, "파일이 복사되었습니다.", "파일 복사", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Util.SaveLog("File Copy Complete: " + copyPath);
            }
            else
            {
                MessageBox.Show(new Form { TopMost = true }, "파일 복사 중 오류가 발생했습니다. IT개발팀으로 문의 바랍니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.SaveLog("File Copy Fail: " + copyPath);
            }

            reader.Close();
            stream.Close();
            response.Close();

            Search();
        }

        // 폴더 설정 메뉴 클릭
        private void settings_menu4_Click(object sender, EventArgs e)
        {
            FolderSetForm folderSetForm = new FolderSetForm(fontDialog);
            folderSetForm.ShowDialog();
        }

        // 잠금 해제 메뉴 클릭
        private void ToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (imgListView.SelectedItems.Count > 0)
            {
                DialogResult confirmResult = MessageBox.Show(new Form { TopMost = true }, "잠금을 해제하시겠습니까?", "잠금 해제", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (confirmResult == DialogResult.Yes)
                {
                    string id = imgListView.SelectedItems[0].SubItems[IDX_ID].Text;

                    try
                    {
                        // FLOWIMG 업데이트
                        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [DAPS2022].[dbo].[CMS_FLOWIMG] set 
                    ID_LOCKERCODE = NULL, V_BEFORESTATE = ''
                    where ID_FIM = '{0}'", id)), "UPDATE");

                        Search();
                        MessageBox.Show(new Form { TopMost = true }, "잠금이 해제되었습니다.", "잠금 해제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(new Form { TopMost = true }, "잠금 해제 중 오류가 발생했습니다. IT개발팀으로 문의 바랍니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Util.SaveLog("Image Unlock Fail: 파일 ID: " + id + "\n" + ex);
                    }
                }
            }
        }

        // 파일 삭제 메뉴 클릭
        private void ToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (imgListView.SelectedItems.Count > 0)
            {
                Delete();
            }
        }

        // 파일 삭제
        private void Delete()
        {
            ListViewItem item = imgListView.SelectedItems[0];
            string id = item.SubItems[IDX_ID].Text;

            try
            {
                // 출고 상태 확인
                dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select NM_FLOWSTATE from [DAPS2022].[dbo].[CMS_FLOWIMG]
                    where ID_FIM = '{0}'", id)), "SELECT");

                if (dt.Rows[0].ItemArray[0].ToString() == "1")
                {
                    Search();
                    MessageBox.Show(new Form { TopMost = true }, "이미 출고된 파일은 삭제할 수 없습니다.", "파일 삭제", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    DialogResult confirmResult = MessageBox.Show(new Form { TopMost = true }, "파일을 삭제하시겠습니까?", "파일 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                    if (confirmResult == DialogResult.Yes)
                    {
                        string pubpart = "";
                        string pan = "";
                        string page = "";

                        Util.getPanPage(ref pubpart, ref pan, ref page, id);

                        // FLOWIMG 업데이트
                        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [DAPS2022].[dbo].[CMS_FLOWIMG] set 
                        B_DELETE = '1' where ID_FIM = '{0}' and NM_FLOWSTATE = '0'", id)), "UPDATE");

                        // WORKHISTORY 인서트
                        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"insert into [DAPS2022].[dbo].[CMS_WORKHISTORY] 
                        (ID_USERCODE, D_WORKDATE, V_CONTENT, N_WORKKIND, N_WORKCODE, N_PUBPART, ID_FID, N_CONTENT_TYPE, C_APP_TYPE) values 
                        ('{0}', getdate(), '화상 ID : {1} ({2}면, {3}판, 부서코드{4})', '화상삭제', '1001', '{5}', '{6}', '2', 'A')",
                        empCode, id, page, pan, pubpart, pubpart, id)), "INSERT");

                        Search();
                        MessageBox.Show(new Form { TopMost = true }, "파일이 삭제되었습니다.", "파일 삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(new Form { TopMost = true }, "파일 삭제 중 오류가 발생했습니다. IT개발팀으로 문의 바랍니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.SaveLog("Image Delete Fail: 파일 ID: " + id + "\n" + ex);
            }
        }

        // 그리드뷰 탭 변경
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGridView2();
        }

        // 테마 설정 메뉴 클릭
        private void SetTheme(int index, Color[] color, bool refresh)
        {
            IDX_THEME = index;

            ToolStripItemCollection items = settings_menu5.DropDownItems;

            for (int i = 0; i < items.Count; i++)
            {
                if (i == index)
                    ((ToolStripMenuItem)items[i]).Checked = true;
                else
                    ((ToolStripMenuItem)items[i]).Checked = false;
            }

            toolStrip.BackColor = color[0];
            splitContainer1.Panel1.BackColor = color[0];
            imgGridView.ColumnHeadersDefaultCellStyle.BackColor = color[0];
            imgGridView2.ColumnHeadersDefaultCellStyle.BackColor = color[0];
            toolStripStatusLabel1.BackColor = color[0];
            toolStripStatusLabel2.BackColor = color[0];

            imgListView.BackColor = color[1];
            imgPreView.BackColor = color[1];
            splitContainer4.Panel2.BackColor = color[1];
            splitContainer3.Panel2.BackColor = color[1];
            splitContainer2.Panel1.BackColor = color[1];
            imgGridView.DefaultCellStyle.BackColor = color[1];
            imgGridView2.DefaultCellStyle.BackColor = color[1];
            imgGridView.BackgroundColor = color[1];
            imgGridView2.BackgroundColor = color[1];

            splitContainer2.BackColor = color[2];
            splitContainer3.BackColor = color[2];
            splitContainer4.BackColor = color[2];
            filename_TB.BackColor = color[2];
            title_TB.BackColor = color[2];
            caption_TB.BackColor = color[2];
            retouch_TB.BackColor = color[2];
            imgGridView.AlternatingRowsDefaultCellStyle.BackColor = color[2];
            imgGridView2.AlternatingRowsDefaultCellStyle.BackColor = color[2];
            imgGridView.GridColor = color[2];
            imgGridView2.GridColor = color[2];
            init_BTN.FlatAppearance.BorderColor = color[2];
            init_BTN.FlatAppearance.MouseDownBackColor = color[2];
            init_BTN.FlatAppearance.MouseOverBackColor = color[2];
            save_real_BTN.FlatAppearance.BorderColor = color[2];
            save_real_BTN.FlatAppearance.MouseDownBackColor = color[2];
            save_real_BTN.FlatAppearance.MouseOverBackColor = color[2];
            save_prev_BTN.FlatAppearance.BorderColor = color[2];
            save_prev_BTN.FlatAppearance.MouseDownBackColor = color[2];
            save_prev_BTN.FlatAppearance.MouseOverBackColor = color[2];
            save_thumb_BTN.FlatAppearance.BorderColor = color[2];
            save_thumb_BTN.FlatAppearance.MouseDownBackColor = color[2];
            save_thumb_BTN.FlatAppearance.MouseOverBackColor = color[2];
            open_BTN.FlatAppearance.BorderColor = color[2];
            open_BTN.FlatAppearance.MouseDownBackColor = color[2];
            open_BTN.FlatAppearance.MouseOverBackColor = color[2];
            print_prev_BTN.FlatAppearance.BorderColor = color[2];
            print_prev_BTN.FlatAppearance.MouseDownBackColor = color[2];
            print_prev_BTN.FlatAppearance.MouseOverBackColor = color[2];

            init_BTN.BackColor = color[3];
            save_real_BTN.BackColor = color[3];
            save_prev_BTN.BackColor = color[3];
            save_thumb_BTN.BackColor = color[3];
            open_BTN.BackColor = color[3];
            print_prev_BTN.BackColor = color[3];

            date_RB1.ForeColor = color[4];
            date_RB2.ForeColor = color[4];
            label5.ForeColor = color[4];
            label1.ForeColor = color[4];
            label2.ForeColor = color[4];
            label3.ForeColor = color[4];
            label6.ForeColor = color[4];
            label4.ForeColor = color[4];
            label7.ForeColor = color[4];
            label8.ForeColor = color[4];
            init_BTN.ForeColor = color[4];
            save_real_BTN.ForeColor = color[4];
            save_prev_BTN.ForeColor = color[4];
            save_thumb_BTN.ForeColor = color[4];
            open_BTN.ForeColor = color[4];
            print_prev_BTN.ForeColor = color[4];
            settings_BTN.ForeColor = color[4];
            refresh_BTN.ForeColor = color[4];
            logout_BTN.ForeColor = color[4];
            toolStripStatusLabel1.ForeColor = color[4];
            toolStripStatusLabel2.ForeColor = color[4];
            imgGridView.ColumnHeadersDefaultCellStyle.ForeColor = color[4];
            imgGridView2.ColumnHeadersDefaultCellStyle.ForeColor = color[4];

            if (index == IDX_THEME_DARK1 || index == IDX_THEME_DARK2 || index == IDX_THEME_BLUE)
                fontDialog.Color = Color.White;
            else if (index == IDX_THEME_LIGHT1 || index == IDX_THEME_LIGHT2)
                fontDialog.Color = Color.Black;

            SetFont(null, null);

            if (refresh)
                Search();
        }

        private void settings_menu5_1_Click(object sender, EventArgs e)
        {
            SetTheme(IDX_THEME_DARK1, themeDark1, true);
        }

        private void settings_menu5_2_Click(object sender, EventArgs e)
        {
            SetTheme(IDX_THEME_DARK2, themeDark2, true);
        }

        private void settings_menu5_3_Click(object sender, EventArgs e)
        {
            SetTheme(IDX_THEME_LIGHT1, themeLight1, true);
        }

        private void settings_menu5_4_Click(object sender, EventArgs e)
        {
            SetTheme(IDX_THEME_LIGHT2, themeLight2, true);
        }

        private void settings_menu5_5_Click(object sender, EventArgs e)
        {
            SetTheme(IDX_THEME_BLUE, themeBlue, true);
        }
    }
}
