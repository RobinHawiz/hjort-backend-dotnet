using DataAccessLibrary.DrinkMenu;
using DataAccessLibrary.Models;
using ServiceLibrary.Exceptions;

namespace ServiceLibrary.DrinkMenu;

public class DrinkMenuService : IDrinkMenuService
{
    private readonly IDrinkMenuData _data;

    public DrinkMenuService(IDrinkMenuData data)
    {
        _data = data;
    }

    public List<DrinkMenuModel> GetAllDrinkMenus()
    {
        return _data.GetAllDrinkMenus();
    }

    public void UpdateDrinkMenu(DrinkMenuModel model)
    {
        bool DrinkMenuExists = _data.ExistsDrinkMenu(model.Id);
        if (!DrinkMenuExists)
        {
            throw new InvalidDrinkMenuIdException();
        }

        _data.UpdateDrinkMenu(model);
    }
}
