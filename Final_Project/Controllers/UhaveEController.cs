using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Final_Project.Repositories.Interface;

namespace Final_Project.Controllers
{
    public class UhaveEController : Controller
    {
        private readonly IUHaveERepo _uHaveERepo;

        public UhaveEController(IUHaveERepo uHaveERepo)
        {
            _uHaveERepo = uHaveERepo;
        }

        public IActionResult GetUHaveE()
        {
            var username = HttpContext.Session.GetString("UserName");
            var uHaveEList = _uHaveERepo.GetUHaveE(username);
            return Json(uHaveEList);
        }
        public IActionResult Compute()
        {
            var username = HttpContext.Session.GetString("UserName");
            _uHaveERepo.ComputeDeclination(username);
            return RedirectToAction("Index", "UserEquipments");
        }
    }
}
