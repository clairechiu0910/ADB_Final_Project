using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Final_Project.Repositories.Interface;
using Final_Project.Models;

namespace Final_Project.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectsRepo _projectsRepo;
        public ProjectsController(IProjectsRepo projectRepo)
        {
            _projectsRepo = projectRepo;
        }

        public IActionResult Index()
        {
            return View();
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
            return RedirectToAction("Index");
        }

        public IActionResult GetProjects()
        {
            var projects = _projectsRepo.GetAllProjects();
            return Json(projects);
        }
    }
}