using System;

namespace Final_Project.Models
{
    public class User
    {
        private string msg;

        public User()
        {

        }

        public User(string msg)
        {
            var msgList = msg.Split(',');
            UID = Int32.Parse(msgList[0]);
            Username = msgList[1];
            Password = msgList[2];
            Name = msgList[3];
            Email = msgList[4];
            Affiliation = msgList[5];
            Title = msgList[6];
            Country = msgList[7];
        }

        public int UID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Affiliation { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
    }
}
