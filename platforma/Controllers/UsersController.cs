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
                List<object> lst = new List<object>();
                lst.Add(newUser.FirstName);
                lst.Add(newUser.LastName);
                lst.Add(newUser.Sex);
                lst.Add(newUser.Email);
                lst.Add(newUser.Username);
                lst.Add(newUser.Password);
                object[] allItems = lst.ToArray();

                //PROCEDURE
                string registerQueryUser = "INSERT INTO platforma.users(firstname, lastname, sex, email, username, password) " +
                    "VALUES (@p0, @p1, @p2, @p3, @p4, @p5) ";

                int output = db.Database.ExecuteSqlCommand(registerQueryUser, allItems);

                string queryGetId = "SELECT * FROM platforma.users WHERE username=\'" + newUser.Username + "\'";
                var user = db.Users.SqlQuery(queryGetId).SingleOrDefault();
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
                //PROCEDURE
                string registerStudent = "INSERT INTO platforma.students(userid, age, country) " +
                    "VALUES (" + id + ", " + student.Age + ", \'" + student.Country + "\')";
                int output = db.Database.ExecuteSqlCommand(registerStudent);

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
                //PROCEDURE
                string registerInstructor = "INSERT INTO platforma.instructors(userid, age, country, adminid) " +
                    "VALUES (" + id + ", " + instructor.Age + ", \'" + instructor.Country + "\', 1)"; 
                int output = db.Database.ExecuteSqlCommand(registerInstructor);

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
                string query = "SELECT platforma.checkUser(\'" + user.Username + "\', \'" + user.Password + "\')";
                int loginSuccessful = db.Database.ExecuteSqlCommand(query);
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
