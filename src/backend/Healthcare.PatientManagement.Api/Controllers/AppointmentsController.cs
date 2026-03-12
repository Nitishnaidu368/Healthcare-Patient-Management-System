using Healthcare.PatientManagement.Api.DTOs;
using Healthcare.PatientManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Healthcare.PatientManagement.Api.Controllers;

[ApiController]
[Route("api/appointments")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<AppointmentResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var appointments = await _appointmentService.GetAllAsync(cancellationToken);
        return Ok(appointments);
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentResponse>> Create([FromBody] CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var appointment = await _appointmentService.ScheduleAsync(request, cancellationToken);
            return Ok(appointment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
