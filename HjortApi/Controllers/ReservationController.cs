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
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(IReservationService service, ILogger<ReservationController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Returns all reservations.
        /// </summary>
        /// <remarks>
        /// Behavior:
        /// - Returns <c>200 OK</c> with an array of <c>ReservationModel</c>.
        /// 
        /// - If there are no reservations, returns <c>200 OK</c> with <c>[]</c>.
        ///
        /// Security:
        /// - Requires JWT Bearer (admin).
        ///
        /// Responses:
        /// - 200: <c>List&lt;ReservationModel&gt;</c>
        /// 
        /// - 401: <c>Unauthorized</c>
        /// 
        /// - 500: <c>ErrorResponse { field: "server", message: "Internal Server Error" }</c>
        /// </remarks>
        /// <response code="200"><c>List&lt;ReservationModel&gt;</c> (empty array if none).</response>
        /// <response code="401">Missing or invalid JWT.</response>
        /// <response code="500">Unexpected server error.</response>
        [Authorize]
        [HttpGet("~/api/protected/reservations")] // => GET /api/protected/reservations
        [ProducesResponseType(typeof(List<ReservationModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Creates a new reservation.
        /// </summary>
        /// <remarks>
        /// Body:
        /// - <c>FirstName</c>: required, 1–50 chars.
        /// 
        /// - <c>LastName</c>: required, 1–50 chars.
        /// 
        /// - <c>PhoneNumber</c>: required, 1–20 chars.
        /// 
        /// - <c>Email</c>: required, 1–128 chars.
        /// 
        /// - <c>Message</c>: required, max 1000 chars.
        /// 
        /// - <c>GuestAmount</c>: required, 1–6.
        /// 
        /// - <c>ReservationDate</c>: required, ISO-8601 string (e.g., <c>2025-09-22T18:30:00.000Z</c>).
        ///
        /// Domain rule:
        /// - <c>ReservationDate</c> must be strictly in the future.
        ///
        /// Error semantics:
        /// - 400 (validation): <c>List&lt;ErrorResponse&gt;</c> from model validation.
        /// 
        /// - 400 (domain): <c>ErrorResponse</c> for invalid <c>ReservationDate</c>.
        ///
        /// Responses:
        /// - 201: <c>Created</c>
        /// 
        /// - 400: Validation or domain errors (see above)
        /// 
        /// - 500: <c>ErrorResponse { field: "server", message: "Internal Server Error" }</c>
        /// </remarks>
        /// <response code="201">Reservation created.</response>
        /// <response code="400">Either <c>List&lt;ErrorResponse&gt;</c> (invalid body) or <c>ErrorResponse</c> (invalid <c>reservationDate</c>).</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPost("~/api/public/reservations")] // => POST /api/public/reservations
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(List<ErrorResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
                _logger.LogError(ex, $"Error when creating reservation data: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
            }
        }

        /// <summary>
        /// Deletes a reservation by ID.
        /// </summary>
        /// <remarks>
        /// Parameters:
        /// - <c>id</c>: reservation ID to delete.
        ///
        /// Security:
        /// - Requires JWT Bearer (admin).
        ///
        /// Responses:
        /// - 204: <c>No Content</c>
        /// 
        /// - 400 (domain): <c>ErrorResponse { field: "id", message: "The reservation with this id does not exist!" }</c>
        /// 
        /// - 401: <c>Unauthorized</c>
        /// 
        /// - 500: <c>ErrorResponse { field: "server", message: "Internal Server Error" }</c>
        /// </remarks>
        /// <response code="204">Reservation deleted.</response>
        /// <response code="400"><c>ErrorResponse</c> (invalid <c>id</c>).</response>
        /// <response code="401">Missing or invalid JWT.</response>
        /// <response code="500">Unexpected server error.</response>
        [Authorize]
        [HttpDelete("~/api/protected/reservations/{id:int}")] // => DEL /api/protected/reservations/{id}
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
                _logger.LogError(ex, $"Error when deleting reservation data: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("server", "Internal Server Error"));
            }
        }
    }
}
