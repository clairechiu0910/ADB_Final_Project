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
            var account = HttpContext.Session.GetString("Account");
            var user = _userRepo.GetProfile(account);
            ViewBag.IsEdit = true;
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            if (user.Account == null)
            {
                // Register
                user.Account = HttpContext.Session.GetString("Account");
                _userRepo.UpdateProfile(user);
            }
            else
            {
                _userRepo.InsertProfile(user);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult GetUser()
        {
            var account = HttpContext.Session.GetString("Account");
            var user = _userRepo.GetProfile(account);

            var tmpList = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Account", user?.Account),
                new Tuple<string, string>("Name", user?.Name),
                new Tuple<string, string>("Address", user?.Address),
                new Tuple<string, string>("Phone", user?.Phone),
                new Tuple<string, string>("Email", user?.Email)
            };

            return Json(tmpList);
        }
    }
}
