using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Neo4j.Driver;
using Final_Project.Models;
using Final_Project.Repositories.Interface;

namespace Final_Project.Repositories.Implementation_Neo4j
{
    public class ProjectsRepoNeo4j : IProjectsRepo
    {
        private readonly IDriver _driver;
        private readonly ISession Session;

        public ProjectsRepoNeo4j(IConfiguration configuration)
        {
            var url = configuration.GetValue<string>("Database:Url");
            var user = configuration.GetValue<string>("Database:User");
            var password = configuration.GetValue<string>("Database:Password");
            _driver = GraphDatabase.Driver(url, AuthTokens.Basic(user, password));
            Session = _driver.Session();
        }

        public List<Project> GetAllProjects()
        {
            throw new NotImplementedException();
        }

        public Project GetProjectById(string pid)
        {
            var result = Session.Run(@"MATCH (p:Project {PID: $PID}) 
                                       RETURN p.PID + ' , ' + p.title + ' , ' + p.project_type
                                        + ' , ' +  p.PI + ' , ' + p.description + ' , ' + p.aperture_upper_limit
                                        + ' , ' + p.aperture_lower_limit + ' , ' + p.FoV_upper_limit + ' , ' + p.FoV_lower_limit + ' , ' + p.pixel_scale_upper_limit + ' , ' + p.pixel_scale_lower_limit
                                        + ' , ' + p.mount_type + ' , ' + p.camera_t1 + ' , ' + p.camera_t2 + ' , ' + p.Johnson_B
                                        + ' , ' + p.Johnson_V + ' , ' + p.Johnson_R + ' , ' + p.SDSS_u + ' , ' + p.SDSS_g + ' , ' + p.SDSS_r + ' , ' + p.SDSS_i + ' , ' + p.SDSS_z",
                                       new { PID = pid });

            var msg = result.Single()[0].As<string>();
            var project = new Project(msg);
            return project;
        }
    }
}
