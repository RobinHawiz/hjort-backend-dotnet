using DataAccessLibrary.Models;

namespace ServiceLibrary.DrinkMenu;
public interface IDrinkService
{
    void CreateDrink(DrinkInsertModel drink);
    void DeleteDrink(int id);
    List<DrinkModel> GetAllDrinksByDrinkMenuId(int id);
    void UpdateDrink(DrinkUpdateModel drink);
}