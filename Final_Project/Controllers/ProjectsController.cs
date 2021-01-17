using Final_Project.Models;
using Final_Project.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectsRepo _projectsRepo;
        private readonly IUHavePRepo _uHavePRepo;

        public ProjectsController(IProjectsRepo projectRepo, IUHavePRepo uHavePRepo)
        {
            _projectsRepo = projectRepo;
            _uHavePRepo = uHavePRepo;
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

        [Authorize]
        public IActionResult Join(string pid)
        {
            var username = HttpContext.Session.GetString("UserName");
            _uHavePRepo.JoinProject(username, pid);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Add()
        {
            return View("Edit", new Project());
        }

        public IActionResult Edit(string pid)
        {
            var project = _projectsRepo.GetProjectById(pid);
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
                _projectsRepo.InsertProject(project);
                return RedirectToAction("Index");
            }
            else
            {
                //EDIT
                _projectsRepo.UpdateProject(project);
                return RedirectToAction("Project", "Projects", new { pid = project.PID });
            }
        }

        public IActionResult GetProjects()
        {
            var projects = _projectsRepo.GetAllProjects();
            return Json(projects);
        }

        [Authorize]
        public IActionResult GetProjectsByUsername()
        {
            var username = HttpContext.Session.GetString("UserName");
            var projects = _projectsRepo.GetProjectsByUsername(username);
            return Json(projects);
        }
    }
}