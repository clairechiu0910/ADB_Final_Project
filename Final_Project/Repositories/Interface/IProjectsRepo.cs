using Final_Project.Models;
using System.Collections.Generic;

namespace Final_Project.Repositories.Interface
{
    public interface IProjectsRepo
    {
        List<Project> GetProjectsByUsername(string uid);
        Project GetProjectById(string pid);
        List<Project> GetYourProjects(string uid);
        void InsertProject(Project project);
        void UpdateProject(Project project);
        int CountNodes();

        List<Project> GetRecommendedProjects(string uid);
        List<Target> GetTargetsByProject(string pid, string uid);

        //With Targets
        void AddTargetToProject(string pid, string tid);
        void RemoveTargetFromProject(string pid, string tid);
        List<Target> GetTargetsInRange(int RA_Lower, int RA_Upper, int DEC_Lower, int DEC_Upper);
    }
}
