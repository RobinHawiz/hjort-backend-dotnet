using DataAccessLibrary.Models;

namespace DataAccessLibrary.DrinkMenu;
public interface IDrinkData
{
    void DeleteDrink(int id);
    bool ExistsDrink(int id);
    List<DrinkModel> GetAllDrinksByDrinkMenuId(int drinkMenuId);
    void InsertDrink(DrinkInsertModel drink);
    void UpdateDrink(DrinkUpdateModel drink);
}