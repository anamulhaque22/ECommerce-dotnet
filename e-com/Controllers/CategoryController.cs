using e_com.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace e_com.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            var db = new E_CommEntities();
            var Category = db.Categories.ToList();

            return View(Category);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category c) { 
            var db = new E_CommEntities();
            db.Categories.Add(c);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new E_CommEntities();
            var dbCategory = (from c in db.Categories
                             where c.Id == id
                             select c).SingleOrDefault();
            return View(dbCategory);
        }
        [HttpPost]
        public ActionResult Edit(Category c) {
            var db = new E_CommEntities();
            var dbCagegory = db.Categories.Find(c.Id);
            dbCagegory.Name = c.Name;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var db = new E_CommEntities();
            var dbCagegory = db.Categories.Find(id);
            db.Categories.Remove(dbCagegory); 
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}