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

    /// <summary>
    /// Returns all drink menus.
    /// </summary>
    /// <remarks>
    /// Behavior:
    /// - Returns <c>200 OK</c> with an array of <c>DrinkMenuModel</c>.
    /// 
    /// - If there are no drink menus, returns <c>200 OK</c> with <c>[]</c>.
    ///
    /// Responses:
    /// - 200: <c>List&lt;DrinkMenuModel&gt;</c>
    /// 
    /// - 500: <c>ErrorResponse { field: "server", message: "Internal Server Error" }</c>
    /// </remarks>
    /// <response code="200"><c>List&lt;DrinkMenuModel&gt;</c> (empty array if none).</response>
    /// <response code="500">Unexpected server error.</response>
    [HttpGet("~/api/public/drink-menu")] // => GET /api/public/drink-menu
    [ProducesResponseType(typeof(List<DrinkMenuModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Updates an existing drink menu.
    /// </summary>
    /// <remarks>
    /// Parameters:
    /// - <c>id</c>: drink menu ID to update.
    ///
    /// Body:
    /// - <c>Title</c>: optional, max 50 chars.
    /// 
    /// - <c>Subtitle</c>: optional, max 50 chars.
    /// 
    /// - <c>PriceTot</c>: required, positive integer (>= 1).
    ///
    /// Security:
    /// - Requires JWT Bearer (admin).
    ///
    /// Error semantics:
    /// - 400 (domain): <c>ErrorResponse</c> when the <c>id</c> does not correspond to an existing drink menu.
    /// 
    /// - 400 (validation): <c>List&lt;ErrorResponse&gt;</c> for invalid body fields (model validation).
    ///
    /// Responses:
    /// - 204: <c>No Content</c>
    /// 
    /// - 400: Domain or validation errors (see above)
    /// 
    /// - 401: <c>Unauthorized</c>
    /// 
    /// - 500: <c>ErrorResponse { field: "server", message: "Internal Server Error" }</c>
    /// </remarks>
    /// <response code="204">Drink menu updated.</response>
    /// <response code="400">Either <c>ErrorResponse</c> (invalid <c>id</c>) or <c>List&lt;ErrorResponse&gt;</c> (invalid body).</response>
    /// <response code="401">Missing or invalid JWT.</response>
    /// <response code="500">Unexpected server error.</response>
    [Authorize]
    [HttpPut("~/api/protected/drink-menu/{id:int}")] // => PUT /api/protected/drink-menu/{id}
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<ErrorResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
            return NoContent();
        }
        catch(InvalidDrinkMenuIdException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(new ErrorResponse(ex.Field, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error when updating drink menu data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }
}
