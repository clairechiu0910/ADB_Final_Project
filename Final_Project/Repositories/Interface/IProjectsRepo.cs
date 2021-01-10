using System;
using System.Collections.Generic;
using Final_Project.Models;

namespace Final_Project.Repositories.Interface
{
    public interface IProjectsRepo
    {
        Project GetProjectById(int id);
        List<Project> GetAllProjects();
    }
}
