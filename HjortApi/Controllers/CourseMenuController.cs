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

    [HttpGet("~/api/public/course-menu")] // => GET /api/public/course-menu
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

    [Authorize]
    [HttpPut("~/api/protected/course-menu/{id:int}")] // => PUT /api/protected/course-menu/id
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
            return Created();
        }
        catch(InvalidCourseMenuIdException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(new ErrorResponse(ex.Field, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving course menu data: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }

    }
}
