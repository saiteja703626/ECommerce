using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    public class SiteController : Controller
    {
        ShoppingPage shoppingPage = new ShoppingPage();
        // GET: Site/Home
        public ActionResult Home(shopping Shopping)
        {
            ShoppingPage shoppingPage = new ShoppingPage();

            List<shopping> shoppinglist = new List<shopping>();
            var GetProducts = shoppingPage.GetAllProducts();
            for (int i = 0; i < GetProducts.Rows.Count; i++)
            {
                shopping shopping = new shopping();
                shopping.ID = Convert.ToInt32(GetProducts.Rows[i]["ID"]);
                shopping.Product = Convert.ToString(GetProducts.Rows[i]["Product"]);
                shopping.ProductID = Convert.ToInt32(GetProducts.Rows[i]["ProductID"]);
                shopping.Cost = Convert.ToInt32(GetProducts.Rows[i]["Cost"]);
                shopping.Category = Convert.ToString(GetProducts.Rows[i]["Category"]);
                shopping.Image = Convert.ToString(GetProducts.Rows[i]["Image"]);
                shopping.Description = Convert.ToString(GetProducts.Rows[i]["Description"]);

                shoppinglist.Add(shopping);
            }
            return View(shoppinglist);
        }

        public ActionResult Clothing(int? id)
        {
            ShoppingPage shoppingPage = new ShoppingPage();
            if (id == null)
            {
                return HttpNotFound();
            }
            shopping shopping = shoppingPage.GetProductById(id);

            if (shopping == null)
            {
                return HttpNotFound();
            }
            return View(shopping);
        }

        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(shopping Shopping, HttpPostedFileBase file)
        {
            string constr = ConfigurationManager.ConnectionStrings["EComm"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            string sqlquery = "insert into [dbo].[shopping] (Product, ProductID, Cost, Category, Image, Description) VALUES (@Product, @ProductID, @Cost, @Category, @Image, @Description)";
            SqlCommand com = new SqlCommand(sqlquery, con);
            con.Open();
            com.Parameters.AddWithValue("@Product", Shopping.Product);
            com.Parameters.AddWithValue("@ProductID", Shopping.ProductID);
            com.Parameters.AddWithValue("@Cost", Shopping.Cost);
            com.Parameters.AddWithValue("@Category", Shopping.Category);

            if (file != null && file.ContentLength > 0)
            {
                string filename = Path.GetFileName(file.FileName);
                string imgpath = Path.Combine(Server.MapPath("~/Product-Images"), filename);
                file.SaveAs(imgpath);
            }
            com.Parameters.AddWithValue("@Image", "/Product-Images/" + file.FileName);

            com.Parameters.AddWithValue("@Description", Shopping.Description);
            com.ExecuteNonQuery();
            con.Close();
            ViewData["Message"] = "Product " + Shopping.Product + " is added successfully";
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(users users)
        {
            string maincon = ConfigurationManager.ConnectionStrings["EComm"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(maincon);
            string sqlquery = "select * from users where UserName=@UserName and Password=@Password";
            sqlcon.Open();
            SqlCommand sqlcom = new SqlCommand(sqlquery, sqlcon);

            SqlParameter usernameparam = sqlcom.Parameters.AddWithValue("@UserName", users.UserName);
            if (users.UserName == null)
            {
                usernameparam.Value = DBNull.Value;
            }

            SqlParameter passwordparam = sqlcom.Parameters.AddWithValue("@Password", users.Password);
            if (users.Password == null)
            {
                passwordparam.Value = DBNull.Value;
            }
            SqlDataReader sdr = sqlcom.ExecuteReader();
            if (sdr.Read())
            {
                return RedirectToAction("Home");
            }
            else
            {
                ViewData["Msg"] = "Enter a Valid UserName and Password";
            }
            sqlcon.Close();
            return View();
        }


        public ActionResult Registration()
        {
            users obj = new users();
            obj.RoleList = shoppingPage.UserReg();
            return View(obj);
        }

        [HttpPost]
        public ActionResult Registration(users users)
        {

            //users users = shoppingPage.UserReg();

            String strConnString = ConfigurationManager.ConnectionStrings["EComm"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_Add_User";
            cmd.Parameters.AddWithValue("@UserName", users.UserName);
            cmd.Parameters.AddWithValue("@Password", users.Password);
            cmd.Parameters.AddWithValue("@Email", users.Email);
            cmd.Parameters.AddWithValue("@RoleId", users.Role);
            cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                string id = cmd.Parameters["@id"].Value.ToString();
                users.RoleList = shoppingPage.UserReg();
                ViewData["message"] = "The New User " + users.UserName + " has created successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return View(users);
        }

        
    }
}