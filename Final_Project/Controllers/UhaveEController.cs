using Final_Project.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class UhaveEController : Controller
    {
        private readonly IUHaveERepo _uHaveERepo;

        public UhaveEController(IUHaveERepo uHaveERepo)
        {
            _uHaveERepo = uHaveERepo;
        }

        public IActionResult Compute()
        {
            var username = HttpContext.Session.GetString("UserName");
            _uHaveERepo.ComputeDeclination(username);
            return RedirectToAction("Index", "UserEquipments");
        }
    }
}
