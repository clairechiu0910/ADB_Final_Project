namespace Final_Project.Services.Interface
{
    public interface IAuthenticationService
    {
        bool IsAuthentication(string account, string password);
    }
}
