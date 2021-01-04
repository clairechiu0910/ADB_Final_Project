using Final_Project.Services.Interface;
using Final_Project.Repositories.Interface;

namespace Final_Project.Services.Implementation
{
    public class RepoAuthenticationService : IAuthenticationService
    {
        private readonly IProfileRepo _profileRepo;
        public RepoAuthenticationService(IProfileRepo profileRepo)
        {
            _profileRepo = profileRepo;
        }

        public bool IsAuthentication(string account, string password)
        {
            var profile = _profileRepo.GetProfile(account);
            if(profile.Password == password)
            {
                return true;
            }
            return false;
        }
    }
}
