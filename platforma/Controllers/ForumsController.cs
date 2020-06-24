using Microsoft.Ajax.Utilities;
using platforma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platforma.Controllers
{
    public class ForumsController : Controller
    {
        DataContext db = new DataContext();
        // GET: Forums
        public ActionResult Index()
        {
            string query = "SELECT f.* " +
                "FROM platforma.forums f ";
            var data = db.Forums.SqlQuery(query).ToList();

            return View(data);
        }

        // GET: Forums/Details/5
        public ActionResult Details(int id)
        {
            string queryComments = "SELECT fc.* " +
                "FROM platforma.forumcomments fc " +
                "WHERE fc.forumid = @p0 " +
                "ORDER BY fc.datecomment DESC ";
            var comments = db.Comments.SqlQuery(queryComments, id).ToList();

            string query = "SELECT f.id, f.description " +
                "FROM platforma.forums f " +
                "WHERE f.id = @p0 ";
            var data = db.Forums.SqlQuery(query, id).SingleOrDefault();
            data.Comments = comments;
            return View(data);
        }

        // GET: Forums/AddComment
        public ActionResult AddComment(int id)
        {
            Session["forumId"] = id;
            return View();
        }

        // POST: Forums/AddComment
        [HttpPost]
        public ActionResult AddComment(ForumComment newComment)
        {
            // instruktor ili student
            try
            {
                if (Session["studentId"] != null)
                {
                    int studentId = (int)Session["studentId"];
                    int forumId = (int)Session["forumId"];
                    string queryAddComment = "INSERT INTO platforma.forums_students(forumid, studentid, comment) " +
                        "VALUES(" + forumId + ", " + studentId + ", \'" + newComment.Comment + "\')" ;
                    int output = db.Database.ExecuteSqlCommand(queryAddComment);

                }
                else
                {
                    int instructorId = (int)Session["instructorId"];
                    int forumId = (int)Session["forumId"];
                    string queryAddComment = "INSERT INTO platforma.forums_instructors(forumid, instructorid, comment) " +
                        "VALUES(" + forumId + ", " + instructorId + ", \'" + newComment.Comment + "\')";
                    int output = db.Database.ExecuteSqlCommand(queryAddComment);
                }

                string _redirect = "Details/" + (int)Session["forumId"];
                return RedirectToAction(_redirect);
            }
            catch
            {
                return View();
            }
        }

        // GET: Forums/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Forums/Create
        [HttpPost]
        public ActionResult Create(Forum newForum)
        {
            try
            {
                string query = "INSERT INTO platforma.forums(description) " +
                    "VALUES (@p0)";
                int output = db.Database.ExecuteSqlCommand(query, newForum.Description);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Forums/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Forums/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Forums/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Forums/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
