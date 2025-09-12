namespace HjortApi.Services
{
    public interface IAdminUserService
    {
        string Authenticate(string username, string password);
    }
}