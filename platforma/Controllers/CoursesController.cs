using platforma.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platforma.Controllers
{
    public class CoursesController : Controller
    {
        DataContext db = new DataContext();

        // GET: Courses
        public ActionResult Index()
        {
            string query = "SELECT c.*, 0 as price, 0 as AvgGrade, c.name as Categories " +
                "FROM platforma.courses c " +
                "ORDER BY c.name";
            var data = db.Courses.SqlQuery(query).ToList();

            return View(data);
        }

        // GET: Courses/Details/5
        public ActionResult Details(int id)
        {
            string query = "SELECT * " +
                "FROM platforma.coursesview cv " +
                "WHERE cv.id = @p0 ";
            var data = db.Courses.SqlQuery(query, id).SingleOrDefault();
            data.Videos = new List<Video>();
            data.Documents = new List<Document>();

            string queryReviews = "SELECT c.* " +
                "FROM platforma.course_review c " +
                "WHERE c.courseid = @p0 ";
            var dataReviews = db.Reviews.SqlQuery(queryReviews, id).ToList();

            float sum = 0;
            for (int i=0; i<dataReviews.Count(); i++)
            {
                sum += dataReviews[i].Grade;
            }
            data.AvgGrade = sum / dataReviews.Count();

            return View(data);
        }

        //GET /Courses/WatchCourse/5
        public ActionResult WatchCourse(int id)
        {
            string query = "SELECT c.*, 0 AS price, 0 AS AvgGrade, c.name as Categories " +
                "FROM platforma.courses c " +
                "WHERE c.id = @p0 ";
            var data = db.Courses.SqlQuery(query, id).SingleOrDefault();

            string queryVideos = "SELECT v.* " +
                "FROM platforma.videos v " +
                "WHERE v.courseid = @p0 " +
                "ORDER BY v.name ";
            var dataVideos = db.Videos.SqlQuery(queryVideos, id).ToList();
            data.Videos = dataVideos;

            string queryDocuments = "SELECT d.* " +
                "FROM platforma.documents d " +
                "WHERE d.courseid = @p0 ";
            var dataDocuments = db.Documents.SqlQuery(queryDocuments, id).ToList();
            data.Documents = dataDocuments;

            return View(data);
        }

        // GET: /Courses/PlayVideo/5
        public ActionResult PlayVideo(int id)
        {
            string queryVideo = "SELECT v.* " +
                "FROM platforma.videos v " +
                "WHERE v.id = @p0 ";
            var dataVideo = db.Videos.SqlQuery(queryVideo, id).SingleOrDefault();

            return View(dataVideo);
        }      

        // GET: Courses/Create
        public ActionResult Create()
        {
            //SAMO INSTRUKTOR
            string queryCategories = "SELECT * " +
                "FROM platforma.categories c ";
            var categories = db.Categories.SqlQuery(queryCategories).ToList();
            ViewBag.categories = categories;
      
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        public ActionResult Create(Course collection)
        {
            //SAMO INSTRUKTOR
            try
            {
                //course attributes
                List<object> lst = new List<object>();
                lst.Add(collection.InstructorId);
                lst.Add(collection.Name);
                lst.Add(collection.Description);
                object[] allItems = lst.ToArray();
                string queryCourses = "INSERT INTO platforma.courses(instructorid, name, description) " +
                    "VALUES (@p0, @p1, @p2)";        
                int outputCourses = db.Database.ExecuteSqlCommand(queryCourses, allItems);

                //find the new course
                string courseQuery = "SELECT c.*, 0 as price, 0 as avgGrade, c.name as Categories " +
                    "FROM platforma.courses c " +
                    "WHERE c.instructorid = @p0 " +
                    "AND c.name = @p1 " +
                    "AND c.description = @p2 ";
                var newCourse = db.Courses.SqlQuery(courseQuery, allItems).SingleOrDefault();

                //categories
                string[] categories = collection.Categories.Split(',');
                for (int i = 0; i < categories.Count(); i++)
                {
                    string queryCategory = "SELECT * " +
                        "FROM platforma.categories c " +
                        "WHERE c.name = \'" + categories[i] + "\'";
                    var cat = db.Categories.SqlQuery(queryCategory).SingleOrDefault();

                    string queryInsert = "INSERT INTO platforma.category_courses(courseid, categoryid) " +
                        "VALUES (" + newCourse.Id + ", " + cat.Id + ") ";
                    int outputQuery = db.Database.ExecuteSqlCommand(queryInsert);
                }

                //video attributes
                string queryVideos = "INSERT INTO platforma.videos(courseid, name, pathvideo, pathimg) " +
                    "VALUES (@p0, @p1, @p2, @p3) ";

                foreach (Video v in collection.Videos)
                {
                    if (v.Name is null || v.Name == "")
                    {
                        break;
                    }
                    List<object> lista = new List<object>();
                    lista.Add(newCourse.Id);
                    lista.Add(v.Name);
                    lista.Add(v.PathVideo);
                    lista.Add(v.PathImg);
                    object[] listaO = lista.ToArray();
                    int outputVideos = db.Database.ExecuteSqlCommand(queryVideos, listaO);
                }

                //document attributes
                string queryDocuments = "INSERT INTO platforma.document(courseid, name, path) " +
                    "VALUES (@p0, @p1, @p2) ";

                foreach (Document d in collection.Documents)
                {
                    if (d.Name is null || d.Name == "")
                    {
                        break;
                    }
                    List<object> lista = new List<object>();
                    lista.Add(newCourse.Id);
                    lista.Add(d.Name);
                    lista.Add(d.Path);
                    object[] listaO = lista.ToArray();
                    int outputDocuments = db.Database.ExecuteSqlCommand(queryDocuments, listaO);
                }

                string queryPrices = "CALL priceCourse(" + newCourse.Id + ", " + collection.Price + ");";
                int outputPrices = db.Database.ExecuteSqlCommand(queryPrices);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int id)
        {
            //SAMO INSTRUKTOR
            string query = "SELECT * " +
                "FROM platforma.coursesview cv " +
                "WHERE cv.id = @p0";

            var data = db.Courses.SqlQuery(query, id).SingleOrDefault();

            string queryVideos = "SELECT v.* " +
                "FROM platforma.videos v " +
                "WHERE v.courseid = @p0";
            var dataVideos = db.Videos.SqlQuery(queryVideos, id).ToList();
            data.Videos = dataVideos;

            string queryDocuments = "SELECT d.* " +
                "FROM platforma.documents d " +
                "WHERE d.courseid = @p0";
            var dataDocuments = db.Documents.SqlQuery(queryDocuments, id).ToList();
            data.Documents = dataDocuments;

            string queryReviews = "SELECT c.* " +
                "FROM platforma.course_review c " +
                "WHERE c.courseid = @p0 ";
            var dataReviews = db.Reviews.SqlQuery(queryReviews, id).ToList();
            float sum = 0;
            for (int i = 0; i < dataReviews.Count(); i++)
            {
                sum += dataReviews[i].Grade;
            }
            data.AvgGrade = sum / dataReviews.Count();

            //view
            string queryCategories = "SELECT * " +
                "FROM platforma.categoriesview cv " +
                "WHERE cv.courseid = @p0";
            List<Category> dataCategories = db.Categories.SqlQuery(queryCategories, id).ToList();
            string categories = "";
            foreach (Category c in dataCategories)
            {
                categories += c.Name += ",";
            }
            data.Categories = categories.Substring(0, categories.Length - 1);

            return View(data);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        public ActionResult Edit(Course collection)
        {
            //SAMO INSTRUKTOR
            try
            {
                //course
                List<object> lst = new List<object>();
                lst.Add(collection.Name);
                lst.Add(collection.Description);
                lst.Add(collection.Id);
                object[] allItems = lst.ToArray();
                string queryCourses = "UPDATE platforma.courses c " +
                    "SET name = @p0, description = @p1 " +
                    "WHERE id = @p2 ";
                int outputCourses = db.Database.ExecuteSqlCommand(queryCourses, allItems);

                //price
                string queryPrices = "CALL priceCourse(" + collection.Id + ", " + collection.Price + ");";
                int outputPrices = db.Database.ExecuteSqlCommand(queryPrices);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
