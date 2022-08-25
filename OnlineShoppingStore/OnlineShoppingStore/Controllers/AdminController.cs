using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity; //Import this library - Step 1
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineShoppingStore.Models;
using System.IO;
using System.Web.Security;
using PagedList;

namespace OnlineShoppingStore.Controllers
{
    public class AdminController : Controller
    {
        private eCommerce_AUHEntities db = new eCommerce_AUHEntities();
        // GET: Admin



        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel objUser)
        {
            if (ModelState.IsValid)
            {
                var obj = db.Users.Where(a => a.Email.Equals(objUser.Email)
                && a.Password.Equals(objUser.Password)
                && a.Email_Verified.Equals("Y") && a.Role.Equals("Admin")).FirstOrDefault();

                if (obj != null) //on success
                {
                    Session["AdminName"] = obj.Name;
                    FormsAuthentication.SetAuthCookie(obj.Email, false);
                    return RedirectToAction("Index", "Admin");
                }
            }
            ModelState.AddModelError("InvalidLoginError", "Invalid credentials or email not verified!");
            return View();
        }




        public ActionResult Index()
        {
            if (Session["AdminName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TotalOrders = db.Orders.Count();
            ViewBag.TotalUsers = db.Users.Count();
            ViewBag.TotalEarnings = db.Orders.Where(a => a.Order_Status.Equals("Completed")).
                                    Select(a => a.Total_Amount).Sum();
            return View(db.Order_Details.ToList());
        }

        public ActionResult AddProduct()
        {
            if (Session["AdminName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Category_ID = new SelectList(db.Categories, "Category_ID", "Category_Name");

            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);//product1
                string extension = Path.GetExtension(file.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                product.Picture = "/External/Main/Products/" + fileName;
                fileName = Path.Combine(Server.MapPath("/External/Main/Products/"), fileName);
                file.SaveAs(fileName);

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("ListProduct");
            }
            ViewBag.Category_ID = new SelectList(db.Categories, "Category_ID",
                "Category_Name", product.Category_ID);
            return View(product);
        }

        //Step 2 - Update your ListProduct Action Method
        public ActionResult ListProduct(int?page)
        {
            if (Session["AdminName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var products = db.Products.Include(a => a.Category1);
            return View(products.ToList());
            ViewBag.Category_ID = new SelectList(db.Categories, "Category_ID", "Category_Name");
            Product_with_Filter model = new Product_with_Filter();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            model.productList = db.Products.OrderBy(a => a.Product_id).ToPagedList(pageNumber, pageSize);
            return View(model);
            return View();
        }


        public ActionResult DeleteProduct(int? id)
        {
            if (Session["AdminName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                RedirectToAction("Index");
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("DeleteProduct")]
        public ActionResult ConfirmDeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            TempData["SuccessfullyDeletedProduct"] = "<script>alert('Product has been deleted!')</script>";
            return RedirectToAction("ListProduct");
        }

        public ActionResult EditProduct(int? id)
        {
            if (Session["AdminName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "Category_ID", "Category_Name", product.Category_ID);
            return View(product);
        }

        public ActionResult Logout()
        {
            Session["AdminName"] = null;
            FormsAuthentication.SignOut();
            TempData["AdminLogout"] = "<script>alert('Admin has signed out!');</script>";
            return RedirectToAction("Index", "Home");
        }


        public ActionResult ListUser()
        {
            if (Session["AdminName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(db.Users.ToList());
        }
        public ActionResult AddUser()
        {
            if (Session["AdminName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }
        public ActionResult ListOrder()
        {
            if (Session["AdminName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var orders = db.Orders.Include(o => o.User);
            return View(orders.ToList());
        }
        public ActionResult DeleteOrders(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedOrders(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EditOrders(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_ID = new SelectList(db.Users, "User_ID", "Name", order.User_ID);
            return View(order);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOrders( Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.User_ID = new SelectList(db.Users, "User_ID", "Name", order.User_ID);
            return View(order);
        }
        public ActionResult DetailsOrders(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }




        public ActionResult UsersDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        public ActionResult UsersEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsersEdit( User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }
        public ActionResult UsersDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult UsersDeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }


}