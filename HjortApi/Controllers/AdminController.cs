using HjortApi.Models;
using HjortApi.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceLibrary.Exceptions;

namespace HjortApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IAdminUserService _service;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IAdminUserService service, ILogger<AdminController> logger) {
        _service = service;
        _logger = logger;
    }

    [HttpPost("login")] // => POST /api/admin/login
    public ActionResult<string> Login([FromBody] AdminUserReqModel req)
    {
        try
        {
            string token = _service.Authenticate(req.Username, req.Password);
            return Ok(JsonConvert.SerializeObject(token));
        }
        catch (InvalidCredException ex)
        {
            _logger.LogError(ex, ex.Message);
            return Unauthorized(new ErrorResponse(ex.Field, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error authenticating admin user: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
        }
    }
}
