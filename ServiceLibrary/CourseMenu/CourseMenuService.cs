using DataAccessLibrary.CourseMenu;
using DataAccessLibrary.Models;
using ServiceLibrary.Exceptions;

namespace ServiceLibrary.CourseMenu;

public class CourseMenuService : ICourseMenuService
{
    public ICourseMenuData _data;

    public CourseMenuService(ICourseMenuData data)
    {
        _data = data;
    }

    public List<CourseMenuModel> GetAllCourseMenus()
    {
        return _data.GetAllCourseMenus();
    }

    public void UpdateCourseMenu(CourseMenuModel model)
    {
        bool courseMenuExists = _data.ExistsCourseMenu(model.Id);
        if (!courseMenuExists)
        {
            throw new InvalidCourseMenuIdException();
        }
        _data.UpdateCourseMenu(model);
    }
}
