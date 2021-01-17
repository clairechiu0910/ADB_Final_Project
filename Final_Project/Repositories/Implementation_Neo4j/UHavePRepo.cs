using Final_Project.Repositories.Interface;
using Microsoft.Extensions.Configuration;
using Neo4j.Driver;


namespace Final_Project.Repositories.Implementation_Neo4j
{
    public class UHavePRepo : IUHavePRepo
    {
        private readonly IDriver _driver;
        private readonly ISession Session;

        public UHavePRepo(IConfiguration configuration)
        {
            var url = configuration.GetValue<string>("Database:Url");
            var user = configuration.GetValue<string>("Database:User");
            var password = configuration.GetValue<string>("Database:Password");
            _driver = GraphDatabase.Driver(url, AuthTokens.Basic(user, password));
            Session = _driver.Session();
        }

        public void JoinProject(string username, string PID)
        {
            var result = Session.Run(@"MATCh(a: User), (b: Project)
                                       WHERE a.username = $username AND b.PID = $PID
                                       CREATE(a) -[r: User_To_Project]-> (b)",
                                       new { username, PID });
        }
    }
}
