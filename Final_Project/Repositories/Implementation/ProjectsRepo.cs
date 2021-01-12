using Final_Project.Repositories.Interface;
using Final_Project.Models;
using System.Collections.Generic;

namespace Final_Project.Repositories.Implementation
{
    public class ProjectsRepo : IProjectsRepo
    {
        public ProjectsRepo() { }

        public List<Project> GetAllProjects()
        {
            var allProjects = new List<Project>()
            {
                new Project(){ PID = 1, Title = "Observe galaxy 1", Project_type = "regulat", Description = "Observe galaxy 1 55 times" },
                new Project(){ PID = 2, Title = "Observe galaxy 2", Project_type = "regulat", Description = "Observe galaxy 2 97 times" },
                new Project(){ PID = 3, Title = "Observe galaxy 3", Project_type = "transient", Description = "Observe Supernova 1 96 time" }
            };
            return allProjects;
        }

        public Project GetProjectById(int id)
        {
            return new Project() { PID = 1, Title = "Observe galaxy 1", Project_type = "regulat", Description = "Observe galaxy 1 55 times" };
        }
    }
}
