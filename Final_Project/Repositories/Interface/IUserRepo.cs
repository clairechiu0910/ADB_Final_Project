using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Final_Project.Models;

namespace Final_Project.Repositories.Interface
{
    public interface IUserRepo
    {
        User GetProfile(string account);
        void InsertProfile(User user);
        void UpdateProfile(User user);
    }
}
