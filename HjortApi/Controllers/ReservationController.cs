using DataAccessLibrary.Models;
using HjortApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Exceptions;
using ServiceLibrary.Reservation;

namespace HjortApi.Controllers
{
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _service;
        private readonly ILogger<AdminController> _logger;

        public ReservationController(IReservationService service, ILogger<AdminController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("~/api/protected/reservations")] // => GET /api/protected/reservations
        public ActionResult<List<ReservationModel>> GetAllReservations()
        {
            try
            {
                List<ReservationModel> reservations = _service.GetAllReservations();
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving reservation data: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
            }
        }

        [HttpPost("~/api/public/reservations")] // => POST /api/public/reservations
        public ActionResult InsertReservation([FromBody] ReservationReqModel req)
        {
            try
            {
                _service.createReservation(new ReservationInsertModel()
                {
                    FirstName = req.FirstName,
                    LastName = req.LastName,
                    PhoneNumber = req.PhoneNumber,
                    Email = req.Email,
                    Message = req.Message,
                    GuestAmount = req.GuestAmount,
                    ReservationDate = req.ReservationDate
                });
                return Created();
            }
            catch (InvalidReservationDateException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ErrorResponse(ex.Field, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving reservation data: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
            }
        }

        [Authorize]
        [HttpDelete("~/api/protected/reservations/{id:int}")] // => DEL /api/protected/reservations/id
        public ActionResult DeleteReservation([FromRoute] int id)
        {
            try
            {
                _service.deleteReservation(id);
                return NoContent();
            }
            catch(InvalidReservationIdException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ErrorResponse(ex.Field, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving reservation data: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
            }
        }
    }
}
