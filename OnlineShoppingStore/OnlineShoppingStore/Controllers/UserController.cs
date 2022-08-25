using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShoppingStore.Models;
using System.Web.Security;
using System.Data.Entity;

namespace OnlineShoppingStore.Controllers
{
    public class UserController : Controller
    {
        private eCommerce_AUHEntities db = new eCommerce_AUHEntities();
        // GET: User
        public ActionResult Logout()
        {
            Session["username"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
        
        
        public ActionResult EditProfile()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int logged_in_user_id = Convert.ToInt32(Session["UserId"]);
            User obj = db.Users.Find(logged_in_user_id);
            return View(obj);
        }


        [HttpPost]
        public ActionResult EditProfile(User obj)
        {
            if (ModelState.IsValid)
            {
                obj.Email_Verified = "y";
                obj.Role = "customer";
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                TempData["ProfileUpdateSuccess"] = "<script>alert('Profile update successfully!')</script>";
                return RedirectToAction("Index", "Home");
            }
            return View(obj);
        }

        public ActionResult OrderHistory()
        {
            int uid = Convert.ToInt32(Session["UserID"]);
            var orders = db.Orders.Include(o => o.User);

            return View(orders.Where(a=>a.User_ID == uid).ToList());
        }
    }
}