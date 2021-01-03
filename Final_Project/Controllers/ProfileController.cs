using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EditProfile()
        {
            var profile = new Profile();
            return View(profile);
        }

        [HttpPost]
        public IActionResult EditProfile(Profile profile)
        {
            _profileRepo.InsertProfile(profile);
            return RedirectToAction("Index");
        }

        public IActionResult GetProfile()
        {
            var id = 0;
            var profile = _profileRepo.GetProfile(id);

            var tmpList = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Account", profile.Account),
                new Tuple<string, string>("Name", profile.Name),
                new Tuple<string, string>("Address", profile.Address),
                new Tuple<string, string>("Phone", profile.Phone),
                new Tuple<string, string>("Email", profile.Email)
            };

            return Json(tmpList);
        }
    }
}
