using DataAccessLibrary.Models;
using HjortApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.CourseMenu;
using ServiceLibrary.Exceptions;

namespace HjortApi.Controllers;

[ApiController]
public class CourseController : ControllerBase
{
    private readonly ICourseService _service;
    private readonly ILogger<CourseController> _logger;

    public CourseController(ICourseService service, ILogger<CourseController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Returns all courses for a given course menu.
    /// </summary>
    /// <remarks>
    /// Parameters:
    /// - <c>courseMenuId</c>: ID of the course menu to list courses for.
    ///
    /// Behavior:
    /// - Returns <c>200 OK</c> with an array of <c>CourseModel</c>.
    /// 
    /// - If the menu has no courses or the menu ID doesn't exist, returns <c>200 OK</c> with <c>[]</c>.
    ///
    /// Responses:
    /// - 200: <c>List&lt;CourseModel&gt;</c>
    /// 
    /// - 500: <c>ErrorResponse { field: "server", message: "Internal Server Error" }</c>
    /// </remarks>
    /// <response code="200"><c>List&lt;CourseModel&gt;</c> for the specified menu (empty array if none).</response>
    /// <response code="500">Unexpected server error.</response>
    [HttpGet("~/api/public/course/{courseMenuId:int}")] // => GET /api/public/course/{courseMenuId}
    [ProducesResponseType(typeof(List<CourseModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public ActionResult<List<CourseModel>> GetAllCoursesByCourseMenuId([FromRoute] int courseMenuId)
    {
        try
        {
            List<CourseModel> courses = _service.GetAllCoursesByCourseMenuId(courseMenuId);

            return Ok(courses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving course data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }

    /// <summary>
    /// Creates a new course under a course menu.
    /// </summary>
    /// <remarks>
    /// Body:
    /// - <c>CourseMenuId</c> &gt;= 1 and must reference an existing course menu.
    /// 
    /// - <c>Name</c> required, 1–200 chars.
    /// 
    /// - <c>Type</c> required, one of <c>starter</c>, <c>main</c>, <c>dessert</c>.
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
    /// <response code="201">Course created.</response>
    /// <response code="400"><c>List&lt;ErrorResponse&gt;</c> (invalid body).</response>
    /// <response code="401">Missing or invalid JWT.</response>
    /// <response code="500">Unexpected server error.</response>
    [Authorize]
    [HttpPost("~/api/protected/course")] // => POST /api/protected/course
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(List<ErrorResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public ActionResult CreateCourse(CourseCreateReqModel course)
    {
        try
        {
            _service.CreateCourse(new CourseInsertModel()
            {
                CourseMenuId = course.CourseMenuId,
                Name = course.Name,
                Type = course.Type,
            });

            return Created();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error when creating course data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }

    /// <summary>
    /// Updates an existing course.
    /// </summary>
    /// <remarks>
    /// Parameters:
    /// - <c>id</c>: course ID to update.
    ///
    /// Body:
    /// - <c>Name</c> required, 1–200 chars.
    /// 
    /// - <c>Type</c> required, one of <c>starter</c>, <c>main</c>, <c>dessert</c>.
    ///
    /// Security:
    /// - Requires JWT Bearer (admin).
    /// 
    /// Error semantics:
    /// - 400 (domain): <c>ErrorResponse</c> when the <c>id</c> does not correspond to an existing course.
    /// 
    /// - 400 (validation): <c>List&lt;ErrorResponse&gt;</c> for invalid body.
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
    /// <response code="204">Course updated.</response>
    /// <response code="400">Either <c>ErrorResponse</c> (invalid <c>id</c>) or <c>List&lt;ErrorResponse&gt;</c> (invalid body).</response>
    /// <response code="401">Missing or invalid JWT.</response>
    /// <response code="500">Unexpected server error.</response>
    [Authorize]
    [HttpPut("~/api/protected/course/{id:int}")] // => PUT /api/protected/course/{id}
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<ErrorResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public ActionResult UpdateCourse([FromRoute] int id, CourseUpdateReqModel course)
    {
        try
        {
            _service.UpdateCourse(new CourseUpdateModel()
            {
                Id = id,
                Name = course.Name,
                Type = course.Type,
            });

            return NoContent();
        }
        catch (InvalidCourseIdException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(new ErrorResponse(ex.Field, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error when updating course data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }

    /// <summary>
    /// Deletes a course by ID.
    /// </summary>
    /// <remarks>
    /// Parameters:
    /// - <c>id</c>: course ID to delete.
    ///
    /// Security:
    /// - Requires JWT Bearer (admin).
    ///
    /// Responses:
    /// - 204: <c>No Content</c>
    /// 
    /// - 400 (domain): <c>ErrorResponse { field: "id", message: "The course with this id does not exist!" }</c>
    /// 
    /// - 401: <c>Unauthorized</c>
    /// 
    /// - 500: <c>ErrorResponse { field: "server", message: "Internal Server Error" }</c>
    /// </remarks>
    /// <response code="204">Course deleted.</response>
    /// <response code="400"><c>ErrorResponse</c> (invalid <c>id</c>).</response>
    /// <response code="401">Missing or invalid JWT.</response>
    /// <response code="500">Unexpected server error.</response>
    [Authorize]
    [HttpDelete("~/api/protected/course/{id:int}")] // => DEL /api/protected/course/{id}
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public ActionResult DeleteCourse([FromRoute] int id)
    {
        try
        {
            _service.DeleteCourse(id);

            return NoContent();
        }
        catch (InvalidCourseIdException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(new ErrorResponse(ex.Field, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error when deleting course data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }
}
