using DataAccessLibrary.Models;
using HjortApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.DrinkMenu;
using ServiceLibrary.Exceptions;

namespace HjortApi.Controllers;

[ApiController]
public class DrinkMenuController : ControllerBase
{
    private readonly IDrinkMenuService _service;
    private readonly ILogger<DrinkMenuController> _logger;

    public DrinkMenuController(IDrinkMenuService service, ILogger<DrinkMenuController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet("~/api/public/drink-menu")] // => GET /api/public/drink-menu
    public ActionResult<List<DrinkMenuModel>> GetAllDrinkMenus()
    {
        try
        {
            List<DrinkMenuModel> drinkMenus = _service.GetAllDrinkMenus();
            return Ok(drinkMenus);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, $"Error retrieving drink menu data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }

    [Authorize]
    [HttpPut("~/api/protected/drink-menu/{id:int}")] // => PUT /api/protected/drink-menu/id
    public ActionResult UpdateDrinkMenu([FromRoute] int id, DrinkMenuReqModel drinkMenu) 
    {
        try
        {
            _service.UpdateDrinkMenu(new DrinkMenuModel()
            {
                Id = id,
                Title = drinkMenu.Title,
                Subtitle = drinkMenu.Subtitle,
                PriceTot = drinkMenu.PriceTot
            });
            return Created();
        }
        catch(InvalidDrinkMenuIdException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(new ErrorResponse(ex.Field, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving drink menu data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }
}
