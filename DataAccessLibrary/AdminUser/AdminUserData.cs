using DataAccessLibrary.Models;

namespace DataAccessLibrary.AdminUser
{
    public class AdminUserData : IAdminUserData
    {
        private readonly ISqliteDataAccess _db;
        private readonly string _connectionStringName = "Default";

        public AdminUserData(ISqliteDataAccess db)
        {
            _db = db;
        }

        public AdminUserModel? GetOneUser(string username, string password)
        {
            string sql = "select id as Id, username as Username, passwordHash as PasswordHash, email as Email, first_name as FirstName, last_name as LastName from admin_user where username = @username";
            return _db.LoadData<AdminUserModel, dynamic>(sql, new {username}, _connectionStringName).FirstOrDefault() ?? null;
        }
    }
}
