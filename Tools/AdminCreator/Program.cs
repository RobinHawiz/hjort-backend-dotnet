using DataAccessLibrary;
using Microsoft.Extensions.Configuration;
using BC = BCrypt.Net.BCrypt;

internal class Program
{
    private static void Main(string[] args)
    {
        IConfiguration config = CreateConfig();
        ISqliteDataAccess db = new SqliteDataAccess(config);
        string connectionStringName = "Default";
        DirectoryInfo dir = TryGetSolutionDirectoryInfo();

        AdminUser user = new
            (
            GetUserInput("Set username: "),
            BC.HashPassword(GetUserInput("Set password: "), workFactor: 10),
            GetUserInput("Set email: "),
            GetUserInput("Set first name: "),
            GetUserInput("Set last name: ")
            );

        string checkUsernameSql = "select 1 from admin_user where username = @Username";

        var row = db.LoadData<dynamic, dynamic>(checkUsernameSql, new { user.Username }, connectionStringName).FirstOrDefault();
        if (row == null)
        {
            string insertUserSql = "insert into admin_user (username, passwordHash, email, first_name, last_name) " +
                                   "values(@Username, @PasswordHash, @Email, @FirstName, @LastName)";

            db.SaveData<AdminUser>(insertUserSql, user, connectionStringName);

            Console.WriteLine($"New user created! Please memories your credentials before closing this window.");
        }
        else
        {
            Console.WriteLine("User already exists!");
        }




    }

    private static string GetUserInput(string message)
    {
        while (true)
        {
            Console.WriteLine(message);
            string output = Console.ReadLine() ?? "";
            if (String.IsNullOrWhiteSpace(output))
            {
                Console.WriteLine("Invalid input. Please try again.");
                Console.WriteLine("--------------------------------");
            }
            else
            {
                return output.Trim();
            }
        }

    }

    private static IConfiguration CreateConfig()
    {
        DirectoryInfo dir = TryGetSolutionDirectoryInfo();
        Dictionary<string, string?> configDict = new()
        {
            {"ConnectionStrings:Default", $"Data Source={dir.FullName}/../db/hjort.db;Version=3"}
        };

        IConfiguration config = new ConfigurationBuilder()
            .AddInMemoryCollection(configDict)
            .Build();

        return config;
    }

    private static DirectoryInfo TryGetSolutionDirectoryInfo()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory != null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }
        return directory;
    }
}

internal sealed record AdminUser(string Username = "", string PasswordHash = "", string Email = "", string FirstName = "", string LastName = "");