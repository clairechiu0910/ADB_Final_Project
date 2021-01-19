using Final_Project.Models;
using Final_Project.Repositories.Interface;
using Microsoft.Extensions.Configuration;
using Neo4j.Driver;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

                                       COALESCE(e.declination_limit,'')    
                                       as msg",
                                      new { username });

            var equipmentsList = new List<Equipment>();

            foreach (var record in result)
            {
                var msg = record["msg"].ToString();
                var newEquipment = new Equipment(msg);
                equipmentsList.Add(newEquipment);
            }

            return equipmentsList;
        }

        public Equipment GetEquipmentByEID(string eid)
        {
            var result = Session.Run(@"MATCH (e:Equipment {EID: $eid})
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
                                       COALESCE(e.SDSS_z,'')",
                                     new { eid });

            var msg = result.Single()[0].As<string>();
            var equipment = new Equipment(msg);

            return equipment;
        }

        public void AddEquipment(Equipment equipment)
        {
            equipment.EID = Session.Run(@"MATCH (e:Equipment)
                                    RETURN count(e)").Single()[0].As<string>();

            equipment.UhaveE_ID = Session.Run(@"MATCH (ue:Equipment) RETURN count(ue)").Single()[0].As<string>();

            var result = Session.Run(@"MATCH (u:User {UID: $UID})
                                       MERGE (u)-[r:User_To_Equipment {UhaveE_ID:$UhaveE_ID}]->(e:Equipment {EID:$EID,
                                                                                                             UID: $UID,
                                                                                                             UhaveE_ID: $UhaveE_ID,
                                                                                                             aperture:$aperture,
                                                                                                             FoV:$FoV,
                                                                                                             pixel_scale:$pixel_scale,
                                                                                                             tracking_accuracy:$tracking_accuracy,
                                                                                                             limiting_magnitude:$limiting_magnitude,
                                                                                                             elevation_limit:$elevation_limit,
                                                                                                             mount_type:$mount_type,
                                                                                                             camera_t1:$camera_t1,
                                                                                                             camera_t2:$camera_t2,
                                                                                                             Johnson_B:$Johnson_B,
                                                                                                             Johnson_V:$Johnson_V,
                                                                                                             Johnson_R:$Johnson_R,
                                                                                                             SDSS_u:$SDSS_u,
                                                                                                             SDSS_g:$SDSS_g,
                                                                                                             SDSS_r:$SDSS_r,
                                                                                                             SDSS_i:$SDSS_i, 
                                                                                                             SDSS_z:$SDSS_z,
                                                                                               site:$site,
                                                                                                             longitude:$longitude,
                                                                                                            latitude:$latitude,
                                                                                                            altitude:$altitude,
                                                                                                           time_zone:$time_zone,
                                                                                                             daylight_saving:$daylight_saving,
                                                                                                          water_vapor:$water_vapor,
                                                                                        light_pollution:$light_pollution
                                                                                                })
                                       RETURN e.EID + ',' + r.UhaveE_ID + ',' + u.username as msg",

                                      new
                                      {
                                          UID = equipment.UID,
                                          EID = equipment.EID,
                                          UhaveE_ID = equipment.UhaveE_ID,
                                          aperture = equipment.Aperture,
                                          FoV = equipment.FoV,
                                          pixel_scale = equipment.PixelScale,
                                          tracking_accuracy = equipment.TrackingAccuracy,
                                          limiting_magnitude = equipment.LimitingMagnitude,
                                          elevation_limit = equipment.ElevationLimit,
                                          mount_type = equipment.MountType,
                                          camera_t1 = equipment.CameraType_1,
                                          camera_t2 = equipment.CameraType_2,
                                          Johnson_B = equipment.Johnson_B,
                                          Johnson_V = equipment.Johnson_V,
                                          Johnson_R = equipment.Johnson_R,
                                          SDSS_u = equipment.SDSS_u,
                                          SDSS_g = equipment.SDSS_g,
                                          SDSS_r = equipment.SDSS_r,
                                          SDSS_i = equipment.SDSS_i,
                                          SDSS_z = equipment.SDSS_z,
                                          site = equipment.Site,
                                          longitude = equipment.Longitude,
                                          latitude = equipment.Latitude,
                                          altitude = equipment.Altitude,
                                          time_zone = equipment.TimeZone,
                                          daylight_saving = equipment.DaylightSaving,
                                          water_vapor = equipment.WaterVapor,
                                          light_pollution = equipment.LightPollution
                                      });
        }

        public void UpdateEquipment(Equipment equipment)
        {
            var result = Session.Run(@"MATCH (u:Equipment {EID: $EID})
                                       SET u.UID = $UID,
                                           u.EID = $EID,
                                           u.UhaveE_ID = $UhaveE_ID,
                                           u.aperture = $aperture,
                                           u.FoV = $FoV,
                                           u.pixel_scale = $pixel_scale,
                                           u.tracking_accuracy = $tracking_accuracy,
                                           u.limiting_magnitude = $limiting_magnitude,
                                           u.elevation_limit = $elevation_limit,
                                           u.mount_type = $mount_type,
                                           u.camera_t1 = $camera_t1,
                                           u.camera_t2 = $camera_t2,
                                           u.Johnson_B = $Johnson_B,
                                           u.Johnson_V = $Johnson_V,
                                           u.Johnson_R = $Johnson_R,
                                           u.SDSS_u = $SDSS_u,
                                           u.SDSS_g = $SDSS_g,
                                           u.SDSS_r = $SDSS_r,
                                           u.SDSS_i = $SDSS_i, 
                                           u.SDSS_z = $SDSS_z,
                                           u.site = $site,
                                           u.longitude = $longitude,
                                           u.latitude = $latitude,
                                           u.altitude = $altitude,
                                           u.time_zone = $time_zone,
                                           u.daylight_saving = $daylight_saving,
                                           u.water_vapor = $water_vapor,
                                           u.light_pollution = $light_pollution",
                                      new
                                      {
                                          UID = equipment.UID,
                                          EID = equipment.EID,
                                          UhaveE_ID = equipment.UhaveE_ID,

                                          aperture = equipment.Aperture,
                                          FoV = equipment.FoV,
                                          pixel_scale = equipment.PixelScale,
                                          tracking_accuracy = equipment.TrackingAccuracy,

                                          limiting_magnitude = equipment.LimitingMagnitude,
                                          elevation_limit = equipment.ElevationLimit,
                                          mount_type = equipment.MountType,

                                          camera_t1 = equipment.CameraType_1,
                                          camera_t2 = equipment.CameraType_2,

                                          Johnson_B = equipment.Johnson_B,
                                          Johnson_V = equipment.Johnson_V,
                                          Johnson_R = equipment.Johnson_R,

                                          SDSS_u = equipment.SDSS_u,
                                          SDSS_g = equipment.SDSS_g,
                                          SDSS_r = equipment.SDSS_r,
                                          SDSS_i = equipment.SDSS_i,
                                          SDSS_z = equipment.SDSS_z,

                                          site = equipment.Site,
                                          longitude = equipment.Longitude,
                                          latitude = equipment.Latitude,
                                          altitude = equipment.Altitude,

                                          time_zone = equipment.TimeZone,
                                          daylight_saving = equipment.DaylightSaving,
                                          water_vapor = equipment.WaterVapor,
                                          light_pollution = equipment.LightPollution
                                      });
            
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
            if (astroResults.Length == 1 || astroResults[0] == "nan")
            {

                var create = Session.Run(@"MATCH (e:Equipment {EID:$EID}), (t:Target {TID:$TID})
                                               MERGE (e)-[:NotInterested {PID:$PID} ]->(t)", new { EID, TID, PID });
                
       
            }
            else
            {
                string start = astroResults[0];
                string end = astroResults[1].Trim();
                var delete = Session.Run(@"MATCH (e:Equipment {EID:$EID})-[r:Interested]-(t:Target {TID:$TID})
                                               DELETE r", new { EID, TID });
                var create = Session.Run(@"MATCH (e:Equipment {EID:$EID}), (t:Target {TID:$TID})
                                               MERGE (e)-[:Interested {PID:$PID,start:$start,end:$end} ]->(t)", new { EID, TID, PID, start, end });
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

        public List<string[]> GetEquipmentSchedule(string username)
        {
            var result = Session.Run(@"MATCH (u:User{username:$username})-[:User_To_Equipment]-(e:Equipment)
                                      RETURN e.UhaveE_ID as UhaveE_ID",
                           new { username });
            var UhaveE_IDList = new List<string>();
            var userEquipmentSchedule = new List<string>();
            foreach (var record in result)
            {;
                var eid = record["UhaveE_ID"].ToString();
                UhaveE_IDList.Add(eid);
                userEquipmentSchedule.Add(eid);
            }
            List<string[]> output = new List<string[]>();
            for(int i = 0;  i< UhaveE_IDList.Count; i++)
            {
                string UhaveE_ID = UhaveE_IDList[i];
                var schedule = Session.Run(@"MATCH (e:Equipment{UhaveE_ID:$UhaveE_ID})-[r:Interested]-(t:Target) 
                                            WHERE r.start <> 'nan'
                                            WITH e, r, t
                                            WHERE(datetime() < datetime(r.start) AND datetime(r.start) < datetime() + duration('PT6H'))
                                            OR(datetime() < datetime(r.end) AND datetime(r.end) < datetime() + duration('PT6H'))
                                            return e.EID, r.PID,t.TID", 
                                            new { UhaveE_ID });
                foreach (var record in schedule)
                {
                    UhaveE_IDList[i] += ",Unavailable," + record["r.PID"] + "," + record["t.TID"];
                }
                if(UhaveE_IDList[i] == userEquipmentSchedule[i])
                {
                    string avail = UhaveE_IDList[i] + "," + "Available" + ",-,-";
                    output.Add(avail.Split(','));
                }
                else
                {
                    string unavail = UhaveE_IDList[i];
                    output.Add(unavail.Split(','));
                }
            }

            return output;
        }

        public List<Equipment> GetProjectEquipmentSchedule(string pid, string username)
        {
            var result = Session.Run(@"MATCH (t:Target)-[:Interested{PID:$pid}]-(e:Equipment)
                                       RETURN DISTINCT(e),
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
                                       COALESCE(e.SDSS_z,'')+ ',' + 

                                       COALESCE(e.declination_limit,'')    
                                       as msg",
                                      new { pid });

            var equipmentsList = new List<Equipment>();

            foreach (var record in result)
            {
                var msg = record["msg"].ToString();
                var newEquipment = new Equipment(msg);
                equipmentsList.Add(newEquipment);
            }


            return equipmentsList;
        }
    }
}
