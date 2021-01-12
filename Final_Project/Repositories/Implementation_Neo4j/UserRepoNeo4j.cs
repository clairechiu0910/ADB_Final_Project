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

        public UserRepoNeo4j(IConfiguration configuration)
        {
            var url = configuration.GetValue<string>("Database:Url");
            var user = configuration.GetValue<string>("Database:User");
            var password = configuration.GetValue<string>("Database:Password");
            _driver = GraphDatabase.Driver(url, AuthTokens.Basic(user, password));
        }

        public User GetProfile(string account)
        {
            throw new NotImplementedException();
        }

        public void InsertProfile(User user)
        {
            using (var session = _driver.Session())
            {
                var log = session.WriteTransaction(tx =>
                {
                    var result = tx.Run("CREATE (a:User) " +
                                        "SET a.Account = $account, a.Password = $password, a.Name = $name, a.Address = $address, a.Phone = $phone, a.Profile = $email " +
                                        "RETURN a.Account + ', from node ' + id(a)",
                        new {
                            account = user.Account,
                            password = user.Password, 
                            name = user.Name,
                            address = user.Address,
                            phone = user.Phone,
                            email = user.Email });
                    return result;
                });
            }
        }

        public void UpdateProfile(User profile)
        {
            throw new NotImplementedException();
        }
    }
}
