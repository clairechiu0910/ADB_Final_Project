using Final_Project.Models;
using Final_Project.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IProjectsRepo _projectsRepo;
        private readonly IUHavePRepo _uHavePRepo;
        private readonly IEquipmentRepo _equipmentRepo;
        public ProjectsController(IProjectsRepo projectRepo, IUHavePRepo uHavePRepo, IEquipmentRepo equipmentRepo)
        {
            _projectsRepo = projectRepo;
            _uHavePRepo = uHavePRepo;
            _equipmentRepo = equipmentRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Project(string pid)
        {
            var project = _projectsRepo.GetProjectById(pid);
            return View(project);
        }

        public IActionResult YourProjects()
        {
            var uid = HttpContext.Session.GetString("UID");
            return View();
        }

        public IActionResult Join(string pid)
        {
            var username = HttpContext.Session.GetString("UserName");
            _uHavePRepo.JoinProject(username, pid);
            //_equipmentRepo.CreateInterest(username, pid);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult InterestAllTargets(string pid)
        {
            var username = HttpContext.Session.GetString("UserName");
            _equipmentRepo.CreateInterest(username, pid);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult InterestTarget(string pid, string tid)
        {
            var username = HttpContext.Session.GetString("UserName");
            _equipmentRepo.CreateSingleInterest(username, pid, tid);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Add()
        {
            return View("Edit", new Project());
        }

        public IActionResult Edit(string pid)
        {
            var project = _projectsRepo.GetProjectById(pid);
            if (project.PI != HttpContext.Session.GetString("UID"))
            {
                return RedirectToAction("Project", new { pid = pid });
            }
            return View(project);
        }

        [HttpPost]
        public IActionResult Edit(Project project)
        {
            var valid = true;
            if (project.Project_type != "regular" && project.Project_type != "transient")
            {
                ViewBag.errMsg = "Project type must be regular or transient.";
                valid = false;
            }
            if (project.Title == null)
            {
                ViewBag.errMsg += "\nPlease input project title.";
                valid = false;
            }
            if (!valid)
            {
                return View("Edit", project);
            }

            if (project.PID == null)
            {
                //ADD
                var uid = HttpContext.Session.GetString("UID");
                project.PI = uid;
                _projectsRepo.InsertProject(project);
                return RedirectToAction("Project", "Projects", new { pid = project.PID });
            }
            else
            {
                //EDIT
                _projectsRepo.UpdateProject(project);
                return RedirectToAction("Project", "Projects", new { pid = project.PID });
            }
        }

        public IActionResult GetProjectTargets(string pid, string uid)
        {
            var projects = _projectsRepo.GetTargetsByProject(pid, uid);
            return Json(projects);
        }

        public IActionResult GetRecommendedProjects()
        {
            var uid = HttpContext.Session.GetString("UID");
            var projects = _projectsRepo.GetRecommendedProjects(uid);
            return Json(projects);
        }

        public IActionResult GetProjectsByUsername()
        {
            var username = HttpContext.Session.GetString("UserName");
            var projects = _projectsRepo.GetProjectsByUsername(username);
            return Json(projects);
        }

        public IActionResult GetYourProjects()
        {
            var uid = HttpContext.Session.GetString("UID");
            var projects = _projectsRepo.GetYourProjects(uid);
            return Json(projects);
        }
    }
}