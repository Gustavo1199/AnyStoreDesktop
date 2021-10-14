using AnyStore.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.DAL
{
    class loginDAL
    {
        //Static String to Connect Database
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        public string loginCheck(loginBLL l)
        {
            //Create a boolean variable and set its value to false and return it
            string rol = "";

            ////Connecting To DAtabase
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //SQL Query to check login
                string sql = "SELECT user_type FROM tbl_users WHERE username=@username AND password=@password";

                //    //Creating SQL Command to pass value
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@username", l.username);
                cmd.Parameters.AddWithValue("@password", l.password);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                adapter.Fill(dt);

                //Checking The rows in DataTable 
                if (dt.Rows.Count>0)
                {
                    //Login Sucessful
                    rol = dt.Rows[0]["user_type"].ToString();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return rol;
        }
    }
}
