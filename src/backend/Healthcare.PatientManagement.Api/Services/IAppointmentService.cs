using Healthcare.PatientManagement.Api.DTOs;

namespace Healthcare.PatientManagement.Api.Services;

public interface IAppointmentService
{
    Task<IReadOnlyCollection<AppointmentResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<AppointmentResponse> ScheduleAsync(CreateAppointmentRequest request, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TreatmentPlanResponse>> GetTreatmentPlansAsync(Guid patientId, CancellationToken cancellationToken);
    Task<TreatmentPlanResponse> AddTreatmentPlanAsync(CreateTreatmentPlanRequest request, CancellationToken cancellationToken);
}
