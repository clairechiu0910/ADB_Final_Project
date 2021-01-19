using System.Collections.Generic;

namespace Final_Project.Models
{
    public class SelectTargetModel
    {
        public int RA_Lower { get; set; }
        public int RA_Upper { get; set; }
        public int DEC_Lower { get; set; }
        public int DEC_Upper { get; set; }

        public string PID { get; set; }

        public List<Target> TargetsInRange;


        public SelectTargetModel()
        {

        }

        public SelectTargetModel(string pid)
        {
            PID = pid;
            RA_Lower = 314;
            RA_Upper = 326;
            DEC_Lower = 80;
            DEC_Upper = 81;
        }
    }
}
