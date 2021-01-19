using Final_Project.Models;
using Final_Project.Repositories.Interface;
using Microsoft.Extensions.Configuration;
using Neo4j.Driver;
using System.Collections.Generic;
using System.IO;

namespace Final_Project.Repositories.Implementation_Neo4j
{
    public class EquipmentRepo : IEquipmentRepo
    {
        private readonly IDriver _driver;
        private readonly ISession Session;
        private readonly AstroCalculations astro;
        readonly string pythonPath;
        readonly string astroPath;
        readonly string decPath;
        public EquipmentRepo(IConfiguration configuration)
        {
            var url = configuration.GetValue<string>("Database:Url");
            var user = configuration.GetValue<string>("Database:User");
            var password = configuration.GetValue<string>("Database:Password");
            _driver = GraphDatabase.Driver(url, AuthTokens.Basic(user, password));
            Session = _driver.Session();
            pythonPath = configuration.GetValue<string>("Python:PythonPath");
            astroPath = configuration.GetValue<string>("Python:AstroPath");
            decPath = configuration.GetValue<string>("Python:DecPath");
            astro = new AstroCalculations(pythonPath, astroPath, decPath);
        }

        public List<Equipment> GetUserEquipment(string username)
        {
            var result = Session.Run(@"MATCH p = (u:User {username: $username})-[r:User_To_Equipment]->(e:Equipment)
                                       RETURN 
                                       COALESCE(e.UID,'') + ',' + 
                                       COALESCE(e.EID,'') + ',' + 
                                       COALESCE(e.UhaveE_ID,'') + ',' + 

                                       COALESCE(e.site,'') + ',' + 
                                       COALESCE(e.longitude,'') + ',' + 
                                       COALESCE(e.latitude,'') + ',' + 
                                       COALESCE(e.altitude,'') + ',' + 

                                       COALESCE(e.time_zone,'') + ',' + 
                                       COALESCE(e.daylight_saving,'') + ',' + 
                                       COALESCE(e.water_vapor,'') + ',' + 
                                       COALESCE(e.light_pollution,'') + ',' + 
                                        
                                       COALESCE(e.aperture,'') + ',' + 
                                       COALESCE(e.FoV,'') + ',' + 
                                       COALESCE(e.pixel_scale,'') + ',' + 
                                       COALESCE(e.tracking_accuracy,'') + ',' + 

                                       COALESCE(e.limiting_magnitude,'') + ',' + 
                                       COALESCE(e.elevation_limit,'') + ',' + 
                                       COALESCE(e.mount_type,'') + ',' + 

                                       COALESCE(e.camera_t1,'') + ',' + 
                                       COALESCE(e.camera_t2,'') + ',' + 
                                       
                                       COALESCE(e.Johnson_B,'') + ',' + 
                                       COALESCE(e.Johnson_V,'') + ',' + 
                                       COALESCE(e.Johnson_R,'') + ',' + 

                                       COALESCE(e.SDSS_u,'') + ',' + 
                                       COALESCE(e.SDSS_g,'') + ',' + 
                                       COALESCE(e.SDSS_r,'') + ',' + 
                                       COALESCE(e.SDSS_i,'') + ',' + 
                                       COALESCE(e.SDSS_z,'') + ',' + 

                                       COALESCE(r.declination_limit,'')    
                                       as msg",
                                      new { username });

            var equipmentsList = new List<Equipment>();

            foreach (var record in result)
            {
                var msg = record["msg"].ToString();
                var newEquipment = new Equipment(msg);
                equipmentsList.Add(newEquipment);
            }

            equipmentsList.Sort(delegate (Equipment x, Equipment y)
            {
                return y.EID.CompareTo(x.EID);
            });

            return equipmentsList;
        }

        public void CreateInterest(string username, string PID)
        {
            var result = Session.Run(@"MATCH p = (u:User {username: $username})-[:User_To_Equipment]->(r:Equipment)
                                       RETURN COALESCE(r.UhaveE_ID,'') + ',' + COALESCE(r.UID,'') + ',' + COALESCE(r.EID,'') + ',' + COALESCE(r.altitude,'') + ',' + COALESCE(r.daylight_saving,'') + ',' + 
                                       COALESCE(r.latitude,'') +','+ COALESCE(r.longitude,'') +','+ COALESCE(r.site,'') +','+ COALESCE(r.time_zone,'') +','+ COALESCE(r.water_vapor,'') + ',' + COALESCE(r.light_pollution,'') as msg, r.elevation_limit as elev_limit",
                                      new { username });

            var equipmentsList = new List<UHaveE>();
            List<string> elevLimit = new List<string>();
            foreach (var record in result)
            {
                var msg = record["msg"].ToString();
                var newEquipment = new UHaveE(msg);
                equipmentsList.Add(newEquipment);
                elevLimit.Add(record["elev_limit"].ToString());
            }

            equipmentsList.Sort(delegate (UHaveE x, UHaveE y)
            {
                return y.EID.CompareTo(x.EID);
            });

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
            for (int i = 0; i < equipmentsList.Count; i++)
            {
                foreach (Target targ in targetList)
                {
                    createInterestRelationship(targ.TID, PID, equipmentsList, elevLimit, targ, i);
                }
            }

        }

        public void CreateSingleInterest(string username, string PID, string TID)
        {
            var result = Session.Run(@"MATCH p = (u:User {username: $username})-[:User_To_Equipment]->(r:Equipment)
                                       RETURN COALESCE(r.UhaveE_ID,'') + ',' + COALESCE(r.UID,'') + ',' + COALESCE(r.EID,'') + ',' + COALESCE(r.altitude,'') + ',' + COALESCE(r.daylight_saving,'') + ',' + 
                                       COALESCE(r.latitude,'') +','+ COALESCE(r.longitude,'') +','+ COALESCE(r.site,'') +','+ COALESCE(r.time_zone,'') +','+ COALESCE(r.water_vapor,'') + ',' + COALESCE(r.light_pollution,'') as msg, COALESCE(r.elevation_limit,'') as elev_limit",
                                      new { username });

            var equipmentsList = new List<UHaveE>();
            List<string> elevLimit = new List<string>();
            foreach (var record in result)
            {
                var msg = record["msg"].ToString();
                var newEquipment = new UHaveE(msg);
                equipmentsList.Add(newEquipment);
                elevLimit.Add(record["elev_limit"].ToString());
            }

            equipmentsList.Sort(delegate (UHaveE x, UHaveE y)
            {
                return y.EID.CompareTo(x.EID);
            });

            var targets = Session.Run(@"MATCH p = (a:Project{PID:$PID})-[:Project_To_Target]->(t:Target{TID:$TID})
                                       RETURN COALESCE(t.TID,'') + ',' + COALESCE(t.Name,'') + ',' + COALESCE(t.RA,'') + ',' + COALESCE(t.Dec,'') as msg",
                                       new { PID, TID });

            var targetList = new List<Target>();

            foreach (var record in targets)
            {
                var msg = record["msg"].ToString();
                var newTarget = new Target(msg);
                targetList.Add(newTarget);
            }
            for (int i = 0; i < equipmentsList.Count; i++)
            {
                foreach (Target targ in targetList)
                {
                    createInterestRelationship(TID, PID, equipmentsList, elevLimit, targ, i);


                }
            }

        }
        private void createInterestRelationship(string TID, string PID, List<UHaveE> equipmentsList, List<string> elevLimit, Target targ, int i)
        {
            /*
                     * # - longitude (deg)
                    # - latitude (deg)
                    # - altitude (m)
                    # - elevation limit of equipment (deg)
                    # 
                    # **Target**
                    # - TID
                    # - RA (longitude) (deg)
                    # - Dec (latitude) (deg)
                    */
            if (!File.Exists(pythonPath))
            {
                //create the log
                System.Console.WriteLine("pythonPath does not exists");
            }
            string[] astroResults = astro.AstroplanCalculations(equipmentsList[i].Longitude, equipmentsList[i].Latitude, equipmentsList[i].Altitude, elevLimit[i], targ.RA, targ.Dec);
            string EID = equipmentsList[i].EID;
            //if not 2 meaning incompatible
            if (astroResults.Length == 2)
            {

                string start = astroResults[0];
                string end = astroResults[1];
                var delete = Session.Run(@"MATCH (e:Equipment {EID:$EID})-[r:Interested]-(t:Target {TID:$TID})
                                               DELETE r", new { EID, TID });
                var create = Session.Run(@"MATCH (e:Equipment {EID:$EID}), (t:Target {TID:$TID})
                                               MERGE (e)-[:Interested {PID:$PID,start:$start,end:$end} ]->(t)", new { EID, TID, PID, start, end });
            }
            else
            {
                var create = Session.Run(@"MATCH (e:Equipment {EID:$EID}), (t:Target {TID:$TID})
                                               MERGE (e)-[:NotInterested {PID:$PID} ]->(t)", new { EID, TID, PID });
            }
        }

        public void ComputeDeclination(string username)
        {
            var result = Session.Run(@"MATCH p = (u:User {username: $username})-[:User_To_Equipment]->(r:Equipment)
                                       RETURN COALESCE(r.UhaveE_ID,'') + ',' + COALESCE(r.UID,'') + ',' + COALESCE(r.EID,'') + ',' + COALESCE(r.altitude,'') + ',' + COALESCE(r.daylight_saving,'') + ',' + 
                                       COALESCE(r.latitude,'') +','+ COALESCE(r.longitude,'') +','+ COALESCE(r.site,'') +','+ COALESCE(r.time_zone,'') +','+ COALESCE(r.water_vapor,'') + ',' + COALESCE(r.light_pollution,'') as msg, r.elevation_limit as elev_limit",
                         new { username });

            var equipmentsList = new List<UHaveE>();
            List<string> elevLimit = new List<string>();
            foreach (var record in result)
            {
                var msg = record["msg"].ToString();
                var newEquipment = new UHaveE(msg);
                equipmentsList.Add(newEquipment);
                elevLimit.Add(record["elev_limit"].ToString());
            }

            equipmentsList.Sort(delegate (UHaveE x, UHaveE y)
            {
                return y.EID.CompareTo(x.EID);
            });

            for (int i = 0; i < equipmentsList.Count; i++)
            {
                string decLimit, UhaveE_ID;
                UhaveE_ID = equipmentsList[i].UhaveE_ID;
                decLimit = astro.Declinationlimit(equipmentsList[i].Longitude, equipmentsList[i].Latitude, equipmentsList[i].Altitude, elevLimit[i]);
                var insertDec = Session.Run(@"MATCH (r:Equipment{UhaveE_ID:$UhaveE_ID})
                                              SET r.declination_limit = $decLimit", new { UhaveE_ID, decLimit });
            }
        }
    }
}
