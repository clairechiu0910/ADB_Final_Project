using Final_Project.Models;
using Final_Project.Repositories.Interface;
using Microsoft.Extensions.Configuration;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

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
        
        public List<Project> GetProjectsByUsername(string username)
        {
            var result = Session.Run(@"MATCH (u:User {username: $username})-[r:User_To_Project]->(p: Project)
                                       RETURN p.PID + ',' + p.title + ',' + p.project_type
                                        + ',' +  p.PI + ',' + p.description + ',' + p.aperture_upper_limit
                                        + ',' + p.aperture_lower_limit + ',' + p.FoV_upper_limit + ',' + p.FoV_lower_limit + ',' + p.pixel_scale_upper_limit + ',' + p.pixel_scale_lower_limit
                                        + ',' + p.mount_type + ',' + p.camera_t1 + ',' + p.camera_t2 + ',' + p.Johnson_B
                                        + ',' + p.Johnson_V + ',' + p.Johnson_R + ',' + p.SDSS_u + ',' + p.SDSS_g + ',' + p.SDSS_r + ',' + p.SDSS_i + ',' + p.SDSS_z as msg",
                                       new { username });
            
            var projectList = new List<Project>();
            foreach (var record in result)
            {
                var msg = record["msg"].ToString();
                projectList.Add(new Project(msg));
            }
            return projectList;
        }
        public List<Project> GetYourProjects(string uid)
        {
            var result = Session.Run(@"MATCH (p: Project {PI:$uid})
                                       RETURN p.PID + ',' + p.title + ',' + p.project_type
                                        + ',' +  p.PI + ',' + p.description + ',' + p.aperture_upper_limit
                                        + ',' + p.aperture_lower_limit + ',' + p.FoV_upper_limit + ',' + p.FoV_lower_limit + ',' + p.pixel_scale_upper_limit + ',' + p.pixel_scale_lower_limit
                                        + ',' + p.mount_type + ',' + p.camera_t1 + ',' + p.camera_t2 + ',' + p.Johnson_B
                                        + ',' + p.Johnson_V + ',' + p.Johnson_R + ',' + p.SDSS_u + ',' + p.SDSS_g + ',' + p.SDSS_r + ',' + p.SDSS_i + ',' + p.SDSS_z as msg",
                                       new { uid });

            var projectList = new List<Project>();
            foreach (var record in result)
            {
                var msg = record["msg"].ToString();
                projectList.Add(new Project(msg));
            }
            return projectList;
        }
        public Project GetProjectById(string pid)
        {
            var result = Session.Run(@"MATCH (p:Project {PID: $PID}) 
                                       RETURN p.PID + ',' + p.title + ',' + p.project_type
                                        + ',' +  p.PI + ',' + p.description + ',' + p.aperture_upper_limit
                                        + ',' + p.aperture_lower_limit + ',' + p.FoV_upper_limit + ',' + p.FoV_lower_limit + ',' + p.pixel_scale_upper_limit + ',' + p.pixel_scale_lower_limit
                                        + ',' + p.mount_type + ',' + p.camera_t1 + ',' + p.camera_t2 + ',' + p.Johnson_B
                                        + ',' + p.Johnson_V + ',' + p.Johnson_R + ',' + p.SDSS_u + ',' + p.SDSS_g + ',' + p.SDSS_r + ',' + p.SDSS_i + ',' + p.SDSS_z",
                                       new { PID = pid });

            var msg = result.Single()[0].As<string>();
            var project = new Project(msg);
            return project;
        }

        public void InsertProject(Project project)
        {
            var newId = CountNodes();
            project.PID = newId.ToString();

            var result = Session.Run(@"CREATE (p:Project) 
                                       SET p.PID = $PID, p.title = $title, 
                                       p.project_type = $project_type, p.PI = $PI, p.description = $description, 
                                       p.aperture_upper_limit = $aperture_upper_limit, p.aperture_lower_limit = $aperture_lower_limit,
                                       p.FoV_upper_limit = $FoV_upper_limit, p.FoV_lower_limit = $FoV_lower_limit, p.pixel_scale_upper_limit = $pixel_scale_upper_limit,
                                       p.pixel_scale_lower_limit = $pixel_scale_lower_limit, p.mount_type = $mount_type, p.camera_t1 = $camera_t1, p.camera_t2 = $camera_t2,
                                       p.Johnson_B = $Johnson_B, p.Johnson_V = $Johnson_V, p.Johnson_R = $Johnson_R, p.SDSS_u = $SDSS_u, p.SDSS_g = $SDSS_g,
                                       p.SDSS_r = $SDSS_r, p.SDSS_i = $SDSS_i, p.SDSS_z = $SDSS_z
                                       RETURN p.title + ' Create'",
                                       new
                                       {
                                           PID = project.PID,
                                           title = project.Title,
                                           project_type = project.Project_type,
                                           PI = project.PI,
                                           description = project.Description,
                                           aperture_upper_limit = project.ApertureUpperLimit,
                                           aperture_lower_limit = project.ApertureLowerLimit,
                                           FoV_upper_limit = project.FoVUpperLimit,
                                           FoV_lower_limit = project.FoVLowerLimit,
                                           pixel_scale_upper_limit = project.PixelScaleUpperLimit,
                                           pixel_scale_lower_limit = project.PixelScaleLowerLimit,
                                           mount_type = project.MountType,
                                           camera_t1 = project.CameraType_1,
                                           camera_t2 = project.CameraType_2,
                                           Johnson_B = project.Johnson_B,
                                           Johnson_V = project.Johnson_V,
                                           Johnson_R = project.Johnson_R,
                                           SDSS_u = project.SDSS_u,
                                           SDSS_g = project.SDSS_g,
                                           SDSS_r = project.SDSS_r,
                                           SDSS_i = project.SDSS_i,
                                           SDSS_z = project.SDSS_z
                                       });
        }

        public void UpdateProject(Project project)
        {
            var result = Session.Run(@"MATCH (p:Project {PID: $PID}) 
                                       SET p.PID = $PID, p.title = $title, 
                                       p.project_type = $project_type, p.PI = $PI, p.description = $description, 
                                       p.aperture_upper_limit = $aperture_upper_limit, p.aperture_lower_limit = $aperture_lower_limit,
                                       p.FoV_upper_limit = $FoV_upper_limit, p.FoV_lower_limit = $FoV_lower_limit, p.pixel_scale_upper_limit = $pixel_scale_upper_limit,
                                       p.pixel_scale_lower_limit = $pixel_scale_lower_limit, p.mount_type = $mount_type, p.camera_t1 = $camera_t1, p.camera_t2 = $camera_t2,
                                       p.Johnson_B = $Johnson_B, p.Johnson_V = $Johnson_V, p.Johnson_R = $Johnson_R, p.SDSS_u = $SDSS_u, p.SDSS_g = $SDSS_g,
                                       p.SDSS_r = $SDSS_r, p.SDSS_i = $SDSS_i, p.SDSS_z = $SDSS_z
                                       RETURN p.title + ' Update'",
                                       new
                                       {
                                           PID = project.PID,
                                           title = project.Title,
                                           project_type = project.Project_type,
                                           PI = project.PI,
                                           description = project.Description,
                                           aperture_upper_limit = project.ApertureUpperLimit,
                                           aperture_lower_limit = project.ApertureLowerLimit,
                                           FoV_upper_limit = project.FoVUpperLimit,
                                           FoV_lower_limit = project.FoVLowerLimit,
                                           pixel_scale_upper_limit = project.PixelScaleUpperLimit,
                                           pixel_scale_lower_limit = project.PixelScaleLowerLimit,
                                           mount_type = project.MountType,
                                           camera_t1 = project.CameraType_1,
                                           camera_t2 = project.CameraType_2,
                                           Johnson_B = project.Johnson_B,
                                           Johnson_V = project.Johnson_V,
                                           Johnson_R = project.Johnson_R,
                                           SDSS_u = project.SDSS_u,
                                           SDSS_g = project.SDSS_g,
                                           SDSS_r = project.SDSS_r,
                                           SDSS_i = project.SDSS_i,
                                           SDSS_z = project.SDSS_z
                                       });
        }

        public List<Target> GetTargetsByProject(string pid)
        {
            string PID = pid;
            /*
            var targets = Session.Run(@"MATCH p = (a:Project{PID:$PID})-[:Project_To_Target]->(t:Target)
                                       RETURN COALESCE(t.TID,'') + ',' + COALESCE(t.Name,'') + ',' + COALESCE(t.RA,'') + ',' + COALESCE(t.Dec,'') as msg",
                           new { PID });
            */
            
            var targets = Session.Run(@"MATCH p = (a:Project{PID:$PID})-[:Project_To_Target]->(t:Target)
                                       RETURN COALESCE(t.TID,'') + ',' + COALESCE(t.Name,'') + ',' + COALESCE(t.RA,'') + ',' + COALESCE(t.Dec,'') as msg",
               new { PID });

            var targetList = new List<Target>(); 
            foreach (var record in targets)
            {
                var msg = record["msg"].ToString();
                var newTarget = new Target(msg);
                targetList.Add(newTarget);
            }
            return targetList;
        }

        public int CountNodes()
        {
            var result = Session.Run(@"MATCH (n:Project) Return count(n)");
            var msg = result.Single()[0].As<string>();
            return Int32.Parse(msg);
        }
    }
}
