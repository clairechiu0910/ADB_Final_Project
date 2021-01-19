using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Final_Project.Models;

namespace Final_Project.Repositories.Interface
{
    public interface IUserRepo
    {
        User GetUser(int uid);
        User GetUser(string username);
        void InsertUser(User user);
        void UpdateUser(User user);
        int CountNodes();
        bool IfUserNameExist(string username);

        List<User> GetRelatedUser(string username);
    }
}
