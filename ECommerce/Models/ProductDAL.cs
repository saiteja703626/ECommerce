using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class ProductDAL
    {
        SqlCommand cmd;
        SqlDataAdapter da;
        DataSet ds;
        public static SqlConnection connect()
        {
            string connection = ConfigurationManager.ConnectionStrings["EComm"].ConnectionString;
            SqlConnection con = new SqlConnection(connection);
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            else
            {
                con.Open();
            }
            return con;
        }
        public bool DMLOpperation(string query)
        {
            cmd = new SqlCommand(query, ProductDAL.connect());
            int x = cmd.ExecuteNonQuery();
            if (x == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public DataTable SelectAll(string query)
        {
            da = new SqlDataAdapter(query, ProductDAL.connect());
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}