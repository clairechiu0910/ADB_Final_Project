using System;
using System.Collections.Generic;
using Final_Project.Models;

namespace Final_Project.Repositories.Interface
{
    public interface IProjectsRepo
    {
        List<Project> GetProjectsByUsername(string uid);
        Project GetProjectById(string pid);
        List<Target> GetTargetsByProject(string pid, string uid);
        List<Project> GetYourProjects(string uid);
        void InsertProject(Project project);
        void UpdateProject(Project project);
        int CountNodes();
        List<Project> GetRecommendedProjects(string uid);
    }
}
