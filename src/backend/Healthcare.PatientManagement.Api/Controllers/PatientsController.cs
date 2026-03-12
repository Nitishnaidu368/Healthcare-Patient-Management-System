using Healthcare.PatientManagement.Api.DTOs;
using Healthcare.PatientManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Healthcare.PatientManagement.Api.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;
    private readonly IAppointmentService _appointmentService;

    public PatientsController(IPatientService patientService, IAppointmentService appointmentService)
    {
        _patientService = patientService;
        _appointmentService = appointmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<PatientResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var patients = await _patientService.GetAllAsync(cancellationToken);
        return Ok(patients);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PatientResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var patient = await _patientService.GetByIdAsync(id, cancellationToken);
        if (patient is null)
        {
            return NotFound();
        }

        return Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<PatientResponse>> Create([FromBody] CreatePatientRequest request, CancellationToken cancellationToken)
    {
        var patient = await _patientService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PatientResponse>> Update(Guid id, [FromBody] UpdatePatientRequest request, CancellationToken cancellationToken)
    {
        var updated = await _patientService.UpdateAsync(id, request, cancellationToken);
        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _patientService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("{id:guid}/treatment-plans")]
    public async Task<ActionResult<IReadOnlyCollection<TreatmentPlanResponse>>> GetTreatmentPlans(Guid id, CancellationToken cancellationToken)
    {
        var plans = await _appointmentService.GetTreatmentPlansAsync(id, cancellationToken);
        return Ok(plans);
    }

    [HttpPost("{id:guid}/treatment-plans")]
    public async Task<ActionResult<TreatmentPlanResponse>> AddTreatmentPlan(Guid id, [FromBody] CreateTreatmentPlanRequest request, CancellationToken cancellationToken)
    {
        if (request.PatientId != id)
        {
            return BadRequest("Payload patient ID must match route ID.");
        }

        var plan = await _appointmentService.AddTreatmentPlanAsync(request, cancellationToken);
        return Ok(plan);
    }
}
