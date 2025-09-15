using DataAccessLibrary.Models;

namespace DataAccessLibrary.CourseMenu;

public class CourseMenuData : ICourseMenuData
{
    private readonly ISqliteDataAccess _db;
    private readonly string _connectionStringName = "Default";

    public CourseMenuData(ISqliteDataAccess db)
    {
        _db = db;
    }

    public List<CourseMenuModel> GetAllCourseMenus()
    {
        string sql = "select id as Id, title as Title, price_tot as PriceTot from course_menu order by id ASC";

        return _db.LoadData<CourseMenuModel, dynamic>(sql, new { }, _connectionStringName);
    }

    public void UpdateCourseMenu(CourseMenuModel courseMenu)
    {
        string sql = "update course_menu set title = @Title, price_tot = @PriceTot where id = @Id";

        _db.SaveData<CourseMenuModel>(sql, courseMenu, _connectionStringName);
    }

    public bool ExistsCourseMenu(int id)
    {
        string sql = "select * from course_menu where id = @id";

        return _db.LoadData<CourseMenuModel, dynamic>(sql, new { id }, _connectionStringName).FirstOrDefault() != null;
    }
}
