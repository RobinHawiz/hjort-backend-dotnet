using DataAccessLibrary.Models;

namespace DataAccessLibrary.DrinkMenu
{
    public interface IDrinkMenuData
    {
        bool ExistsDrinkMenu(int id);
        List<DrinkMenuModel> GetAllDrinkMenus();
        void UpdateDrinkMenu(DrinkMenuModel drinkMenu);
    }
}