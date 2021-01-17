using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Final_Project.Models;
using Final_Project.Repositories.Interface;

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
            var account = HttpContext.Session.GetString("UserName");
            var user = _userRepo.GetUser(account);
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
                _userRepo.InsertUser(user);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult GetUser()
        {
            var account = HttpContext.Session.GetString("UserName");
            var user = _userRepo.GetUser(account);

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
    }
}
