namespace Final_Project.Models
{
    public class UHaveE
    {
        public UHaveE()
        {

        }

        public UHaveE(string msg)
        {
            var msgList = msg.Split(',');

            UhaveE_ID = msgList[0];
            UID = msgList[1];
            EID = msgList[2];
            Altitude = msgList[3];
            DaylightSaving = msgList[4];
            Latitude = msgList[5];
            Longitude = msgList[6];
            Site = msgList[7];
            TimeZone = msgList[8];
            WaterVapor = msgList[9];
            LightPollution = msgList[10];
        }

        public string UhaveE_ID { get; set; }
        public string UID { get; set; }
        public string EID { get; set; }
        public string Site { get; set; }
        public string Longitude { get; set; }

        public string Latitude { get; set; }
        public string Altitude { get; set; }
        public string TimeZone { get; set; }
        public string DaylightSaving { get; set; }
        public string WaterVapor { get; set; }

        public string LightPollution { get; set; }
    }
}
