using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Final_Project.Repositories.Interface;


namespace Final_Project.Controllers
{
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

    }
}
