using DataAccessLibrary.Models;

namespace DataAccessLibrary.CourseMenu;

public interface ICourseData
{
    void DeleteCourse(int id);
    bool ExistsCourse(int id);
    List<CourseModel> GetAllCoursesByCourseMenuId(int courseMenuId);
    void InsertCourse(CourseInsertModel course);
    void UpdateCourse(CourseUpdateModel course);
}