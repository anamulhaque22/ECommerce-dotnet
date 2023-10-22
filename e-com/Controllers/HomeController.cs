using e_com.Auth;
using e_com.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace e_com.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var db = new E_CommEntities();
            var product = db.Products.ToList();
            return View(product);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var db = new E_CommEntities();
            var Categorys = db.Categories.ToList();
            ViewBag.Category = Categorys;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product p)
        {
            var db = new E_CommEntities();
            db.Products.Add(p);
            db.SaveChanges();
            
            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new E_CommEntities();
            var categorys = db.Categories.ToList();
            var dbProduct = (from p in db.Products
                             where p.Id == id
                             select p).SingleOrDefault();
            ViewBag.Categorys = categorys;
            return View(dbProduct);
        }

        [HttpPost]
        public ActionResult Edit(Product p) {
            var db = new E_CommEntities();
            var dbData = db.Products.Find(p.Id);
            dbData.Title = p.Title;
            dbData.Price = p.Price;
            dbData.Category = p.Category;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var db = new E_CommEntities();
            var dbProduct = db.Products.Find(id);
            db.Products.Remove(dbProduct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}