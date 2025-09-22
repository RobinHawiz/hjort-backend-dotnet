using HjortApi.Models;
using HjortApi.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceLibrary.Exceptions;
using Microsoft.Extensions.Logging;

namespace HjortApi.Controllers;

[ApiController]
public class AdminController : ControllerBase
{
    private readonly IAdminUserService _service;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IAdminUserService service, ILogger<AdminController> logger) {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Authenticates an admin and returns a JWT for protected endpoints.
    /// </summary>
    /// <remarks>
    /// Requirements:
    /// - Username and password must match an existing admin user.
    /// 
    /// Responses:
    /// - 200: <c>{ "token": "..." }</c> – JWT string
    /// 
    /// - 400 (validation): <c>List&lt;ErrorResponse&gt;</c> for invalid body.
    /// 
    /// - 401: <c>ErrorResponse { field: "login", message: "An admin user with this username or password does not exist!" }</c>
    /// 
    /// - 500: <c>ErrorResponse { field: "server", message: "Internal Server Error" }</c>
    /// </remarks>
    /// <response code="200">JWT bearer token used for /api/protected/* routes.</response>
    /// <response code="400"><c>List&lt;ErrorResponse&gt;</c> (invalid body).</response>
    /// <response code="401">Invalid credentials.</response>
    /// <response code="500">Unexpected server error.</response>
    [HttpPost("~/api/admin/login")] // => POST /api/admin/login
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ErrorResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
