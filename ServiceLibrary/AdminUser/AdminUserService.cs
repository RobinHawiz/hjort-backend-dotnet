using DataAccessLibrary.AdminUser;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceLibrary.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace HjortApi.Services;

public class AdminUserService : IAdminUserService
{
    private readonly IAdminUserData _data;
    private readonly IConfiguration _config;

    public AdminUserService(IAdminUserData data, IConfiguration config)
    {
        _data = data;
        _config = config;
    }

    public string Authenticate(string username, string password)
    {
        AdminUserModel? adminUser = _data.GetOneUser(username, password);

        if (adminUser == null)
        {
            throw new InvalidCredException();
        }
        if (BC.Verify(password, adminUser.PasswordHash) == false)
        {
            throw new InvalidCredException();
        }
        else
        {
            return GenerateToken();
        } 
    }

    private string GenerateToken()
    {
        var secretKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(
            _config.GetValue<string>("Authentication:SecretKey") ?? throw new InvalidOperationException("Authentication:SecretKey is missing.")));

        var SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _config.GetValue<string>("Authentication:Issuer"),
            _config.GetValue<string>("Authentication:Audience"),
            null,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(1),
            SigningCredentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
