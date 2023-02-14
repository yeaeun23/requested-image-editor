using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace ImageWork
{
    class Util
    {
        // 판 CB 리스트 세팅
        public static void SetPanCBList(ComboBox cb)
        {
            DataTable dt = ExecuteQuery(new SqlCommand(string.Format(@"select N_PAN_CODE from [DAPS2022].[dbo].[CMS_PAN] where id_mechae = 65 order by n_pan_code asc")), "SELECT");

            foreach (DataRow dr in dt.Rows)
                cb.Items.Add(dr["N_PAN_CODE"].ToString()); // "5"
        }

        // 사용자 코드로 이름 구하기
        public static string getUserName(string userCode)
        {
            DataTable dt = ExecuteQuery(new SqlCommand(string.Format(@"select V_USERNAME from [CMSCOM].[dbo].[T_USERINFO] where ID_USERCODE = '{0}'", userCode)), "SELECT");

            return dt.Rows[0]["V_USERNAME"].ToString();
        }

        // 사용자 코드로 부서코드 구하기
        public static string getPubpartName(string userCode)
        {
            DataTable dt = ExecuteQuery(new SqlCommand(string.Format(@"select ID_FOLDER from [CMSCOM].[dbo].[T_USERINFO] where ID_USERCODE = '{0}'", userCode)), "SELECT");

            return dt.Rows[0]["ID_FOLDER"].ToString();
        }

        // 부서코드, 판 면 구하기
        public static void getPanPage(ref string pubpart, ref string pan, ref string page, string id)
        {
            DataTable dt = ExecuteQuery(new SqlCommand(string.Format(@"select ID_PUBPART, N_PAN, N_PAGE from [DAPS2022].[dbo].[CMS_FLOWIMG] where ID_FIM = '{0}'", id)), "SELECT");

            pubpart = dt.Rows[0]["ID_PUBPART"].ToString();
            pan = dt.Rows[0]["N_PAN"].ToString();
            page = dt.Rows[0]["N_PAGE"].ToString();
        }

        // 상태표시줄 세팅
        public static void SetStatusLabel(ToolStripStatusLabel toolStrip, string msg)
        {
            toolStrip.Text = DateTime.Now.ToString("(HH:mm:ss) ") + msg;
        }

        // 딜레이
        public static DateTime Delay(int ms)
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

        public static DataTable ExecuteQuery(SqlCommand cmd, string action)
        {
            string conString = @"";

            using (SqlConnection con = new SqlConnection(conString))
            {
                cmd.Connection = con;

                switch (action)
                {
                    case "SELECT":
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            sda.SelectCommand = cmd;

                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                return dt;
                            }
                        }
                    case "UPDATE":
                    case "INSERT":
                    case "DELETE":
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        break;
                }

                return null;
            }
        }

        public static void SaveLog(string msg)
        {
            FileStream fo = null;
            StreamWriter sw = null;

            try
            {
                if (Directory.Exists(Application.StartupPath + "\\log") == false)
                    Directory.CreateDirectory(Application.StartupPath + "\\log");

                string strFileName = Application.StartupPath + "\\log\\ImageWork_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                fo = new FileStream(strFileName, FileMode.Append);
                sw = new StreamWriter(fo);

                sw.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff] ") + msg);
                sw.Close();
                fo.Close();
            }
            catch (Exception)
            {
                if (sw != null)
                    sw.Close();
                if (fo != null)
                    fo.Close();
            }
        }
    }
}
