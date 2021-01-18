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

        public UHaveERepo(IConfiguration configuration)
        {
            var url = configuration.GetValue<string>("Database:Url");
            var user = configuration.GetValue<string>("Database:User");
            var password = configuration.GetValue<string>("Database:Password");
            _driver = GraphDatabase.Driver(url, AuthTokens.Basic(user, password));
            Session = _driver.Session();
        }

        public List<UHaveE> GetUHaveE(string username)
        {
            var result = Session.Run(@"MATCH p = ({username: $username})-[r:User_To_Equipment]->()
                                       RETURN r.UhaveE_ID +', '+ r.UID +', '+ r.EID +', '+ r.altitude+', '+r.daylight_saving+', '+r.latitude+', '+r.longitude+', '+r.site+', '+r.time_zone+', '+r.water_vapor +', '+ r.light_pollution as msg",
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
    }
}
