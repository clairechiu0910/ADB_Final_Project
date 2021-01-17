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
    [Authorize]
    public class UserEquipmentsController : Controller
    {
        private readonly IEquipmentRepo _equipmentRepo;

        public UserEquipmentsController(IEquipmentRepo equipmentRepo)
        {
            _equipmentRepo = equipmentRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetUserEquipments()
        {
            var username = HttpContext.Session.GetString("UserName");
            var equipments = _equipmentRepo.GetUserEquipment(username);
            return Json(equipments);
        }
    }
}
