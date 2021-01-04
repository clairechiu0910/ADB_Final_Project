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
    public class ProfileRepo : IProfileRepo
    {
        private readonly string ProfileFilePath;

        public ProfileRepo(IHostingEnvironment environment)
        {
            ProfileFilePath = Path.Combine(environment.WebRootPath, "ProfileJson.json");
        }

        public Profile GetProfile(string account)
        {
            var profileList = GetJsonFileData();
            var profile = profileList.FirstOrDefault(s => s.Account == account);
            return profile;
        }

        public void InsertProfile(Profile profile)
        {
            var profileList = GetJsonFileData();
            profileList.Add(profile);
            WriteJsonFile(profileList);
            return;
        }

        public void UpdateProfile(Profile profile)
        {
            var profileList = GetJsonFileData();

            var idx = profileList.FindIndex(s => s.Account == profile.Account);
            profileList.RemoveAt(idx);
            profileList.Add(profile);

            WriteJsonFile(profileList);
        }

        private void WriteJsonFile(List<Profile> profileList)
        {
            var jsonString = JsonSerializer.Serialize(profileList);
            File.WriteAllText(ProfileFilePath, jsonString);
        }

        private List<Profile> GetJsonFileData()
        {
            try
            {
                var fileJsonString = File.ReadAllText(ProfileFilePath);
                var profileList = JsonSerializer.Deserialize<List<Profile>>(fileJsonString);
                return profileList;
            }
            catch
            {
                return new List<Profile>(){};
            }
        }
    }
}
