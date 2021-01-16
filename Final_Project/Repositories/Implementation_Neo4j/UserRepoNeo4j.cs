using Final_Project.Models;
using Final_Project.Repositories.Interface;
using Microsoft.Extensions.Configuration;
using Neo4j.Driver;
using System;
using System.Linq;

namespace Final_Project.Repositories.Implementation_Neo4j
{
    public class UserRepoNeo4j : IUserRepo
    {
        private readonly IDriver _driver;
        private readonly ISession Session;

        public UserRepoNeo4j(IConfiguration configuration)
        {
            var url = configuration.GetValue<string>("Database:Url");
            var user = configuration.GetValue<string>("Database:User");
            var password = configuration.GetValue<string>("Database:Password");
            _driver = GraphDatabase.Driver(url, AuthTokens.Basic(user, password));
            Session = _driver.Session();
        }

        public User GetUser(string username)
        {
            var result = Session.Run(@"MATCH (u:User {username: $username}) 
                                       RETURN u.UID +','+ u.username +','+ u.password +','+ u.name +','+ u.email +','+ u.affiliation +','+ u.title +','+ u.country",
                                       new { username });
            var msg = result.Single()[0].As<string>();
            var user = new User(msg);
            return user;
        }

        public User GetUser(int uid)
        {
            var result = Session.Run(@"MATCH (u:User {UID: $UID}) 
                                       RETURN u.UID +','+ u.username +','+ u.password +','+ u.name +','+ u.email +','+ u.affiliation +','+ u.title +','+ u.country",
                                       new { UID = uid });
            var msg = result.Single()[0].As<string>();
            var user = new User(msg);
            return user;
        }

        public void InsertUser(User user)
        {
            var result = Session.Run(@"CREATE (u:User) 
                                       SET u.UID = $UID, u.username = $username, u.password = $password, u.name = $name, u.email = $email, u.affiliation = $affiliation, u.title = $title, u.country = $country
                                       RETURN u.username + ' Create'",
                                       new {
                                           UID = user.UID,
                                           username =user.Username,
                                           password = user.Password,
                                           name = user.Name,
                                           email = user.Email,
                                           country = user.Country, 
                                           title = user.Title,
                                           affiliation = user.Affiliation
                                       });
        }

        public void UpdateUser(User user)
        {
            var result = Session.Run(@"MATCH (u:User {username: $username}) 
                                       SET u.UID = $UID, u.username = $username, u.password = $password, u.name = $name, u.email = $email, u.affiliation = $affiliation, u.title = $title, u.country = $country
                                       RETURN u.username + ' Create'",
                                       new
                                       {
                                           UID = user.UID,
                                           username = user.Username,
                                           password = user.Password,
                                           name = user.Name,
                                           email = user.Email,
                                           country = user.Country,
                                           title = user.Title,
                                           affiliation = user.Affiliation
                                       });
        }
    }
}
