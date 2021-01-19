using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Final_Project.Repositories.Interface;
using Final_Project.Models;


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

        public IActionResult Add()
        {
            return View("Edit", new Equipment());
        }


        public IActionResult Edit(string eid)
        {
            var equipment = _equipmentRepo.GetEquipmentByEID(eid);
            return View(equipment);
        }

        [HttpPost]
        public IActionResult Edit(Equipment equipment)
        {
            equipment.UID = HttpContext.Session.GetString("UID");
            if(equipment.EID == null)
            {
                //ADD
                _equipmentRepo.AddEquipment(equipment);
            }
            else
            {
                //Edit
                _equipmentRepo.UpdateEquipment(equipment);
            }
            return RedirectToAction("Index");
        }

        public IActionResult EquipmentSchedule()
        {
            return View();
        }

        public IActionResult Compute()
        {
            var username = HttpContext.Session.GetString("UserName");
            _equipmentRepo.ComputeDeclination(username);
            return RedirectToAction("Index");
        }

        public IActionResult GetUserEquipments()
        {
            var username = HttpContext.Session.GetString("UserName");
            var equipments = _equipmentRepo.GetUserEquipment(username);
            return Json(equipments);
        }
        public IActionResult GetEquipmentSchedule()
        {
            var username = HttpContext.Session.GetString("UserName");
            var schedule = _equipmentRepo.GetEquipmentSchedule(username);
            return Json(schedule);
        }
        public IActionResult DetailedSchedule()
        {
            return View();
        }
        public IActionResult GetDetailedSchedule()
        {
            var username = HttpContext.Session.GetString("UserName");
            var schedule = _equipmentRepo.GetDetailedSchedule(username);
            return Json(schedule);
        }
    }
}
