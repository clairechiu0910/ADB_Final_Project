namespace Final_Project.Models
{
    public class Target
    {
        public string TID { get; set; }
        public string Name { get; set; }
        public string RA { get; set; }
        public string Dec { get; set; }

        public Target(string msg)
        {
            var msgList = msg.Split(',');
            TID = msgList[0];
            Name = msgList[1];
            RA = msgList[2];
            Dec = msgList[3];
        }
    }
}
