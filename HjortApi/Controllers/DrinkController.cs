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

    /// <summary>
    /// Returns all drinks for a given drink menu.
    /// </summary>
    /// <remarks>
    /// Parameters:
    /// - <c>drinkMenuId</c>: ID of the drink menu to list drinks for.
    ///
    /// Behavior:
    /// - Returns <c>200 OK</c> with an array of <c>DrinkModel</c>.
    /// 
    /// - If the menu has no drinks or the menu ID doesn't exist, returns <c>200 OK</c> with <c>[]</c>.
    ///
    /// Responses:
    /// - 200: <c>List&lt;DrinkModel&gt;</c>
    /// 
    /// - 500: <c>ErrorResponse { field: "server", message: "Internal Server Error" }</c>
    /// </remarks>
    /// <response code="200"><c>List&lt;DrinkModel&gt;</c> for the specified menu (empty array if none).</response>
    /// <response code="500">Unexpected server error.</response>
    [HttpGet("~/api/public/drink/{drinkMenuId:int}")] // => GET /api/public/drink/{drinkMenuId}
    [ProducesResponseType(typeof(List<DrinkModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Creates a new drink under a drink menu.
    /// </summary>
    /// <remarks>
    /// Requirements:
    /// - <c>DrinkMenuId</c> &gt;= 1.
    /// 
    /// - <c>Name</c> required, 1–200 chars.
    ///
    /// Security:
    /// - Requires JWT Bearer (admin).
    ///
    /// Responses:
    /// - 201: <c>Created</c>
    /// 
    /// - 400 (validation): <c>List&lt;ErrorResponse&gt;</c> for invalid body.
    /// 
    /// - 401: <c>Unauthorized</c>
    /// 
    /// - 500: <c>ErrorResponse { field: "server", message: "Internal Server Error" }</c>
    /// </remarks>
    /// <response code="201">Drink created.</response>
    /// <response code="400"><c>List&lt;ErrorResponse&gt;</c> (invalid body).</response>
    /// <response code="401">Missing or invalid JWT.</response>
    /// <response code="500">Unexpected server error.</response>
    [Authorize]
    [HttpPost("~/api/protected/drink")] // => POST /api/protected/drink
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(List<ErrorResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
            _logger.LogError(ex, $"Error when creating drink data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }

    /// <summary>
    /// Updates an existing drink.
    /// </summary>
    /// <remarks>
    /// Parameters:
    /// - <c>id</c>: drink ID to update.
    ///
    /// Body:
    /// - <c>Name</c> required, 1–200 chars.
    ///
    /// Security:
    /// - Requires JWT Bearer (admin).
    ///
    /// Error semantics:
    /// - 400 (domain): <c>ErrorResponse</c> when the <c>id</c> does not correspond to an existing drink.
    /// 
    /// - 400 (validation): <c>List&lt;ErrorResponse&gt;</c> for invalid body fields.
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
    /// <response code="204">Drink updated.</response>
    /// <response code="400">Either <c>ErrorResponse</c> (invalid <c>id</c>) or <c>List&lt;ErrorResponse&gt;</c> (invalid body).</response>
    /// <response code="401">Missing or invalid JWT.</response>
    /// <response code="500">Unexpected server error.</response>
    [Authorize]
    [HttpPut("~/api/protected/drink/{id:int}")] // => PUT /api/protected/drink/{id}
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<ErrorResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public ActionResult UpdateDrink([FromRoute] int id, DrinkUpdateReqModel drink)
    {
        try
        {
            _service.UpdateDrink(new DrinkUpdateModel()
            {
                Id = id,
                Name = drink.Name,
            });

            return NoContent();
        }
        catch (InvalidDrinkIdException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(new ErrorResponse(ex.Field, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error when updating drink data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }

    /// <summary>
    /// Deletes a drink by ID.
    /// </summary>
    /// <remarks>
    /// Parameters:
    /// - <c>id</c>: drink ID to delete.
    ///
    /// Security:
    /// - Requires JWT Bearer (admin).
    ///
    /// Responses:
    /// - 204: <c>No Content</c>
    /// 
    /// - 400 (domain): <c>ErrorResponse { field: "id", message: "The drink with this id does not exist!" }</c>
    /// 
    /// - 401: <c>Unauthorized</c>
    /// 
    /// - 500: <c>ErrorResponse { field: "server", message: "Internal Server Error" }</c>
    /// </remarks>
    /// <response code="204">Drink deleted.</response>
    /// <response code="400"><c>ErrorResponse</c> (invalid <c>id</c>).</response>
    /// <response code="401">Missing or invalid JWT.</response>
    /// <response code="500">Unexpected server error.</response>
    [Authorize]
    [HttpDelete("~/api/protected/drink/{id:int}")] // => DEL /api/protected/drink/{id}
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public ActionResult DeleteDrink([FromRoute] int id)
    {
        try
        {
            _service.DeleteDrink(id);

            return NoContent();
        }
        catch (InvalidDrinkIdException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(new ErrorResponse(ex.Field, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error when deleting drink data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }
}
