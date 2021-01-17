using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Repositories.Interface
{
    public interface IUHavePRepo
    {
        void JoinProject(string username, string PID);
    }
}
