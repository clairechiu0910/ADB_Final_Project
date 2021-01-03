using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Final_Project.Models;

namespace Final_Project.Controllers
{
    public class ProfileController : Controller
    {
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
            return RedirectToAction("Index");
        }

        public IActionResult GetProfile()
        {
            var tmp = new Profile()
            {
                Username = "hello",
                Name = "Mary",
                Address = "hell, lkdfji, liseir",
                Phone = "8881868",
                Email = "abc@thii.ckd.com"
            };

            var tmpList = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Username", tmp.Username),
                new Tuple<string, string>("Name", tmp.Name),
                new Tuple<string, string>("Address", tmp.Address),
                new Tuple<string, string>("Phone", tmp.Phone),
                new Tuple<string, string>("Email", tmp.Email)
            };

            return Json(tmpList);
        }
    }
}
