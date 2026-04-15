using Microsoft.AspNetCore.Mvc;
using reservations_api.DTOs.Requests;
using reservations_api.Services;

namespace reservations_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
  private readonly IReservationService _reservationService;

  public ReservationsController(IReservationService reservationService)
  {
    _reservationService = reservationService;
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateReservationRequest request)
  {
    if (!ModelState.IsValid)
    {
      return ValidationProblem(ModelState);
    }

    try
    {
      var createdReservation = await _reservationService.CreateAsync(request);
      return CreatedAtAction(
          nameof(Create),
          createdReservation);
    }
    catch (InvalidOperationException ex)
    {
      if (ex.Message.Contains("StartTime"))
      {
        return BadRequest(new { message = ex.Message });
      }

      if (ex.Message.Contains("Time conflict"))
      {
        return Conflict(new { message = ex.Message });
      }

      throw;
    }
  }

  [HttpDelete("{id:guid}")]
  public async Task<IActionResult> Delete(Guid id)
  {

    var deleted = await _reservationService.DeleteAsync(id);

    if (!deleted)
    {
      return NotFound();
    }

    return NoContent();
  }

  [HttpGet]
  public async Task<IActionResult> GetByDate([FromQuery] DateOnly? date)
  {
    if (!ModelState.IsValid)
    {
      return ValidationProblem(ModelState);
    }

    if (date is null)
    {
      return BadRequest(new { message = "Date query parameter is required" });
    }

    var reservations = await _reservationService.GetByDateAsync(date.Value);

    return Ok(reservations);
  }
}
