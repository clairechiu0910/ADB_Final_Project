namespace Final_Project.Models
{
    public class Equipment
    {
        public Equipment()
        {

        }

        public Equipment(string msg)
        {
            var msgList = msg.Split(',');

            EID = msgList[0];
            Aperture = msgList[1];
            FoV = msgList[2];
            PixelScale = msgList[3];
            TrackingAccuracy = msgList[4];

            LimitingMagnitude = msgList[5];
            ElevationLimit = msgList[6];
            MountType = msgList[7];

            CameraType_1 = msgList[8];
            CameraType_2 = msgList[9];

            Johnson_B = msgList[10];
            Johnson_V = msgList[11];
            Johnson_R = msgList[12];

            SDSS_u = msgList[13];
            SDSS_g = msgList[14];
            SDSS_r = msgList[15];
            SDSS_i = msgList[16];
            SDSS_z = msgList[17];
        }

        public string EID { get; set; }
        public string Aperture { get; set; }
        public string FoV { get; set; }
        public string PixelScale { get; set; }
        public string TrackingAccuracy { get; set; }
        public string LimitingMagnitude { get; set; }
        public string ElevationLimit { get; set; }
        public string MountType { get; set; }
        public string CameraType_1 { get; set; }
        public string CameraType_2 { get; set; }
        public string Johnson_B { get; set; }
        public string Johnson_V { get; set; }
        public string Johnson_R { get; set; }
        public string SDSS_u { get; set; }
        public string SDSS_g { get; set; }
        public string SDSS_r { get; set; }
        public string SDSS_i { get; set; }
        public string SDSS_z { get; set; }
    }
}
