using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace ImageWork
{
    class Util
    {
        public Util()
        {
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

                sw.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + msg);
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
