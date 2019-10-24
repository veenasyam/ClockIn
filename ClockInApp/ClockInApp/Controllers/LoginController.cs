using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClockInApp.Models;
using Microsoft.AspNetCore.Http;

namespace ClockInApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<User> dbData = new List<User>();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(User objUser) {
            dbData.Add(new Models.User { UserID = 1, UserName = "veena", Password = "veena" });
            dbData.Add(new Models.User { UserID = 2, UserName = "JaneDoe", Password = "janedoe" });
            dbData.Add(new Models.User { UserID = 3, UserName = "JackieTest", Password = "jackie" });
            dbData.Add(new Models.User { UserID = 4, UserName = "JackTest", Password = "jack" });

            if (ModelState.IsValid) {
                var obj = dbData.Where(a => a.UserName.Equals(objUser.UserName) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                if (obj != null) {
                    HttpContext.Session.SetString("UserID", obj.UserID.ToString());
                    HttpContext.Session.SetString("UserName", obj.UserName.ToString());
                    HttpContext.Session.SetString("Password", obj.Password.ToString());
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(objUser);
        }

        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}