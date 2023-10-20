using e_com.EF;
using e_com.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace e_com.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
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
        public ActionResult Create(Product p, HttpPostedFileBase Image)
        {
            var db = new E_CommEntities();
            if (Image != null)
            {
                string fileName = Image.FileName;
                p.Image = fileName;
                string path = Server.MapPath("~/Content/Product/"+ fileName);
                Image.SaveAs(path);
                db.Products.Add(p);
                db.SaveChanges();
            }

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
        public ActionResult Edit(Product p)
        {
            var db = new E_CommEntities();
            var dbData = db.Products.Find(p.Id);
            dbData.Title = p.Title;
            dbData.Price = p.Price;
            dbData.Category = p.Category;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = new E_CommEntities();
            var dbProduct = db.Products.Find(id);
            string folderPath = Server.MapPath("~/Content/Product/");
            string path = Path.Combine(folderPath, dbProduct.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            db.Products.Remove(dbProduct);
            db.SaveChanges();
            return RedirectToAction("Index", "Product");
        }
    }
}