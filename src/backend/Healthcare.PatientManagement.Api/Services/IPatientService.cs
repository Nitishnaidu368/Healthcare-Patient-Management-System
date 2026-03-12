using Healthcare.PatientManagement.Api.DTOs;

namespace Healthcare.PatientManagement.Api.Services;

public interface IPatientService
{
    Task<IReadOnlyCollection<PatientResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<PatientResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PatientResponse> CreateAsync(CreatePatientRequest request, CancellationToken cancellationToken);
    Task<PatientResponse?> UpdateAsync(Guid id, UpdatePatientRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
