using Final_Project.Repositories.Interface;
using Final_Project.Models;
using System.Collections.Generic;

namespace Final_Project.Repositories.Implementation
{
    public class ProjectsRepo : IProjectsRepo
    {
        public ProjectsRepo() { }

        public List<Projects> GetAllProjects()
        {
            var allProjects = new List<Projects>()
            {
                new Projects(){Title = "Observe galaxy 1", Project_type = "regulat", Description = "Observe galaxy 1 55 times"},
                new Projects(){Title = "Observe galaxy 2", Project_type = "regulat", Description = "Observe galaxy 2 97 times"},
                new Projects(){Title = "Observe galaxy 3", Project_type = "transient", Description = "Observe Supernova 1 96 time"}
            };
            return allProjects;
        }
    }
}
