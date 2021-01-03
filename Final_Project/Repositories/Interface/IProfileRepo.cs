using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Final_Project.Models;

namespace Final_Project.Repositories.Interface
{
    public interface IProfileRepo
    {
        IProfileRepo GetProfile(int id);
        bool InsertProfile(Profile profile);
        bool UpdateProfile(Profile profile);
    }
}
