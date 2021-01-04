using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Final_Project.Models;
using Final_Project.Repositories.Interface;

namespace Final_Project.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileRepo _profileRepo;

        public ProfileController(IProfileRepo profileRepo)
        {
            _profileRepo = profileRepo;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            var profile = new Profile();
            ViewBag.IsEdit = false;
            return View("EditProfile", profile);
        }

        [Authorize]
        public IActionResult EditProfile()
        {
            var account = HttpContext.Session.GetString("Account");
            var profile = _profileRepo.GetProfile(account);
            ViewBag.IsEdit = true;
            return View(profile);
        }

        [HttpPost]
        public IActionResult EditProfile(Profile profile)
        {
            _profileRepo.InsertProfile(profile);
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult GetProfile()
        {
            var account = HttpContext.Session.GetString("Account");
            var profile = _profileRepo.GetProfile(account);

            var tmpList = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Account", profile?.Account),
                new Tuple<string, string>("Name", profile?.Name),
                new Tuple<string, string>("Address", profile?.Address),
                new Tuple<string, string>("Phone", profile?.Phone),
                new Tuple<string, string>("Email", profile?.Email)
            };

            return Json(tmpList);
        }
    }
}
