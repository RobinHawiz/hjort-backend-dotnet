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
        service.AddSingleton<ISqliteDataAccess, SqliteDataAccess>();
        service.AddSingleton<IAdminUserData, AdminUserData>();
        service.AddSingleton<IReservationData, ReservationData>();
        service.AddSingleton<ICourseMenuData, CourseMenuData>();
        service.AddSingleton<ICourseData, CourseData>();
        service.AddSingleton<IDrinkMenuData, DrinkMenuData>();
        service.AddSingleton<IDrinkData, DrinkData>();
    }

    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IAdminUserService, AdminUserService>();
        services.AddSingleton<IReservationService, ReservationService>();
        services.AddSingleton<ICourseMenuService, CourseMenuService>();
        services.AddSingleton<ICourseService, CourseService>();
        services.AddSingleton<IDrinkMenuService, DrinkMenuService>();
        services.AddSingleton<IDrinkService, DrinkService>();
    }
}
