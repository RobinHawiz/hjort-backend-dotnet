using DataAccessLibrary;
using DataAccessLibrary.AdminUser;
using DataAccessLibrary.CourseMenu;
using DataAccessLibrary.DrinkMenu;
using DataAccessLibrary.Reservation;
using HjortApi.Endpoints;
using HjortApi.Models;
using HjortApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServiceLibrary.CourseMenu;
using ServiceLibrary.DrinkMenu;
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
builder.Services.AddSingleton<ICourseMenuData, CourseMenuData>();
builder.Services.AddSingleton<ICourseMenuService, CourseMenuService>();
builder.Services.AddSingleton<ICourseData, CourseData>();
builder.Services.AddSingleton<ICourseService, CourseService>();
builder.Services.AddSingleton<IDrinkMenuData, DrinkMenuData>();
builder.Services.AddSingleton<IDrinkMenuService, DrinkMenuService>();
builder.Services.AddSingleton<IDrinkData, DrinkData>();
builder.Services.AddSingleton<IDrinkService, DrinkService>();
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
var corsPolicy = "Prod";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
    {
        policy.WithOrigins("https://robinhawiz.github.io").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.Configure<ApiBehaviorOptions>(opts =>
{
    opts.InvalidModelStateResponseFactory = actioncontext =>
    {
        List<ErrorResponse> errors = new();
        foreach (var modelStateEntry in actioncontext.ModelState)
        {
            string key = modelStateEntry.Key;
            // Skip entries with no errors (e.g., route params like `id` still appear in ModelState).
            if (key != "id")
            {
                if (key == "Password")
                {
                    // Rewrite so error response references the property the client actually sends, which is PasswordHash.
                    key = "PasswordHash";
                }
                ErrorResponse error = new(char.ToLower(key[0]) + key[1..], modelStateEntry.Value.Errors.Select(me => me.ErrorMessage).DefaultIfEmpty("").First());
                errors.Add(error);
            }
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