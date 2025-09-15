using DataAccessLibrary.CourseMenu;
using DataAccessLibrary.Models;
using ServiceLibrary.Exceptions;

namespace ServiceLibrary.CourseMenu;

public class CourseService : ICourseService
{
    private readonly ICourseData _data;
    public CourseService(ICourseData data)
    {
        _data = data;
    }

    public List<CourseModel> GetAllCoursesByCourseMenuId(int courseMenuId)
    {
        return _data.GetAllCoursesByCourseMenuId(courseMenuId);
    }

    public void CreateCourse(CourseInsertModel course)
    {
        _data.InsertCourse(course);
    }

    public void UpdateCourse(CourseUpdateModel course)
    {
        bool CourseExists = _data.ExistsCourse(course.Id);
        if (!CourseExists)
        {
            throw new InvalidCourseIdException();
        }
        _data.UpdateCourse(course);
    }

    public void DeleteCourse(int id)
    {
        bool CourseExists = _data.ExistsCourse(id);
        if (!CourseExists)
        {
            throw new InvalidCourseIdException();
        }
        _data.DeleteCourse(id);
    }
}
