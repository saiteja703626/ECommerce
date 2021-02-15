using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Models
{
    //string Image = System.Text.Encoding.UTF8.GetString();
    public class ShoppingPage
    {
        public DataTable GetAllProducts()
        {
            string constr = ConfigurationManager.ConnectionStrings["EComm"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM shopping"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "shopping";
                            sda.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }
       
        public shopping GetProductById(int? id)
        {
            shopping shopping = new shopping();
            string constr = ConfigurationManager.ConnectionStrings["EComm"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string sqlquery = "SELECT * from shopping WHERE ID=" + id;
                SqlCommand cmd = new SqlCommand(sqlquery, con);
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    shopping.Product = Convert.ToString(sdr["Product"]);
                    shopping.ProductID = Convert.ToInt32(sdr["ProductID"]);
                    shopping.Cost = Convert.ToInt32(sdr["Cost"]);
                    shopping.Category = Convert.ToString(sdr["Category"]);
                    shopping.Image = Convert.ToString(sdr["Image"]);
                    shopping.Description = Convert.ToString(sdr["Description"]);
                }
            }
            return shopping;
        }

        public List<SelectListItem> UserReg()
        {
            users users = new users();
           // List<SelectListItem> RoleList = new List<SelectListItem>();

            
            List<SelectListItem> roleList = new List<SelectListItem>();
            //roleList.Add(new role { RoleId=1,Name="Admin"});
            string constr = ConfigurationManager.ConnectionStrings["EComm"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " SELECT RoleId, Name FROM Roles";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            roleList.Add(new SelectListItem
                            {
                                Text = sdr["Name"].ToString(),
                                Value = sdr["RoleId"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            //users.RoleList = roleList;
            return roleList;
        }



    }

}