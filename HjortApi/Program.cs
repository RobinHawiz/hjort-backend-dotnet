using DataAccessLibrary;
using DataAccessLibrary.AdminUser;
using DataAccessLibrary.Reservation;
using HjortApi.Endpoints;
using HjortApi.Models;
using HjortApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServiceLibrary.Reservation;
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
builder.Services.AddSingleton<IReservationData, ReservationData>();
builder.Services.AddSingleton<IReservationService, ReservationService>();
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

builder.Services.Configure<ApiBehaviorOptions>(opts =>
{
    opts.InvalidModelStateResponseFactory = actioncontext =>
    {
        List<ErrorResponse> errors = new();
        foreach (var modelStateEntry in actioncontext.ModelState)
        {
            ErrorResponse error = new (modelStateEntry.Key, modelStateEntry.Value.Errors.Select(me => me.ErrorMessage).First().ToString());
            errors.Add(error);
        }
        return new BadRequestObjectResult(errors);
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddUtilityEndpoints();

app.UseHttpsRedirection();

app.UseCors(corsPolicy);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();