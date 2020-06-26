using platforma.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Text;

namespace platforma.Controllers
{
    
    public class CategoriesController : Controller
    {
        DataContext db = new DataContext();

        // GET: Category
        public ActionResult Index()
        {
            string query = "SELECT * " +
                "FROM platforma.categories c " +
                "ORDER BY c.name";
            var data = db.Categories.SqlQuery(query).ToList();

            return View(data);
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            string query = "SELECT * " +
                "FROM platforma.categories c " +
                "WHERE c.id = @p0 ";
            var data = db.Categories.SqlQuery(query, id).SingleOrDefault();

            string query2 = "SELECT * " +
                "FROM platforma.category_details cd " +
                "WHERE cd.categoryid = @p0 ";
            var data2 = db.Courses.SqlQuery(query2, id).ToList();
            data.Courses = data2;
            
            return View(data);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(Category collection)
        {
            try
            {
                string queryInsert = "CALL platforma.insert_category(\'" + collection.Name + "\')";
                int output = db.Database.ExecuteSqlCommand(queryInsert);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            string query = "SELECT * " +
                "FROM platforma.categories c " +
                "WHERE c.id = @p0";
            var data = db.Categories.SqlQuery(query, id).SingleOrDefault();

            return View(data);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(Category collection)
        {
            try
            {
                string queryUpdate = "CALL platforma.update_category(\'" + collection.Name + "\', " + collection.Id + ")";
                int output = db.Database.ExecuteSqlCommand(queryUpdate);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /* uncomment if needed Delete Category functions
        
        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            string query = "SELECT * " +
                "FROM platforma.categories c " +
                "WHERE c.id = @p0";
            var data = db.Categories.SqlQuery(query, id).SingleOrDefault();

            return View(data);
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(Category collection)
        {
            try
            {
                string queryDelete = "CALL platforma.delete_category(" + collection.Id + ")";
                int output = db.Database.ExecuteSqlCommand(queryDelete);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }*/
    }
}
