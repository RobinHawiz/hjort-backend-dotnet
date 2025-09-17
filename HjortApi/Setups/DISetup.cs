using DataAccessLibrary;
using DataAccessLibrary.AdminUser;
using DataAccessLibrary.CourseMenu;
using DataAccessLibrary.DrinkMenu;
using DataAccessLibrary.Reservation;
using HjortApi.Services;
using ServiceLibrary.CourseMenu;
using ServiceLibrary.DrinkMenu;
using ServiceLibrary.Reservation;

namespace HjortApi.Setups;

public static class DISetup
{
    public static void AddDatabaseAccess(this IServiceCollection service)
    {
        service.AddScoped<ISqliteDataAccess, SqliteDataAccess>();
        service.AddScoped<IAdminUserData, AdminUserData>();
        service.AddScoped<IReservationData, ReservationData>();
        service.AddScoped<ICourseMenuData, CourseMenuData>();
        service.AddScoped<ICourseData, CourseData>();
        service.AddScoped<IDrinkMenuData, DrinkMenuData>();
        service.AddScoped<IDrinkData, DrinkData>();
    }

    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAdminUserService, AdminUserService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<ICourseMenuService, CourseMenuService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IDrinkMenuService, DrinkMenuService>();
        services.AddScoped<IDrinkService, DrinkService>();
    }
}
