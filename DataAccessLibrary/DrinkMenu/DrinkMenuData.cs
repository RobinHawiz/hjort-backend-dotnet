using DataAccessLibrary.Models;

namespace DataAccessLibrary.DrinkMenu;

public class DrinkMenuData : IDrinkMenuData
{
    private readonly ISqliteDataAccess _db;
    private readonly string _connectionStringName = "Default";

    public DrinkMenuData(ISqliteDataAccess db)
    {
        _db = db;
    }

    public List<DrinkMenuModel> GetAllDrinkMenus()
    {
        string sql = "select id as Id, title as Title, subtitle as Subtitle, price_tot as PriceTot from drink_menu order by id ASC";

        return _db.LoadData<DrinkMenuModel, dynamic>(sql, new { }, _connectionStringName);
    }

    public void UpdateDrinkMenu(DrinkMenuModel drinkMenu)
    {
        string sql = "update drink_menu set title = @Title, subtitle = @Subtitle, price_tot = @PriceTot where id = @Id";

        _db.SaveData<DrinkMenuModel>(sql, drinkMenu, _connectionStringName);
    }

    public bool ExistsDrinkMenu(int id)
    {
        string sql = "select * from drink_menu where id = @id";

        return _db.LoadData<DrinkMenuModel, dynamic>(sql, new { id }, _connectionStringName).FirstOrDefault() != null;
    }
}
