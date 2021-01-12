using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Final_Project.Repositories.Interface;
using Final_Project.Models;

namespace Final_Project.Repositories.Implementation
{
    public class UserRepo : IUserRepo
    {
        private readonly string ProfileFilePath;

        public UserRepo(IHostingEnvironment environment)
        {
            ProfileFilePath = Path.Combine(environment.WebRootPath, "UserJson.json");
        }

        public User GetProfile(string account)
        {
            var profileList = GetJsonFileData();
            var profile = profileList.FirstOrDefault(s => s.Account == account);
            return profile;
        }

        public void InsertProfile(User profile)
        {
            var profileList = GetJsonFileData();
            profileList.Add(profile);
            WriteJsonFile(profileList);
            return;
        }

        public void UpdateProfile(User profile)
        {
            var profileList = GetJsonFileData();

            var idx = profileList.FindIndex(s => s.Account == profile.Account);
            profileList.RemoveAt(idx);
            profileList.Add(profile);

            WriteJsonFile(profileList);
        }

        private void WriteJsonFile(List<User> profileList)
        {
            var jsonString = JsonSerializer.Serialize(profileList);
            File.WriteAllText(ProfileFilePath, jsonString);
        }

        private List<User> GetJsonFileData()
        {
            try
            {
                var fileJsonString = File.ReadAllText(ProfileFilePath);
                var profileList = JsonSerializer.Deserialize<List<User>>(fileJsonString);
                return profileList;
            }
            catch
            {
                return new List<User>(){};
            }
        }
    }
}
