using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        public Profile GetProfile(int id)
        {
            throw new NotImplementedException();
        }

        public void InsertProfile(Profile profile)
        {
            var profileList = GetJsonFileData();
            profileList.Add(profile);
            var jsonString = JsonSerializer.Serialize(profileList);
            File.WriteAllText(ProfileFilePath, jsonString);
            return;
        }

        public void UpdateProfile(Profile profile)
        {
            throw new NotImplementedException();
        }

        private List<Profile> GetJsonFileData()
        {
            var fileJsonString = File.ReadAllText(ProfileFilePath);
            var profileList = JsonSerializer.Deserialize<List<Profile>>(fileJsonString);
            return profileList;
        }
    }
}
