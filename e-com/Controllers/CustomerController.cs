using e_com.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace e_com.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Registration() {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(Customer c)
        {
            var db = new E_CommEntities();
            if(db.Customers.Any(dc => dc.Email == c.Email))
            {
                ViewBag.Notification = "This account has already existed!";
                return View();
            }
            db.Customers.Add(c);
            db.SaveChanges();
            return RedirectToAction("Login", "Customer");
        }

        [HttpGet]
        public ActionResult Login() {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Customer c) {
            var db = new E_CommEntities();
            var checkLogin = db.Customers.Where(x => x.Email.Equals(c.Email) && x.Password.Equals(c.Password)).FirstOrDefault();
            if(checkLogin != null){
                Session["UserEmail"] = c.Email.ToString();
                Session["UserName"] = checkLogin.Name.ToString();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Notification = "Wrong Username or Passowrd!";
            return View();
        }

        public ActionResult OrderList()
        {
            var db = new E_CommEntities();
            var orders = new List<Order>();
            if (Session["UserEmail"] != null)
            {
                string customerEmail = (string)Session["UserEmail"];
                orders = db.Orders.Where(o => o.Customer.Email == customerEmail).ToList();
            }
            return View(orders);
        }
    }
}