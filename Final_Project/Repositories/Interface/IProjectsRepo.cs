using System;
using System.Collections.Generic;
using Final_Project.Models;

namespace Final_Project.Repositories.Interface
{
    public interface IProjectsRepo
    {
        List<Project> GetAllProjects();
        Project GetProjectById(string pid);
        void InsertProject(Project project);
        void UpdateProject(Project project);
        int CountNodes();
    }
}
