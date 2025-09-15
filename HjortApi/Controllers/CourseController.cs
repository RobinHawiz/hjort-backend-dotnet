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

    [HttpGet("~/api/public/course/{courseMenuId:int}")] // => GET /api/public/course/courseMenuId
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

    [Authorize]
    [HttpPost("~/api/protected/course")] // => POST /api/protected/course
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
            _logger.LogError(ex, $"Error retrieving course data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }

    [Authorize]
    [HttpPut("~/api/protected/course/{id:int}")] // => PUT /api/protected/course/id
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

            return Created();
        }
        catch (InvalidCourseIdException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(new ErrorResponse(ex.Field, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving course data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }

    [Authorize]
    [HttpDelete("~/api/protected/course/{id:int}")] // => DEL /api/protected/course/id
    public ActionResult DeleteCourse([FromRoute] int id)
    {
        try
        {
            _service.DeleteCourse(id);

            return Created();
        }
        catch (InvalidCourseIdException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(new ErrorResponse(ex.Field, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving course data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }
}
