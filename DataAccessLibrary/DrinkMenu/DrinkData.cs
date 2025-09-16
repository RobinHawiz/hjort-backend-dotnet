using DataAccessLibrary.Models;

namespace DataAccessLibrary.DrinkMenu;
public class DrinkData : IDrinkData
{
    private readonly ISqliteDataAccess _db;
    private readonly string _connectionStringName = "Default";

    public DrinkData(ISqliteDataAccess db)
    {
        _db = db;
    }

    public List<DrinkModel> GetAllDrinksByDrinkMenuId(int drinkMenuId)
    {
        string sql = "select id as Id, drink_menu_id as DrinkMenuId, name as Name from drink where drink_menu_id = @drinkMenuId order by id ASC";

        return _db.LoadData<DrinkModel, dynamic>(sql, new { drinkMenuId }, _connectionStringName);
    }

    public void InsertDrink(DrinkInsertModel drink)
    {
        string sql = "insert into drink (drink_menu_id, name) values(@DrinkMenuId, @Name)";

        _db.SaveData<DrinkInsertModel>(sql, drink, _connectionStringName);
    }

    public void UpdateDrink(DrinkUpdateModel drink)
    {
        string sql = "update drink set name = @Name where id = @Id";

        _db.SaveData<DrinkUpdateModel>(sql, drink, _connectionStringName);
    }

    public void DeleteDrink(int id)
    {
        string sql = "delete from drink where id = @id";

        _db.SaveData<dynamic>(sql, new { id }, _connectionStringName);
    }

    public bool ExistsDrink(int id)
    {
        string sql = "select * from drink where id = @id";

        return _db.LoadData<DrinkModel, dynamic>(sql, new { id }, _connectionStringName).FirstOrDefault() != null;
    }
}
