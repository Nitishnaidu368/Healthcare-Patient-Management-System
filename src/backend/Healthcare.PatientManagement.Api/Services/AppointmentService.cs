using Healthcare.PatientManagement.Api.Data;
using Healthcare.PatientManagement.Api.Domain;
using Healthcare.PatientManagement.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.PatientManagement.Api.Services;

public class AppointmentService : IAppointmentService
{
    private readonly AppDbContext _dbContext;

    public AppointmentService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<AppointmentResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Appointments
            .AsNoTracking()
            .OrderBy(x => x.StartAtUtc)
            .Select(x => new AppointmentResponse(
                x.Id,
                x.PatientId,
                x.DoctorName,
                x.StartAtUtc,
                x.EndAtUtc,
                x.Reason,
                x.Status))
            .ToListAsync(cancellationToken);
    }

    public async Task<AppointmentResponse> ScheduleAsync(CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        if (request.EndAtUtc <= request.StartAtUtc)
        {
            throw new InvalidOperationException("Appointment end time must be after start time.");
        }

        var patientExists = await _dbContext.Patients.AnyAsync(x => x.Id == request.PatientId, cancellationToken);
        if (!patientExists)
        {
            throw new KeyNotFoundException("Patient not found.");
        }

        var hasDoctorConflict = await _dbContext.Appointments.AnyAsync(x =>
            x.DoctorName == request.DoctorName &&
            x.StartAtUtc < request.EndAtUtc &&
            request.StartAtUtc < x.EndAtUtc,
            cancellationToken);

        if (hasDoctorConflict)
        {
            throw new InvalidOperationException("Doctor already has an overlapping appointment.");
        }

        var appointment = new Appointment
        {
            PatientId = request.PatientId,
            DoctorName = request.DoctorName.Trim(),
            StartAtUtc = request.StartAtUtc,
            EndAtUtc = request.EndAtUtc,
            Reason = request.Reason.Trim()
        };

        _dbContext.Appointments.Add(appointment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new AppointmentResponse(
            appointment.Id,
            appointment.PatientId,
            appointment.DoctorName,
            appointment.StartAtUtc,
            appointment.EndAtUtc,
            appointment.Reason,
            appointment.Status);
    }

    public async Task<IReadOnlyCollection<TreatmentPlanResponse>> GetTreatmentPlansAsync(Guid patientId, CancellationToken cancellationToken)
    {
        return await _dbContext.TreatmentPlans
            .AsNoTracking()
            .Where(x => x.PatientId == patientId)
            .OrderByDescending(x => x.IsActive)
            .ThenBy(x => x.ReviewDate)
            .Select(x => new TreatmentPlanResponse(
                x.Id,
                x.PatientId,
                x.Diagnosis,
                x.CarePlan,
                x.ReviewDate,
                x.IsActive))
            .ToListAsync(cancellationToken);
    }

    public async Task<TreatmentPlanResponse> AddTreatmentPlanAsync(CreateTreatmentPlanRequest request, CancellationToken cancellationToken)
    {
        var patientExists = await _dbContext.Patients.AnyAsync(x => x.Id == request.PatientId, cancellationToken);
        if (!patientExists)
        {
            throw new KeyNotFoundException("Patient not found.");
        }

        var treatmentPlan = new TreatmentPlan
        {
            PatientId = request.PatientId,
            Diagnosis = request.Diagnosis.Trim(),
            CarePlan = request.CarePlan.Trim(),
            ReviewDate = request.ReviewDate
        };

        _dbContext.TreatmentPlans.Add(treatmentPlan);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new TreatmentPlanResponse(
            treatmentPlan.Id,
            treatmentPlan.PatientId,
            treatmentPlan.Diagnosis,
            treatmentPlan.CarePlan,
            treatmentPlan.ReviewDate,
            treatmentPlan.IsActive);
    }
}
