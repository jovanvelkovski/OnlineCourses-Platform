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

        // GET: Forums/AddComment/5
        public ActionResult AddComment(int id)
        {
            Session["forumId"] = id;
            return View();
        }

        // POST: Forums/AddComment/5
        [HttpPost]
        public ActionResult AddComment(ForumComment newComment)
        {
            try
            {
                if (Session["studentId"] != null)
                {
                    int studentId = (int)Session["studentId"];
                    int forumId = (int)Session["forumId"];
                    string queryAddComment = string.Format("CALL platforma.add_forum_comment_as_a_student({0}, {1}, \'{2}\')", forumId, studentId, newComment.Comment);
                    int output = db.Database.ExecuteSqlCommand(queryAddComment);

                }
                else
                {
                    int instructorId = (int)Session["instructorId"];
                    int forumId = (int)Session["forumId"];
                    string queryAddComment = string.Format("CALL platforma.add_forum_comment_as_an_instructor({0}, {1}, \'{2}\')", forumId, instructorId, newComment.Comment);
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
                string queryCreate = string.Format("CALL platforma.insert_forum(\'{0}\')", newForum.Description);
                int output = db.Database.ExecuteSqlCommand(queryCreate);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
