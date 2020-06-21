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
        [AllowAnonymous]
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

            string query2 = "SELECT cou.*, p.price, 0 as AvgGrade, 'asd' as Categories " +
                "FROM platforma.category_courses cc " +
                "INNER JOIN platforma.courses cou ON cc.courseid = cou.id " +
                "INNER JOIN platforma.prices p ON p.courseid = cou.id " +
                "WHERE cc.categoryid = @p0 " +
                "ORDER BY cou.name";

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
                List<object> lst = new List<object>();
                lst.Add(collection.Name);
                object[] allItems = lst.ToArray();
                string query = "INSERT INTO platforma.categories(name) " +
                    "VALUES (@p0)";

                int output = db.Database.ExecuteSqlCommand(query, allItems);

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
                List<object> lst = new List<object>();
                lst.Add(collection.Name);
                lst.Add(collection.Id);
                object[] allItems = lst.ToArray();
                string query = "UPDATE platforma.categories c " +
                    "SET name = @p0 " +
                    "WHERE id = @p1";

                int output = db.Database.ExecuteSqlCommand(query, allItems);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

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
                List<object> lst = new List<object>();
                lst.Add(collection.Id);
                object[] allItems = lst.ToArray();
                string query = "DELETE " +
                    "FROM platforma.categories c " +
                    "WHERE id = @p0";

                int output = db.Database.ExecuteSqlCommand(query, allItems);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
