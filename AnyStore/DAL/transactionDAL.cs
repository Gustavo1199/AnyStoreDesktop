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
    class transactionDAL
    {
        //Create a connection string variable
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        #region Insert Transaction Method
        public bool Insert_Transaction(transactionsBLL t, out int transactionID)
        {
            //Create a boolean value and set its default value to false
            bool isSuccess = false;
            //Set the out transactionID value to negative 1 i.e. -1
            transactionID = -1;
            //Create a SqlConnection first
            SqlConnection conn = new SqlConnection(myconnstrng);
            try
            {
                //SQL Query to Insert Transactions
                string sql = "INSERT INTO tbl_transactions (type, dea_cust_id, grandTotal, transaction_date, tax, discount, added_by,Status) VALUES (@type, @dea_cust_id, @grandTotal, @transaction_date, @tax, @discount, @added_by,'VIGENTE'); SELECT @@IDENTITY;";

                //Sql Commandto pass the value in sql query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Passing the value to sql query using cmd
                cmd.Parameters.AddWithValue("@type", t.type);
                cmd.Parameters.AddWithValue("@dea_cust_id", t.dea_cust_id);
                cmd.Parameters.AddWithValue("@grandTotal", t.grandTotal);
                cmd.Parameters.AddWithValue("@transaction_date", t.transaction_date);
                cmd.Parameters.AddWithValue("@tax", t.tax);
                cmd.Parameters.AddWithValue("@discount", t.discount);
                cmd.Parameters.AddWithValue("@added_by", t.added_by);

                //Open Database Connection
                conn.Open();

                //Execute the Query
                object o = cmd.ExecuteScalar();

                //If the query is executed successfully then the value will not be null else it will be null
                if(o!=null)
                {
                    //Query Executed Successfully
                    transactionID = int.Parse(o.ToString());
                    isSuccess = true;
                }
                else
                {
                    //failed to execute query
                    isSuccess = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close the connection 
                conn.Close();
            }

            return isSuccess;
        }
        #endregion
        #region METHOD TO DISPLAY ALL THE TRANSACTION
        public DataTable DisplayAllTransactions()
        {
            //SQlConnection First
            SqlConnection conn = new SqlConnection(myconnstrng);

            //Create a DAta Table to hold the datafrom database temporarily
            DataTable dt = new DataTable();

            try
            {
                //Write the SQL Query to Display all Transactions
                string sql = "SELECT id as Código, type as Tipo, dea_cust_id as 'Cliente/Proveedor', grandTotal as Monto, transaction_date as 'Fecha Transacción', tax as Impuesto, discount as Descuento, a.Status as Estatus,  (select username from tbl_users b where a.added_by = b.id) as 'Usuario Ultima Actualización' FROM tbl_transactions a";

                //SqlCommand to Execute Query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //SqlDataAdapter to Hold the data from database
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open DAtabase Connection
                conn.Open();

                adapter.Fill(dt);
            }
            catch(Exception ex)
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
        // no se usa
        #region METHOD TO DISPLAY TRANSACTION BASED ON TRANSACTION TYPE
        public DataTable DisplayTransactionByType(string type)
        {
            //Create SQL Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            //Create a DataTable
            DataTable dt = new DataTable();

            try
            {
                //Write SQL Query
                string sql = "SELECT * FROM tbl_transactions WHERE type='"+type+"'";

                //SQL Command to Execute Query
                SqlCommand cmd = new SqlCommand(sql, conn);
                //SQlDataAdapter to hold the data from database
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open DAtabase Connection
                conn.Open();
                adapter.Fill(dt);
            }
            catch(Exception ex)
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

        #region DisplayTotalTransactions
        public string GetTotalTransaccions()
        {
            //SQlConnection First
            SqlConnection conn = new SqlConnection(myconnstrng);
            string res = "";
            try
            {
                //Write the SQL Query to Display all Transactions
                string sql = "SELECT sum(grandTotal) FROM tbl_transactions WHERE STATUS = 'VIGENTE'";

                //SqlCommand to Execute Query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //SqlDataAdapter to Hold the data from database
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open DAtabase Connection
                conn.Open();

                res = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return res;
        }

        public string DisplaySumTransactionByDate(string year, string month,string day, string type)
        {
            //Create SQL Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            //Create a DataTable
            DataTable dt = new DataTable();

            string res = "";
            try
            {

                //Write the SQL Query to Display all Transactions
                string sql = "SELECT sum(grandTotal) FROM tbl_transactions where year(transaction_date) = IIF('" + year + "' = '', year(transaction_date), '" + year + "') AND month(transaction_date) = IIF('" + month + "' = '', month(transaction_date), '" + month + "') AND day(transaction_date) = IIF('" + day + "' = '', day(transaction_date), '" + day + "') AND type=IIF('" + type + "' = '', type, '" + type + "') AND STATUS = 'VIGENTE'";

                //SqlCommand to Execute Query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //SqlDataAdapter to Hold the data from database
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open DAtabase Connection
                conn.Open();

                res = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return res;
        }
        #endregion

        #region Display Transaction by Date
        public DataTable DisplayTransactionByDate(string year, string month, string day, string type)
        {
            //Create SQL Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            //Create a DataTable
            DataTable dt = new DataTable();

            try
            {
                
                //Write SQL Query
                string sql = "SELECT id as Código, type as Tipo, dea_cust_id as 'Cliente/Proveedor', grandTotal as Monto, transaction_date as 'Fecha Transacción', tax as Impuesto, discount as Descuento, a.Status as Estatus,  (select username from tbl_users b where a.added_by = b.id) as 'Usuario Ultima Actualización'  FROM tbl_transactions a WHERE year(transaction_date) = IIF('" + year + "' = '', year(transaction_date), '" + year + "') AND month(transaction_date) = IIF('" + month + "' = '', month(transaction_date), '" + month + "') AND day(transaction_date) = IIF('" + day + "' = '', day(transaction_date), '" + day + "') AND type=IIF('" + type + "' = '', type, '" + type + "')";

                //SQL Command to Execute Query
                SqlCommand cmd = new SqlCommand(sql, conn);
                //SQlDataAdapter to hold the data from database
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open DAtabase Connection
                conn.Open();
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

        #region GetTransactionID
        public string GetTransactionID(DateTime transaction_date, int added_by)
        {
            //Create SQL Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            //Create a DataTable
            DataTable dt = new DataTable();

            string res = "";
            try
            {

                //Write the SQL Query to Display all Transactions
                string sql = "SELECT id FROM tbl_transactions where transaction_date = @tdate and added_by = @user";

                //SqlCommand to Execute Query
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@tdate", transaction_date);
                cmd.Parameters.AddWithValue("@user", added_by);

                //SqlDataAdapter to Hold the data from database
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open DAtabase Connection
                conn.Open();

                res = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return res;
        }
        #endregion


        #region UpdateHeaderTotal
        public bool UpdateHeaderTotal(int tranID, double total)
        {
            //Creating Boolean variable and set its default value to false
            bool isSuccess = false;

            //Creating SQL Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Query to Update Category
                string sql = "UPDATE tbl_transactions SET grandTotal = grandTotal - @total WHERE id=@tranID";

                //SQl Command to Pass the Value on Sql Query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Passing Value using cmd
                cmd.Parameters.AddWithValue("@tranID", tranID);
                cmd.Parameters.AddWithValue("@total", total);


                //Open DAtabase Connection
                conn.Open();

                //Create Int Variable to execute query
                int rows = cmd.ExecuteNonQuery();

                //if the query is successfully executed then the value will be grater than zero 
                if (rows > 0)
                {
                    //Query Executed Successfully
                    isSuccess = true;
                }
                else
                {
                    //Failed to Execute Query
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
        public bool UpdateTransactionTotal(int tranID, bool EsCompra)
        {
            //Creating Boolean variable and set its default value to false
            bool isSuccess = false;

            //Creating SQL Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Query to Update Category
                string sql = "";
                    if(EsCompra)
                    sql = "UPDATE tbl_transactions SET grandTotal =((select iif(sum(total) = null, 0, sum(total)) from tbl_transaction_detail where transaction_id = @tranID) * ((tax/100) + 1) *(1-(discount/100)) ) * -1  WHERE id=@tranID";
                    else
                    sql = "UPDATE tbl_transactions SET grandTotal =((select iif(sum(total) = null, 0, sum(total)) from tbl_transaction_detail where transaction_id = @tranID) * ((tax/100) + 1) *(1-(discount/100)) )  WHERE id=@tranID";

                //SQl Command to Pass the Value on Sql Query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Passing Value using cmd
                cmd.Parameters.AddWithValue("@tranID", tranID);


                //Open DAtabase Connection
                conn.Open();

                //Create Int Variable to execute query
                int rows = cmd.ExecuteNonQuery();

                //if the query is successfully executed then the value will be grater than zero 
                if (rows > 0)
                {
                    //Query Executed Successfully
                    isSuccess = true;
                }
                else
                {
                    //Failed to Execute Query
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
        public bool UpdateTransaction(transactionsBLL tran)
        {
            //Creating Boolean variable and set its default value to false
            bool isSuccess = false;

            //Creating SQL Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Query to Update Category
                string sql = "UPDATE tbl_transactions set dea_cust_id = @dea_cust_id, discount = @discount, tax = @tax where id = @tranID";
                  

                //SQl Command to Pass the Value on Sql Query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Passing Value using cmd
                cmd.Parameters.AddWithValue("@tranID", tran.id);
                cmd.Parameters.AddWithValue("@dea_cust_id", tran.dea_cust_id);
                cmd.Parameters.AddWithValue("@discount", tran.discount);
                cmd.Parameters.AddWithValue("@tax", tran.tax);


                //Open DAtabase Connection
                conn.Open();

                //Create Int Variable to execute query
                int rows = cmd.ExecuteNonQuery();

                //if the query is successfully executed then the value will be grater than zero 
                if (rows > 0)
                {
                    //Query Executed Successfully
                    isSuccess = true;
                }
                else
                {
                    //Failed to Execute Query
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

    }
}
