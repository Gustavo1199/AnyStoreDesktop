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
    class transactionDetailDAL
    {
        //Create Connection String
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        #region Insert Method for Transaction Detail
        public bool InsertTransactionDetail(transactionDetailBLL td)
        {
            //Create a boolean value and set its default value to false
            bool isSuccess = false;

            //Create a database connection here
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Sql Query to Insert Transaction detais
                string sql = "INSERT INTO tbl_transaction_detail (product_id, rate, qty, total, dea_cust_id, added_date, added_by, transaction_id) VALUES (@product_id, @rate, @qty, @total, @dea_cust_id, @added_date, @added_by, @transaction_id)";

                //Passing the value to the SQL Query
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Passing the values using cmd
                cmd.Parameters.AddWithValue("@product_id", td.product_id);
                cmd.Parameters.AddWithValue("@rate", td.rate);
                cmd.Parameters.AddWithValue("@qty", td.qty);
                cmd.Parameters.AddWithValue("@total", td.total);
                cmd.Parameters.AddWithValue("@dea_cust_id", td.dea_cust_id);
                cmd.Parameters.AddWithValue("@added_date", td.added_date);
                cmd.Parameters.AddWithValue("@added_by", td.added_by);
                cmd.Parameters.AddWithValue("@transaction_id", td.transactionID);

                //Open Database connection
                conn.Open();

                //declare the int variable and execute the query
                int rows = cmd.ExecuteNonQuery();

                if(rows>0)
                {
                    //Query Executed Successfully
                    isSuccess = true;
                }
                else
                {
                    //FAiled to Execute Query
                    isSuccess = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close Database Connection
                conn.Close();
            }
            return isSuccess;
        }
        #endregion

        #region Get Details
        internal DataTable GetDetailsFromTransaction(string id)
        {
            //Creating Database Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            DataTable dt = new DataTable();

            try
            {
                //Wrting SQL Query to get all the data from DAtabase
                string sql = "select id, product_id, (select name from tbl_products where id = product_id) as 'Nombre producto', rate as 'Precio por Unidad', qty as Cantidad, total as 'Precio total' from tbl_transaction_detail where transaction_id = '" + id +"'";

                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //Open DAtabase Connection
                conn.Open();
                //Adding the value from adapter to Data TAble dt
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        private DataTable UpdateProducts(int id, string tipo)
        {
            //Creating Database Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            DataTable dt = new DataTable();

            try
            {
                //Wrting SQL Query to get all the data from DAtabase
                string sql = "";
                if(tipo.ToUpper() == "COMPRA")
                    sql = $"UPDATE Tabla_A SET Tabla_A.qty = Tabla_A.qty - Tabla_B.qty FROM tbl_products AS Tabla_A INNER JOIN tbl_transaction_detail AS Tabla_B  ON Tabla_A.id = Tabla_B.product_id WHERE Tabla_B.transaction_id = {id}";
                else if(tipo.ToUpper() == "VENTA")
                    sql = $"UPDATE Tabla_A SET Tabla_A.qty = Tabla_A.qty + Tabla_B.qty FROM tbl_products AS Tabla_A INNER JOIN tbl_transaction_detail AS Tabla_B  ON Tabla_A.id = Tabla_B.product_id WHERE Tabla_B.transaction_id = {id}";

                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //Open DAtabase Connection
                conn.Open();
                //Adding the value from adapter to Data TAble dt
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        private DataTable UpdateStatus(int id)
        {
            //Creating Database Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            DataTable dt = new DataTable();

            try
            {
                //Wrting SQL Query to get all the data from DAtabase
                string sql = $"UPDATE tbl_transactions SET Status = 'CANCELADA' WHERE id = {id}";

                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //Open DAtabase Connection
                conn.Open();
                //Adding the value from adapter to Data TAble dt
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        #endregion

        #region Delete Detail
        public bool DeleteDetail(int productID)
        {
            //Create a Boolean variable and set its value to false
            bool isSuccess = false;

            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //SQL Query to Delete from Database
                string sql = "DELETE FROM tbl_transaction_detail WHERE id=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                //Passing the value using cmd
                cmd.Parameters.AddWithValue("@id", productID);

                //Open SqlConnection
                conn.Open();

                int rows = cmd.ExecuteNonQuery();

                //If the query is executd successfully then the value of rows will be greater than zero else it will be less than 0
                if (rows > 0)
                {
                    //Query Executed Successfully
                    isSuccess = true;
                }
                else
                {
                    //Faied to Execute Query
                    isSuccess = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return isSuccess;
        }
        #endregion


        #region GetDetailByID

        public transactionDetailBLL GetDetailByID(int id)
        {
            SqlConnection conn = new SqlConnection(myconnstrng);
            //First Create an Object of DeaCust BLL and REturn it
            transactionDetailBLL p = new transactionDetailBLL();

            //Data TAble to Holdthe data temporarily
            DataTable dt = new DataTable();

            try
            {
                //SQL Query to Get id based on Name
                string sql = "SELECT rate, qty FROM tbl_transaction_detail WHERE id = " + id;
                //Create the SQL Data Adapter to Execute the Query
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                conn.Open();

                //Passing the CAlue from Adapter to DAtatable
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    p.rate = decimal.Parse(dt.Rows[0]["rate"].ToString());
                    p.qty = decimal.Parse(dt.Rows[0]["qty"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return p;

        }

        #endregion



        #region UpdateDetail

        public bool UpdateDetail(transactionDetailBLL data)
        {
            //Create a Boolean Variable and Set its value to false
            bool success = false;

            //SQl Connection to Connect Database
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Write the SQL Query to Update Qty
                string sql = "UPDATE tbl_transaction_detail SET qty =  @qty, rate =@rate, total = @qty * @rate WHERE id=@id";

                //Create SQL Command to Pass the calue into Queyr
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Passing the VAlue trhough parameters
                cmd.Parameters.AddWithValue("@id", data.id);
                cmd.Parameters.AddWithValue("@qty", data.qty);
                cmd.Parameters.AddWithValue("@rate", data.rate);

                //Open Database Connection
                conn.Open();

                //Create Int Variable and Check whether the query is executed Successfully or not
                int rows = cmd.ExecuteNonQuery();
                //Lets check if the query is executed Successfully or not
                if (rows > 0)
                {
                    //Query Executed Successfully
                    success = true;
                }
                else
                {
                    //Failed to Execute Query
                    success = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return success;
        }

        #endregion
        internal void Update(int codigo, string tipo)
        {
            UpdateProducts(codigo, tipo);

            UpdateStatus(codigo);


        }

    }
}
