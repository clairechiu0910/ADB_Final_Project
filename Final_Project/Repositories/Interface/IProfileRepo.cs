using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Final_Project.Models;

namespace Final_Project.Repositories.Interface
{
    public interface IProfileRepo
    {
        Profile GetProfile(string account);
        void InsertProfile(Profile profile);
        void UpdateProfile(Profile profile);
    }
}
