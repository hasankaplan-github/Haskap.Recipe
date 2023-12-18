using Haskap.DugunSalonu.Application.Contracts;
using Haskap.DugunSalonu.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Haskap.DugunSalonu.Presentation.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationController : BaseController
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService=reservationService;
    }

    [HttpPost("GetReservationsForCalendar")]
    public async Task<IActionResult> GetReservationsForCalendar(GetReservationsForCalendar_InputDto inputDto)
    {
        throw new Exception();
        var output = await _reservationService.GetReservationsForCalendar(inputDto);
        return Ok(output);
    }

    [HttpGet("{reservationId}")]
    public async Task<IActionResult> GetReservationById(Guid reservationId)
    {
        return Ok(await _reservationService.GetReservationById(reservationId));
    }

    [HttpPost("CreateTemporaryReservation")]
    public async Task<IActionResult> CreateTemporaryReservation(CreateTemporaryReservation_InputDto inputDto)
    {
        var outputDto = await _reservationService.CreateTemporaryReservation(inputDto);
        return Ok(outputDto);
    }

    [HttpPost("CreateReservation")]
    public async Task<IActionResult> CreateReservation(CreateReservation_InputDto inputDto)
    {
        var outputDto = await _reservationService.CreateReservation(inputDto);
        return Ok(outputDto);
    }

    [HttpPost("MarkAsTemporary")]
    public async Task<IActionResult> MarkAsTemporary(MarkAsTemporary_InputDto inputDto)
    {
        await _reservationService.MarkAsTemporary(inputDto);
        return Ok();
    }

    [HttpPost("MarkAsPermanent")]
    public async Task<IActionResult> MarkAsPermanent(MarkAsPermanent_InputDto inputDto)
    {
        await _reservationService.MarkAsPermanent(inputDto);
        return Ok();
    }
}
