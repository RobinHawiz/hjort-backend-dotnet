using DataAccessLibrary.Models;

namespace DataAccessLibrary.CourseMenu
{
    public interface ICourseMenuData
    {
        bool ExistsCourseMenu(int id);
        List<CourseMenuModel> GetAllCourseMenus();
        void UpdateCourseMenu(CourseMenuModel courseMenu);
    }
}