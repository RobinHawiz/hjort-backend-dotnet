using DataAccessLibrary;
using DataAccessLibrary.AdminUser;
using HjortApi.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    opts.JsonSerializerOptions.DictionaryKeyPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISqliteDataAccess, SqliteDataAccess>();
builder.Services.AddSingleton<IAdminUserData, AdminUserData>();
builder.Services.AddSingleton<IAdminUserService, AdminUserService>();
builder.Services.AddAuthentication("Bearer").AddJwtBearer(opts =>
{
    opts.TokenValidationParameters = new()
    {
        ValidateIssuer = true, 
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
        ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(
            builder.Configuration.GetValue<string>("Authentication:SecretKey")))
    };
});
var corsPolicy = "Test";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(corsPolicy);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();