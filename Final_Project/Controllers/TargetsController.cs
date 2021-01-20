using Final_Project.Models;
using Final_Project.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    [Authorize]
    public class TargetsController : Controller
    {
        private readonly IProjectsRepo _projectsRepo;
        public TargetsController(IProjectsRepo projectsRepo)
        {
            _projectsRepo = projectsRepo;
        }

        public IActionResult SelectTargets(string pid)
        {
            var defaultRange = new SelectTargetModel(pid);
            var targetList = _projectsRepo.GetTargetsInRange(defaultRange.RA_Lower, defaultRange.RA_Upper, defaultRange.DEC_Lower, defaultRange.DEC_Upper);
            defaultRange.TargetsInRange = targetList;
            return View(defaultRange);
        }

        [HttpPost]
        public IActionResult SelectTargets(SelectTargetModel selectTarget)
        {
            var targetList = _projectsRepo.GetTargetsInRange(selectTarget.RA_Lower, selectTarget.RA_Upper, selectTarget.DEC_Lower, selectTarget.DEC_Upper);
            selectTarget.TargetsInRange = targetList;
            return View(selectTarget);
        }

        public IActionResult AddTargetToProject(string pid, string tid)
        {
            _projectsRepo.AddTargetToProject(pid, tid);
            return RedirectToAction("Project", "Projects", new { pid = pid });
        }
    }
}