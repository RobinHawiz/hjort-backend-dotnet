using DataAccessLibrary.DrinkMenu;
using DataAccessLibrary.Models;
using ServiceLibrary.Exceptions;

namespace ServiceLibrary.DrinkMenu;
public class DrinkService : IDrinkService
{
    private readonly IDrinkData _data;

    public DrinkService(IDrinkData data)
    {
        _data = data;
    }

    public List<DrinkModel> GetAllDrinksByDrinkMenuId(int id)
    {
        return _data.GetAllDrinksByDrinkMenuId(id);
    }

    public void CreateDrink(DrinkInsertModel drink)
    {
        _data.InsertDrink(drink);
    }

    public void UpdateDrink(DrinkUpdateModel drink)
    {
        bool drinkExists = _data.ExistsDrink(drink.Id);
        if (!drinkExists)
        {
            throw new InvalidDrinkIdException();
        }

        _data.UpdateDrink(drink);
    }

    public void DeleteDrink(int id)
    {
        bool drinkExists = _data.ExistsDrink(id);
        if (!drinkExists)
        {
            throw new InvalidDrinkIdException();
        }

        _data.DeleteDrink(id);
    }
}
