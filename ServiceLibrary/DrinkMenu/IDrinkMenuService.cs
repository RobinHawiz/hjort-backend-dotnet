using DataAccessLibrary.Models;

namespace ServiceLibrary.DrinkMenu;
public interface IDrinkMenuService
{
    List<DrinkMenuModel> GetAllDrinkMenus();
    void UpdateDrinkMenu(DrinkMenuModel model);
}