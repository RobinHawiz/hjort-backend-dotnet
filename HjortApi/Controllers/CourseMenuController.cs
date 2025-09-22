using DataAccessLibrary.Models;
using HjortApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.CourseMenu;
using ServiceLibrary.Exceptions;
using System.Reflection;

namespace HjortApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseMenuController : ControllerBase
{
    private readonly ICourseMenuService _service;
    private readonly ILogger<CourseMenuController> _logger;

    public CourseMenuController(ICourseMenuService service, ILogger<CourseMenuController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Returns all course menus.
    /// </summary>
    /// <remarks>
    /// Behavior:
    /// - Returns <c>200 OK</c> with an array of <c>CourseMenuModel</c>.
    /// 
    /// - If there are no menus, returns <c>200 OK</c> with <c>[]</c>.
    ///
    /// Responses:
    /// - 200: <c>List&lt;CourseMenuModel&gt;</c>
    /// 
    /// - 500: <c>ErrorResponse { field: "server", message: "Internal Server Error" }</c>
    /// </remarks>
    /// <response code="200"><c>List&lt;CourseMenuModel&gt;</c>.</response>
    /// <response code="500">Unexpected server error.</response>
    [HttpGet("~/api/public/course-menu")] // => GET /api/public/course-menu
    [ProducesResponseType(typeof(List<CourseMenuModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public ActionResult<List<CourseMenuModel>> GetAllCourseMenus()
    {
        try
        {
            List<CourseMenuModel> courseMenus = _service.GetAllCourseMenus();
            return Ok(courseMenus);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, $"Error retrieving course menu data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }

    /// <summary>
    /// Updates an existing course menu.
    /// </summary>
    /// <remarks>
    /// Parameters:
    /// - <c>id</c>: course menu ID to update.
    ///
    /// Body:
    /// - <c>Name</c> required, 1–200 chars.
    ///
    /// Security:
    /// - Requires JWT Bearer (admin).
    ///
    /// Error semantics:
    /// - 400 (domain): <c>ErrorResponse</c> when the <c>id</c> does not correspond to an existing course menu.
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
    /// <response code="204">Course menu updated.</response>
    /// <response code="400">Either <c>ErrorResponse</c> (invalid <c>id</c>) or <c>List&lt;ErrorResponse&gt;</c> (invalid body).</response>
    /// <response code="401">Missing or invalid JWT.</response>
    /// <response code="500">Unexpected server error.</response>
    [Authorize]
    [HttpPut("~/api/protected/course-menu/{id:int}")] // => PUT /api/protected/course-menu/{id}
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<ErrorResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public ActionResult UpdateCourseMenu([FromRoute] int id, CourseMenuReqModel courseMenu)
    {
        try
        {
            _service.UpdateCourseMenu(new CourseMenuModel()
            {
                Id = id,
                Title = courseMenu.Title,
                PriceTot = courseMenu.PriceTot,
            });
            return NoContent();
        }
        catch(InvalidCourseMenuIdException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(new ErrorResponse(ex.Field, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error when updating course menu data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }

    }
}
