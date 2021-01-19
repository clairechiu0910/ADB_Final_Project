using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Final_Project.Models;

namespace Final_Project.Repositories.Interface
{
    public interface IEquipmentRepo
    {
        List<Equipment> GetUserEquipment(string username);
        Equipment GetEquipmentByEID(string eid);

        void AddEquipment(Equipment equipment);
        void UpdateEquipment(Equipment equipment);

        void CreateInterest(string username, string PID);
        void CreateSingleInterest(string username, string PID, string TID);
        void ComputeDeclination(string username);
        List<string[]> GetEquipmentSchedule(string username);
        List<Equipment> GetProjectEquipmentSchedule(string pid, string username);
    }
}
