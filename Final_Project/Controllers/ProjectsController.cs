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
            return View("Edit");
        }

        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Projects projects)
        {
            return RedirectToAction("Index");
        }

        public IActionResult GetProjects()
        {
            var projects = _projectsRepo.GetAllProjects();
            return Json(projects);
        }
    }
}