namespace Final_Project.Models
{
    public class Project
    {
        public Project()
        {
            Title = "New Project";
            Project_type = "regular";
            Description = "Description";

            ApertureUpperLimit = "1000";
            ApertureLowerLimit = "0";

            FoVUpperLimit = "1000";
            FoVLowerLimit = "0";

            PixelScaleUpperLimit = "1000";
            PixelScaleLowerLimit = "0";

            MountType = "german";

            CameraType_1 = "mono";
            CameraType_2 = "cooled";

            Johnson_B = "n";
            Johnson_V = "n";
            Johnson_R = "n";

            SDSS_u = "n";
            SDSS_g = "n";
            SDSS_r = "n";
            SDSS_i = "n";
            SDSS_z = "n";
        }

        public Project(string msg)
        {
            var msgList = msg.Split(',');

            PID = msgList[0];
            Title = msgList[1];
            Project_type = msgList[2];
            PI = msgList[3];
            Description = msgList[4];

            ApertureUpperLimit = msgList[5];
            ApertureLowerLimit = msgList[6];

            FoVUpperLimit = msgList[7];
            FoVLowerLimit = msgList[8];

            PixelScaleUpperLimit = msgList[9];
            PixelScaleLowerLimit = msgList[10];

            MountType = msgList[11];
            CameraType_1 = msgList[12];
            CameraType_2 = msgList[13];

            Johnson_B = msgList[14];
            Johnson_V = msgList[15];
            Johnson_R = msgList[16];

            SDSS_u = msgList[17];
            SDSS_g = msgList[18];
            SDSS_r = msgList[19];
            SDSS_i = msgList[20];
            SDSS_z = msgList[21];
            Score = "0";
        }
        public Project(string msg, string score)
        {
            var msgList = msg.Split(',');

            PID = msgList[0];
            Title = msgList[1];
            Project_type = msgList[2];
            PI = msgList[3];
            Description = msgList[4];

            ApertureUpperLimit = msgList[5];
            ApertureLowerLimit = msgList[6];

            FoVUpperLimit = msgList[7];
            FoVLowerLimit = msgList[8];

            PixelScaleUpperLimit = msgList[9];
            PixelScaleLowerLimit = msgList[10];

            MountType = msgList[11];
            CameraType_1 = msgList[12];
            CameraType_2 = msgList[13];

            Johnson_B = msgList[14];
            Johnson_V = msgList[15];
            Johnson_R = msgList[16];

            SDSS_u = msgList[17];
            SDSS_g = msgList[18];
            SDSS_r = msgList[19];
            SDSS_i = msgList[20];
            SDSS_z = msgList[21];
            Score = score;
        }
        public string Score { get; set; }
        public string PID { get; set; }
        public string Title { get; set; }
        public string Project_type { get; set; }
        public string PI { get; set; }
        public string Description { get; set; }
        public string ApertureUpperLimit { get; set; }
        public string ApertureLowerLimit { get; set; }
        public string FoVUpperLimit { get; set; }
        public string FoVLowerLimit { get; set; }
        public string PixelScaleUpperLimit { get; set; }
        public string PixelScaleLowerLimit { get; set; }
        public string MountType { get; set; }
        public string CameraType_1 { get; set; }
        public string CameraType_2 {get; set; }
        public string Johnson_B { get; set; }
        public string Johnson_V { get; set; }
        public string Johnson_R { get; set; }
        public string SDSS_u { get; set; }
        public string SDSS_g { get; set; }
        public string SDSS_r { get; set; }
        public string SDSS_i { get; set; }
        public string SDSS_z { get; set; }
        public string Progress { get; set; }
    }
}
