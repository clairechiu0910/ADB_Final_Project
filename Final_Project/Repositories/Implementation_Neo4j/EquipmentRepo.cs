using Final_Project.Models;
using Final_Project.Repositories.Interface;
using Microsoft.Extensions.Configuration;
using Neo4j.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Final_Project.Repositories.Implementation_Neo4j
{
    public class EquipmentRepo : IEquipmentRepo
    {
        private readonly IDriver _driver;
        private readonly ISession Session;

        public EquipmentRepo(IConfiguration configuration)
        {
            var url = configuration.GetValue<string>("Database:Url");
            var user = configuration.GetValue<string>("Database:User");
            var password = configuration.GetValue<string>("Database:Password");
            _driver = GraphDatabase.Driver(url, AuthTokens.Basic(user, password));
            Session = _driver.Session();
        }

        public List<Equipment> GetUserEquipment(string username)
        {
            var result = Session.Run(@"MATCH p = (u:User {username: $username})-[r:User_To_Equipment]->(e:Equipment)
                                       RETURN e.EID+', '+e.aperture+', '+e.FoV+', '+e.pixel_scale+', '+e.tracking_accuracy+', '+e.limiting_magnitude+', '+e.elevation_limit+', '+
                                              e.mount_type+', '+e.camera_t1+', '+e.camera_t2+', '+e.Johnson_B+', '+e.Johnson_V+', '+e.Johnson_R+', '+e.SDSS_u+', '+e.SDSS_g+', '+e.SDSS_r+', '+e.SDSS_i+', '+e.SDSS_z as msg",
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
    }
}
