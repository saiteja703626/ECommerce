using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    public class AddToCartController : Controller
    {
        DataTable dt;
        ProductDAL productDAL = new ProductDAL();
        public ActionResult Add(shopping shopping)
        {
            if (Session["cart"] == null)
            {
                List<shopping> li = new List<shopping>();

                li.Add(shopping);
                Session["cart"] = li;
                ViewBag.cart = li.Count();

                Session["count"] = 1;
            }
            else
            {
                List<shopping> li = (List<shopping>)Session["cart"];
                li.Add(shopping);
                Session["cart"] = li;
                ViewBag.cart = li.Count();
                Session["count"] = Convert.ToInt32(Session["count"]) + 1;
            }
            return RedirectToAction("Home", "Site");
        }

        public ActionResult Myorder()
        {
            return View((List<shopping>)Session["cart"]);
        }

        public ActionResult Checkout()
        {
            return View();
        }
    }
}