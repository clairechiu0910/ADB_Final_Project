namespace Final_Project.Models
{
    public class Project
    {
        public Project()
        {

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
            MountType = msgList[10];
            CameraType_1 = msgList[11];
            CameraType_2 = msgList[12];

            Johnson_B = msgList[13];
            Johnson_V = msgList[14];
            Johnson_R = msgList[15];

            SDSS_u = msgList[16];
            SDSS_g = msgList[17];
            SDSS_r = msgList[18];
            SDSS_i = msgList[19];
            SDSS_z = msgList[20];
        }

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
