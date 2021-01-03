using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Final_Project.Models;
using Final_Project.Repositories.Interface;

namespace Final_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProjectsRepo _projectrepo;

        public HomeController(IProjectsRepo projectRepo)
        {
            _projectrepo = projectRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public IActionResult Login(string account, string password, string returnUrl)
        {
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GetProjects()
        {
            var projects = _projectrepo.GetAllProjects();
            return Json(projects);
        }
    }
}
