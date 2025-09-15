using DataAccessLibrary.Models;

namespace DataAccessLibrary.CourseMenu;

public class CourseData : ICourseData
{
    private readonly ISqliteDataAccess _db;
    private readonly string _connectionStringName = "Default";

    public CourseData(ISqliteDataAccess db)
    {
        _db = db;
    }

    public List<CourseModel> GetAllCoursesByCourseMenuId(int courseMenuId)
    {
        string sql = "select id as Id, course_menu_id as CourseMenuId, name as Name, type as Type from course where course_menu_id = @courseMenuId order by id ASC";

        return _db.LoadData<CourseModel, dynamic>(sql, new { courseMenuId }, _connectionStringName);
    }

    public void InsertCourse(CourseInsertModel course)
    {
        string sql = "insert into course (course_menu_id, name, type) values(@CourseMenuId, @Name, @Type)";

        _db.SaveData<CourseInsertModel>(sql, course, _connectionStringName);
    }

    public void UpdateCourse(CourseUpdateModel course)
    {
        string sql = "update course set name = @Name, type = @Type where id = @Id";

        _db.SaveData<CourseUpdateModel>(sql, course, _connectionStringName);
    }

    public void DeleteCourse(int id)
    {
        string sql = "delete from course where id = @id";

        _db.SaveData<dynamic>(sql, new { id }, _connectionStringName);
    }

    public bool ExistsCourse(int id)
    {
        string sql = "select * from course where id = @id";

        return _db.LoadData<CourseModel, dynamic>(sql, new { id }, _connectionStringName).FirstOrDefault() != null;
    }
}
