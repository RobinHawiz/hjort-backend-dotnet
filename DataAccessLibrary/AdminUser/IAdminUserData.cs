using DataAccessLibrary.Models;

namespace DataAccessLibrary.AdminUser;

public interface IAdminUserData
{
    AdminUserModel? GetOneUser(string username, string password);
}