namespace Final_Project.Models
{
    public class Target
    {
        public string TID { get; set; }
        public string Name { get; set; }
        public string RA { get; set; }
        public string Dec { get; set; }
        public string Score { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public Target(string msg)
        {
            var msgList = msg.Split(',');
            TID = msgList[0];
            Name = msgList[1];
            RA = msgList[2];
            Dec = msgList[3];
            Score = "0";
            Start = "";
            End = "";
        }
        public Target(string msg, string score)
        {
            var msgList = msg.Split(',');
            TID = msgList[0];
            Name = msgList[1];
            RA = msgList[2];
            Dec = msgList[3];
            Score = score;
            Start = "";
            End = "";
        }
        public Target(string msg, string score, string start, string end)
        {
            var msgList = msg.Split(',');
            TID = msgList[0];
            Name = msgList[1];
            RA = msgList[2];
            Dec = msgList[3];
            Score = score;
            Start = start;
            End = end;
        }
    }
}
