using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace GetAuthorizePicture
{
    class Program
    {
        private static string CS = Properties.Settings.Default.Connectionstring.ToString();
        private static string QueryUnauthorize = Properties.Settings.Default.QueryUnauthorize.ToString();
        private static string distpath = Properties.Settings.Default.DestinPath.ToString();

        static void Main(string[] args)
        {
            DataTable PicturePathTable = new DataTable();
            PicturePathTable = PictureTable();
            string SourcePath = "";

            foreach (DataRow dr in PicturePathTable.Rows)
            {
                SourcePath = Convert.ToString(dr["OutPic"]);

                string destinationPath = Path.Combine(distpath, Path.GetFileName(SourcePath));
                Console.WriteLine(Convert.ToString(dr["OutPic"]));
                CopyFile(SourcePath, destinationPath);
               
            }
            Console.WriteLine("Finish");
            Console.ReadLine();
        }

        private static DataTable PictureTable()
        {
            DataTable table = new DataTable();
            using (SqlConnection cn = new SqlConnection(CS))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(QueryUnauthorize, cn);
                    cmd.CommandType = CommandType.Text;

                    table.Load(cmd.ExecuteReader());
                    cmd.Dispose();
                    cmd = null;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    cn.Close();
                }
            }
            return table;

        }

        public static void CopyFile(string currentFile, string distinFile)
        {
            try
            {
                if (Directory.Exists(distpath))
                {
                    if (File.Exists(currentFile))
                    {
                        File.Copy(currentFile, distinFile, true);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
                
            }
        }
    }
}
