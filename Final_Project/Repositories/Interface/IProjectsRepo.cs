using System;
using System.Collections.Generic;
using Final_Project.Models;

namespace Final_Project.Repositories.Interface
{
    public interface IProjectsRepo
    {
        List<Project> GetAllProjects();
        List<Project> GetProjectsByUsername(string uid);
        Project GetProjectById(string pid);
        List<Target> GetTargetsByProject(string pid);
        void InsertProject(Project project);
        void UpdateProject(Project project);
        int CountNodes();
    }
}
