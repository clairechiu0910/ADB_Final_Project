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

            UID = msgList[0];
            EID = msgList[1];
            UhaveE_ID = msgList[2];

            Site = msgList[3];
            Longitude = msgList[4];
            Latitude = msgList[5];
            Altitude = msgList[6];

            TimeZone = msgList[7];
            DaylightSaving = msgList[8];
            WaterVapor = msgList[9];
            LightPollution = msgList[10];

            Aperture = msgList[11];
            FoV = msgList[12];
            PixelScale = msgList[13];
            TrackingAccuracy = msgList[14];

            LimitingMagnitude = msgList[15];
            ElevationLimit = msgList[16];
            MountType = msgList[17];

            CameraType_1 = msgList[18];
            CameraType_2 = msgList[19];

            Johnson_B = msgList[20];
            Johnson_V = msgList[21];
            Johnson_R = msgList[22];

            SDSS_u = msgList[23];
            SDSS_g = msgList[24];
            SDSS_r = msgList[25];
            SDSS_i = msgList[26];
            SDSS_z = msgList[27];


            if (msgList.Length == 29)
            {
                DeclinationLimit = msgList[28];
            }
        }

        public string UID { get; set; }
        public string EID { get; set; }
        public string UhaveE_ID { get; set; }

        public string Site { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Altitude { get; set; }

        public string TimeZone { get; set; }
        public string DaylightSaving { get; set; }
        public string WaterVapor { get; set; }
        public string LightPollution { get; set; }

        public string DeclinationLimit { get; set; }

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
