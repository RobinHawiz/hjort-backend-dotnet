using DataAccessLibrary.Models;
using HjortApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.DrinkMenu;
using ServiceLibrary.Exceptions;

namespace HjortApi.Controllers;

[ApiController]
public class DrinkController : ControllerBase
{
    private readonly IDrinkService _service;
    private readonly ILogger<DrinkController> _logger;

    public DrinkController(IDrinkService service, ILogger<DrinkController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet("~/api/public/drink/{drinkMenuId:int}")] // => GET /api/public/drink/drinkMenuId
    public ActionResult<List<DrinkModel>> GetAllDrinksByDrinkMenuId([FromRoute]int drinkMenuId)
    {
        try
        {
            List<DrinkModel> drinks = _service.GetAllDrinksByDrinkMenuId(drinkMenuId);
            return Ok(drinks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving drink data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }

    [Authorize]
    [HttpPost("~/api/protected/drink")] // => POST /api/protected/drink
    public ActionResult CreateDrink(DrinkCreateReqModel drink)
    {
        try
        {
            _service.CreateDrink(new DrinkInsertModel()
            {
                DrinkMenuId = drink.DrinkMenuId,
                Name = drink.Name
            });
            return Created();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving drink data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }

    [Authorize]
    [HttpPut("~/api/protected/drink/{id:int}")] // => PUT /api/protected/drink/id
    public ActionResult UpdateDrink([FromRoute] int id, DrinkUpdateReqModel drink)
    {
        try
        {
            _service.UpdateDrink(new DrinkUpdateModel()
            {
                Id = id,
                Name = drink.Name,
            });

            return Created();
        }
        catch (InvalidDrinkIdException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(new ErrorResponse(ex.Field, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving drink data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }

    [Authorize]
    [HttpDelete("~/api/protected/drink/{id:int}")] // => DEL /api/protected/drink/id
    public ActionResult DeleteDrink([FromRoute] int id)
    {
        try
        {
            _service.DeleteDrink(id);

            return Created();
        }
        catch (InvalidDrinkIdException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(new ErrorResponse(ex.Field, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving drink data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }
}
