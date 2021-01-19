using Final_Project.Models;
using Final_Project.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Final_Project.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepo _userRepo;

        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            var user = new User();
            ViewBag.IsEdit = false;
            return View("EditUser", user);
        }

        [Authorize]
        public IActionResult EditUser()
        {
            var username = HttpContext.Session.GetString("UserName");
            var user = _userRepo.GetUser(username);
            ViewBag.IsEdit = true;
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            if (user.Username == null)
            {
                // Register
                user.Username = HttpContext.Session.GetString("UserName");
                _userRepo.UpdateUser(user);
            }
            else
            {
                if (_userRepo.IfUserNameExist(user.Username))
                {
                    ViewBag.errMsg = "This username have already exist.";
                    ViewBag.IsEdit = false;
                    return View("EditUser", user);
                }
                _userRepo.InsertUser(user);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult GetUser()
        {
            var username = HttpContext.Session.GetString("UserName");
            var user = _userRepo.GetUser(username);

            var tmpList = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Username", user?.Username),
                new Tuple<string, string>("Name", user?.Name),
                new Tuple<string, string>("Email", user?.Email),
                new Tuple<string, string>("Country", user?.Country),
                new Tuple<string, string>("Title", user?.Title),
                new Tuple<string, string>("Affiliation", user?.Affiliation),
            };

            return Json(tmpList);
        }

        public IActionResult GetProjectManager(string uid)
        {
            var manager = _userRepo.GetUserByUID(uid);
            manager.RemovePrivateData();
            return Json(manager);
        }
    }
}
