﻿using Final_Project.Models;
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

        public List<Target> GetTargetsByProject(string pid, string uid)
        {
            string PID = pid;
            string UID = uid;
            /*
            var targets = Session.Run(@"MATCH p = (a:Project{PID:$PID})-[:Project_To_Target]->(t:Target)
                                       RETURN COALESCE(t.TID,'') + ',' + COALESCE(t.Name,'') + ',' + COALESCE(t.RA,'') + ',' + COALESCE(t.Dec,'') as msg",
                           new { PID });
            */
            var recommend = Session.Run(@"MATCH p = (a:Project{PID:$PID})-[r:Project_To_Target]->(t:Target), (u:User{UID:$UID})-[r2:User_To_Equipment]-(e:Equipment)
                                        WITH DISTINCT(t), e,
                                        CASE WHEN e.SDSS_g = r.SDSS_g  THEN 1 ELSE 0 END AS SDSS_g,
                                        CASE WHEN e.SDSS_u = r.SDSS_u  THEN 1 ELSE 0 END AS SDSS_u,
                                        CASE WHEN e.SDSS_r = r.SDSS_r  THEN 1 ELSE 0 END AS SDSS_r,
                                        CASE WHEN e.SDSS_z = r.SDSS_z  THEN 1 ELSE 0 END AS SDSS_z,
                                        CASE WHEN e.SDSS_i = r.SDSS_i  THEN 1 ELSE 0 END AS SDSS_i,
                                        CASE WHEN e.Johnson_B = r.Johnson_B  THEN 1 ELSE 0 END AS Johnson_B,
                                        CASE WHEN e.Johnson_R = r.Johnson_R  THEN 1 ELSE 0 END AS Johnson_R,
                                        CASE WHEN e.Johnson_V = r.Johnson_V  THEN 1 ELSE 0 END AS Johnson_V
                                        WITH t, e, COALESCE(t.TID, '') + ',' + COALESCE(t.Name, '') + ',' + COALESCE(t.RA, '') + ',' + COALESCE(t.Dec, '') as msg,
                                        MAX(SDSS_g + SDSS_u + SDSS_r + SDSS_z + SDSS_i + Johnson_B + Johnson_R + Johnson_V) as total
                                        WHERE total >= 5
                                        MERGE(e) -[:Recommend{total:total}]->(t)
                                        RETURN t, e, total
                                        ",
                                        new { PID, UID });
            var score = Session.Run(@"MATCH p = (a:Project{PID:$PID})-[r:Project_To_Target]->(t:Target), (u:User{UID:$UID})-[r2:User_To_Equipment]-(e:Equipment)
                                        WITH DISTINCT(t), e,
                                        CASE WHEN e.SDSS_g = r.SDSS_g  THEN 1 ELSE 0 END AS SDSS_g,
                                        CASE WHEN e.SDSS_u = r.SDSS_u  THEN 1 ELSE 0 END AS SDSS_u,
                                        CASE WHEN e.SDSS_r = r.SDSS_r  THEN 1 ELSE 0 END AS SDSS_r,
                                        CASE WHEN e.SDSS_z = r.SDSS_z  THEN 1 ELSE 0 END AS SDSS_z,
                                        CASE WHEN e.SDSS_i = r.SDSS_i  THEN 1 ELSE 0 END AS SDSS_i,
                                        CASE WHEN e.Johnson_B = r.Johnson_B  THEN 1 ELSE 0 END AS Johnson_B,
                                        CASE WHEN e.Johnson_R = r.Johnson_R  THEN 1 ELSE 0 END AS Johnson_R,
                                        CASE WHEN e.Johnson_V = r.Johnson_V  THEN 1 ELSE 0 END AS Johnson_V
                                        WITH t, e, COALESCE(t.TID, '') + ',' + COALESCE(t.Name, '') + ',' + COALESCE(t.RA, '') + ',' + COALESCE(t.Dec, '') as msg,
                                        MAX(SDSS_g + SDSS_u + SDSS_r + SDSS_z + SDSS_i + Johnson_B + Johnson_R + Johnson_V) as total
                                        MERGE(e) -[:Score{total:total}]->(t)
                                        RETURN t, e, total
                                        ",
            new { PID, UID });
            var targets = Session.Run(@"MATCH p = (a:Project{PID:$PID})-[:Project_To_Target]->(t:Target)<-[s:Score]-(e:Equipment)-[:User_To_Equipment]-(u:User{UID:$UID})
                                        WITH DISTINCT(t), MAX(s.total) as total_score
                                        RETURN COALESCE(t.TID, '') + ',' + COALESCE(t.Name, '') + ',' + COALESCE(t.RA, '') + ',' + COALESCE(t.Dec, '') as msg, total_score
                                        ORDER BY total_score DESC",
                           new { PID , UID });
            var targetList = new List<Target>(); 
            foreach (var record in targets)
            {
                var msg = record["msg"].ToString();
                var targetscore = record["total_score"].ToString();
                var newTarget = new Target(msg, targetscore);
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
