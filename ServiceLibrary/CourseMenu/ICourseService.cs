using DataAccessLibrary.Models;

namespace ServiceLibrary.CourseMenu
{
    public interface ICourseService
    {
        List<CourseModel> GetAllCoursesByCourseMenuId(int courseMenuId);
        void CreateCourse(CourseInsertModel course);
        void DeleteCourse(int id);
        void UpdateCourse(CourseUpdateModel course);
    }
}