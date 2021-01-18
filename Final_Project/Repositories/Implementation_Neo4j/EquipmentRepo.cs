using Final_Project.Models;
using Final_Project.Repositories.Interface;
using Microsoft.Extensions.Configuration;
using Neo4j.Driver;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;

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
            pythonPath = configuration.GetValue<string>("Python:PythonPath");
            astroPath = configuration.GetValue<string>("Python:AstroPath");
            decPath = configuration.GetValue<string>("Python:DecPath");
            _driver = GraphDatabase.Driver(url, AuthTokens.Basic(user, password));
            astro = new AstroCalculations(pythonPath, astroPath, decPath);
            Session = _driver.Session();
        }

        public List<Equipment> GetUserEquipment(string username)
        {
            var result = Session.Run(@"MATCH p = (u:User {username: $username})-[r:User_To_Equipment]->(e:Equipment)
                                       RETURN COALESCE(e.EID,'') + ',' + COALESCE(e.aperture,'') + ',' + COALESCE(e.FoV,'') + ',' + COALESCE(e.pixel_scale,'') + ',' + COALESCE(e.tracking_accuracy,'') + ',' + 
                                       COALESCE(e.limiting_magnitude,'') + ',' + COALESCE(e.elevation_limit,'') + ',' + COALESCE(e.mount_type,'') + ',' + COALESCE(e.camera_t1,'') + ',' + COALESCE(e.camera_t2,'') + ',' + 
                                       COALESCE(e.Johnson_B,'') + ',' + COALESCE(e.Johnson_V,'') + ',' + COALESCE(e.Johnson_R,'') + ',' + COALESCE(e.SDSS_u,'') + ',' + COALESCE(e.SDSS_g,'') + ',' + 
                                       COALESCE(e.SDSS_r,'') + ',' + COALESCE(e.SDSS_i,'') + ',' + COALESCE(e.SDSS_z,'') as msg",
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
            var result = Session.Run(@"MATCH p = (u:User {username: $username})-[r:User_To_Equipment]->(e:Equipment)
                                       RETURN COALESCE(r.UhaveE_ID,'') + ',' + COALESCE(r.UID,'') + ',' + COALESCE(r.EID,'') + ',' + COALESCE(r.altitude,'') + ',' + COALESCE(r.daylight_saving,'') + ',' + 
                                       COALESCE(r.latitude,'') +','+ COALESCE(r.longitude,'') +','+ COALESCE(r.site,'') +','+ COALESCE(r.time_zone,'') +','+ COALESCE(r.water_vapor,'') + ',' + COALESCE(r.light_pollution,'') as msg, e.elevation_limit",
                                      new { username });

            var equipmentsList = new List<UHaveE>();
            List<string> elevLimit = new List<string>();
            foreach (var record in result)
            {
                var msg = record["msg"].ToString();
                var newEquipment = new UHaveE(msg);
                equipmentsList.Add(newEquipment);
                elevLimit.Add(record["e.elevation_limit"].ToString());
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
                    string[] astroResults = AstroplanCalculations(equipmentsList[i].Longitude, equipmentsList[i].Latitude, equipmentsList[i].Altitude, elevLimit[i], targ.RA,targ.Dec);
                    string EID = equipmentsList[i].EID;
                    string TID = targ.TID;
                    //if not 2 meaning incompatible
                    if (astroResults.Length == 2)
                    {
                       
                        string start = astroResults[0];
                        string end = astroResults[1];
                        var create = Session.Run(@"MATCH (e:Equipment {EID:$EID}), (t:Target {TID:$TID})
                                               CREATE (e)-[:Interested {PID:$PID,start:$start,end:$end} ]->(t)", new { EID, TID, PID, start, end });
                    }
                    else
                    {
                        var create = Session.Run(@"MATCH (e:Equipment {EID:$EID}), (t:Target {TID:$TID})
                                               CREATE (e)-[:NotInterested {PID:$PID} ]->(t)", new { EID, TID, PID });
                    }

                }
            }

        }

        private string RunCmd(string cmd, string args)
        {
            //ProcessStartInfo start = new ProcessStartInfo(@"C:\Windows\System32\cmd.exe");
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Users\Me\AppData\Local\Programs\Python\Python37\python.exe";
            start.Arguments = string.Format("\"{0}\" {1}", cmd, args);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            string result = "";
            string error = "";
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    result += reader.ReadToEnd() + "\n";
                }
                using (StreamReader reader = process.StandardError)
                {
                    error += reader.ReadToEnd() + "\n";  
                }
            }
            return result;
        }
        public string[] AstroplanCalculations(string userLong, string userLat, string userAlt, string userElev, string targetLong, string targetLat)
        {
            // requires user long,lat,alt,elev and target long targetlat.
            string args = string.Format("{0} {1} {2} {3} {4} {5}", userLong, userLat, userAlt, userElev, targetLong, targetLat);
            string result = RunCmd(@"D:\Downloads\Astroplan_calculations.py", args);
            string[] results = result.Split(",");
            // Console.WriteLine(results[0]); // start_time
            // Console.WriteLine(results[1]); // end_time
            return results;
        }
        public double Declinationlimit(string argLong, string argLat, string argAlt, string argElev)
        {
            // requires equipment long lat alt and elev
            string args = string.Format("{0} {1} {2} {3}", argLong, argLat, argAlt, argElev);
            string result = RunCmd("D:\\Downloads\\Declination_limit_of_location.py", args);
            double decLimit = double.Parse(result);
            return decLimit;
        }
    }
}
