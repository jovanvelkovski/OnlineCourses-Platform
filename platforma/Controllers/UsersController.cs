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

                string registerQuery = "INSERT INTO platforma.users(firstname, lastname, sex, email, username, password) " +
                    "VALUES (@p0, @p1, @p2, @p3, @p4, @p5) ";

                int output = db.Database.ExecuteSqlCommand(registerQuery, allItems);

                return RedirectToAction("Index", "Home");
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
                    Session["userType"] = "student";

                    string queryName = "SELECT * " +
                        "FROM platforma.users u " +
                        "WHERE u.username = \'" + user.Username + "\'";
                    var person = db.Users.SqlQuery(queryName).SingleOrDefault();
                    Session["firstName"] = person.FirstName;
                    Session["username"] = person.Username;
                    Session["userId"] = person.Id;
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
