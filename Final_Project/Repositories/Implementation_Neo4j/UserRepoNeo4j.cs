using Final_Project.Models;
using Final_Project.Repositories.Interface;
using Microsoft.Extensions.Configuration;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
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
            var newID = CountNodes();
            user.UID = newID.ToString();

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
                                       RETURN u.username + ' Update'",
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

        public int CountNodes()
        {
            var result = Session.Run(@"MATCH (n:User) Return count(n)");
            var msg = result.Single()[0].As<string>();
            return Int32.Parse(msg);
        }

        public List<User> GetRelatedUser(string username)
        {
            var result = Session.Run(@"MATCH(a:User {username: $username})-[r:User_To_Project]-(b:Project)-[r2:User_To_Project]-(c:User)
                                       WITH c, count(r2) as rel_count
                                       RETURN c.UID +','+ c.username +','+ c.password +','+ c.name +','+ c.email +','+ c.affiliation +','+ c.title +','+ c.country +','+ rel_count as msg
                                       ORDER BY rel_count DESC
                                       LIMIT 10",
                                      new { username });

            var relatedUsers = new List<User>();

            foreach (var record in result)
            {
                var msg = record["msg"].ToString();
                relatedUsers.Add(new User(msg));
            }

            return relatedUsers;
        }
    }
}
