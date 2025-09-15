using DataAccessLibrary.Models;

namespace ServiceLibrary.CourseMenu
{
    public interface ICourseMenuService
    {
        List<CourseMenuModel> GetAllCourseMenus();
        void UpdateCourseMenu(CourseMenuModel model);
    }
}