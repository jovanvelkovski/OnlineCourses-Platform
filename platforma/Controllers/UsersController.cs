using Microsoft.Ajax.Utilities;
using platforma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platforma.Controllers
{
    public class UsersController : Controller
    {
        DataContext db = new DataContext();

        // GET: Users/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        [HttpPost]
        public ActionResult Register(User newUser)
        {
            try
            {
                string queryRegisterUser = string.Format("CALL platforma.register_user(\'{0}\', \'{1}\', \'{2}\', \'{3}\', \'{4}\', \'{5}\')",
                    newUser.FirstName, newUser.LastName, newUser.Sex, newUser.Email, newUser.Username, newUser.Password);

                int output = db.Database.ExecuteSqlCommand(queryRegisterUser);

                string queryUserId = string.Format("SELECT * FROM platforma.users WHERE username=\'{0}\'", newUser.Username);
                var user = db.Users.SqlQuery(queryUserId).SingleOrDefault();
                Session["userId"] = user.Id;

                return RedirectToAction("ChooseUserType");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/ChooseUserType
        public ActionResult ChooseUserType()
        {
            return View();
        }

        // GET: Users/RegisterStudent
        public ActionResult RegisterStudent()
        {
            return View();
        }

        // POST: Users/RegisterStudent
        [HttpPost]
        public ActionResult RegisterStudent(Student student)
        {
            try
            {
                int id = (int)Session["userId"];
                string queryRegisterStudent = string.Format("CALL platforma.register_as_a_student({0}, {1}, \'{2}\')", id, student.Age, student.Country);
                int output = db.Database.ExecuteSqlCommand(queryRegisterStudent);

                return RedirectToAction("Login", "Users");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/RegisterInstructor
        public ActionResult RegisterInstructor()
        {
            return View();
        }

        // Post: Users/RegisterInstructor
        [HttpPost]
        public ActionResult RegisterInstructor(Instructor instructor)
        {
            try
            {
                int id = (int)Session["userId"];
                string queryRegisterInstructor = string.Format("CALL platforma.register_as_an_instructor({0}, {1}, \'{2}\')", id, instructor.Age, instructor.Country);
                int output = db.Database.ExecuteSqlCommand(queryRegisterInstructor);

                return RedirectToAction("Login", "Users");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        public ActionResult Login(User user)
        {
            try
            {
                string queryCheckUser = string.Format("SELECT platforma.checkUser(\'{0}\', \'{1}\')", user.Username, user.Password);
                int loginSuccessful = db.Database.ExecuteSqlCommand(queryCheckUser);
                if (loginSuccessful != 0)
                {
                    string queryName = "SELECT * " +
                        "FROM platforma.users u " +
                        "WHERE u.username = \'" + user.Username + "\'";
                    var person = db.Users.SqlQuery(queryName).SingleOrDefault();
                    Session["firstName"] = person.FirstName;
                    Session["username"] = person.Username;
                    Session["userId"] = person.Id;

                    string queryStudent = "SELECT * " +
                        "FROM platforma.students s " +
                        "WHERE s.userid = " + person.Id;
                    string queryInstructor = "SELECT * " +
                        "FROM platforma.instructors i " +
                        "WHERE i.userid = " + person.Id;
                    string queryAdmin = "SELECT * " +
                        "FROM platforma.admins a " +
                        "WHERE a.userid = " + person.Id;

                    var student = db.Students.SqlQuery(queryStudent).SingleOrDefault();
                    var instructor = db.Instructors.SqlQuery(queryInstructor).SingleOrDefault();
                    var admin = db.Admins.SqlQuery(queryAdmin).SingleOrDefault();
                    if (student != null)
                    {
                        Session["studentId"] = student.Id;
                        Session["userType"] = "student";
                    }
                    else
                    {
                        Session["studentId"] = null;
                    }

                    if (instructor != null)
                    {
                        Session["instructorId"] = instructor.Id;
                        Session["userType"] = "instructor";
                    }
                    else
                    {
                        Session["instructorId"] = null;
                    }

                    if (admin != null)
                    {
                        Session["adminId"] = admin.Id;
                        Session["userType"] = "admin";
                    }
                    else
                    {
                        Session["adminId"] = null;
                    }
                }
                else
                {
                    return View();
                }

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
