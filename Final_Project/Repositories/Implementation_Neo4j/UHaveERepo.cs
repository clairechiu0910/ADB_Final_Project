using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Neo4j.Driver;
using Final_Project.Models;
using Final_Project.Repositories.Interface;

namespace Final_Project.Repositories.Implementation_Neo4j
{
    public class UHaveERepo : IUHaveERepo
    {
        private readonly IDriver _driver;
        private readonly ISession Session;
        readonly string pythonPath;
        readonly string astroPath;
        readonly string decPath;
        private readonly AstroCalculations astro;
        public UHaveERepo(IConfiguration configuration)
        {
            var url = configuration.GetValue<string>("Database:Url");
            var user = configuration.GetValue<string>("Database:User");
            var password = configuration.GetValue<string>("Database:Password");
            _driver = GraphDatabase.Driver(url, AuthTokens.Basic(user, password));
            Session = _driver.Session();

            //python
            pythonPath = configuration.GetValue<string>("Python:PythonPath");
            astroPath = configuration.GetValue<string>("Python:AstroPath");
            decPath = configuration.GetValue<string>("Python:DecPath");
            astro = new AstroCalculations(pythonPath, astroPath, decPath);
        }

        public List<UHaveE> GetUHaveE(string username)
        {
            var result = Session.Run(@"MATCH p = ({username: $username})-[:User_To_Equipment]->(r:Equipment)
                                       RETURN r.UhaveE_ID +','+ r.UID +','+ r.EID +','+ r.altitude+','+r.daylight_saving+','+r.latitude+','+r.longitude+','+r.site+','+r.time_zone+','+r.water_vapor +','+ r.light_pollution + ',' + COALESCE(r.declination_limit,'') as msg",
                                       new { username });

            var uHaveEList = new List<UHaveE>();

            foreach (var record in result)
            {
                var msg = record["msg"].ToString();
                uHaveEList.Add(new UHaveE(msg));
            }

            uHaveEList.Sort(delegate (UHaveE x, UHaveE y)
            {
                return y.EID.CompareTo(x.EID);
            });

            return uHaveEList;
        }
        public void ComputeDeclination(string username)
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

            for (int i = 0; i < equipmentsList.Count; i++)
            {
                string decLimit, UhaveE_ID;
                UhaveE_ID = equipmentsList[i].UhaveE_ID;
                decLimit = astro.Declinationlimit(equipmentsList[i].Longitude, equipmentsList[i].Latitude, equipmentsList[i].Altitude, elevLimit[i]);
                var insertDec = Session.Run(@"MATCH ()-[r:User_To_Equipment{UhaveE_ID:$UhaveE_ID}]-()
                                              SET r.declination_limit = $decLimit", new { UhaveE_ID, decLimit });
            }
        }
    }
}
