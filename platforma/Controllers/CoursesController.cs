﻿using platforma.Models;
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

        // GET /Courses/BuyCourse
        public ActionResult BuyCourse(int id)
        {
            int studentId = (int)Session["studentId"];
            string query = string.Format("SELECT platforma.checkBoughtCourse({0}, {1})", studentId, id);
            int output = db.Database.ExecuteSqlCommand(query);
            if (output != 1)
            {
                string queryPrice = "SELECT * FROM platforma.prices p WHERE p.courseid = " + id;
                var price = db.Prices.SqlQuery(queryPrice).SingleOrDefault();
                string queryProc = string.Format("SELECT platforma.student_buys_course({0}, {1}, {2})", studentId, price.Price, id);
                int output2 = db.Database.ExecuteSqlCommand(queryProc);
            }
            return RedirectToAction("MyCourses", "Courses");
        }

        // GET: /MyCourses
        public ActionResult MyCourses()
        {
            var data = new object();
            if (Session["userType"].Equals("student"))
            {
                string queryCourses = "SELECT sc.id, sc.instructorid, sc.name, sc.description, sc.price, sc.avggrade, sc.categories " +
                    "FROM platforma.student_courses sc " +
                    "WHERE sc.studentid = @p0 " +
                    "ORDER BY sc.name ";
                int studentid = (int)Session["studentId"];
                data = db.Courses.SqlQuery(queryCourses, studentid).ToList();
            }
            else if (Session["userType"].Equals("instructor"))
            {
                string queryCourses = "SELECT c.*, 0 as price, 0 as avgGrade, c.name as categories " +
                    "FROM platforma.courses c " +
                    "WHERE c.instructorid = @p0 " +
                    "ORDER BY c.name ";
                int instructorid = (int)Session["instructorId"];
                data = db.Courses.SqlQuery(queryCourses, instructorid).ToList();
            }
            
            return View(data);
        }

        //GET /Courses/WatchCourse/5
        public ActionResult WatchCourse(int id)
        {
            int studentId = (int)Session["studentId"];
            string queryBC = string.Format("SELECT platforma.checkBoughtCourse({0}, {1})", studentId, id);
            int output = db.Database.ExecuteSqlCommand(queryBC);

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

        public ActionResult WatchCourseNotBought()
        {
            return View();
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
            try
            {
                //course attributes

                List<object> lst = new List<object>();
                int instructorId = (int)Session["instructorId"];
                lst.Add(instructorId);
                lst.Add(collection.Name);
                lst.Add(collection.Description);
                object[] allItems = lst.ToArray();
                string queryCourses = string.Format("CALL platforma.insert_course({0}, \'{1}\', \'{2}\')", instructorId, collection.Name, collection.Description);
                int outputCourses = db.Database.ExecuteSqlCommand(queryCourses);

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
                    string queryInsert = string.Format("CALL platforma.insert_category_course({0}, {1})", newCourse.Id, cat.Id);
                    int outputQuery = db.Database.ExecuteSqlCommand(queryInsert);
                }

                string queryPrices = string.Format("CALL priceCourse({0}, {1})", newCourse.Id, collection.Price);
                int outputPrices = db.Database.ExecuteSqlCommand(queryPrices);

                //videos
                foreach (Video v in collection.Videos)
                {
                    if (v.Name is null || v.Name == "")
                    {
                        break;
                    }
                    string queryVideos = string.Format("CALL platforma.insert_video({0}, \'{1}\', \'{2}\', \'{3}\')", newCourse.Id, v.Name, v.PathVideo, v.PathImg);
                    int outputVideos = db.Database.ExecuteSqlCommand(queryVideos);
                }

                //documents
                foreach (Document d in collection.Documents)
                {
                    if (d.Name is null || d.Name == "")
                    {
                        break;
                    }
                    string queryDocuments = string.Format("CALL platforma.insert_document({0}, \'{1}\', \'{2}\')", newCourse.Id, d.Name, d.Path);
                    int outputDocuments = db.Database.ExecuteSqlCommand(queryDocuments);
                }

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
            int responsibleId;
            if (Session["instructorId"] != null)
            {
                responsibleId = (int)Session["instructorId"];
            }
            else
            {
                responsibleId = (int)Session["adminId"];
            }

            string query = "SELECT * " +
                "FROM platforma.coursesview cv " +
                "WHERE cv.id = @p0 " +
                "ORDER BY cv.price desc " +
                "LIMIT 1";

            var data = db.Courses.SqlQuery(query, id).SingleOrDefault();
            
            if (responsibleId != data.InstructorId)
            {
                return RedirectToAction("CourseNotOwner");
            }

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
            try
            {
                string queryCourse = string.Format("CALL platforma.update_course(\'{0}\', \'{1}\', {2})", collection.Name, collection.Description, collection.Id);
                int outputCourse = db.Database.ExecuteSqlCommand(queryCourse);

                string queryPrices = string.Format("CALL priceCourse({0}, {1})", collection.Id, collection.Price);
                int outputPrices = db.Database.ExecuteSqlCommand(queryPrices);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CourseNotOwner()
        {
            return View();
        }

    }
}
