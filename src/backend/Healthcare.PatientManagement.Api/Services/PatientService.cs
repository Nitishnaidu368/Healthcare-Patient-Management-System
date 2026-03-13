using Healthcare.PatientManagement.Api.Data;
using Healthcare.PatientManagement.Api.Domain;
using Healthcare.PatientManagement.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.PatientManagement.Api.Services;

public class PatientService : IPatientService
{
    private readonly AppDbContext _dbContext;

    public PatientService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<PatientResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Patients
            .AsNoTracking()
            .OrderBy(x => x.LastName)
            .ThenBy(x => x.FirstName)
            .Select(patient => new PatientResponse(
                patient.Id,
                patient.FirstName,
                patient.LastName,
                patient.DateOfBirth,
                patient.Email,
                patient.PhoneNumber,
                patient.Gender))
            .ToListAsync(cancellationToken);
    }

    public async Task<PatientResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Patients
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(patient => new PatientResponse(
                patient.Id,
                patient.FirstName,
                patient.LastName,
                patient.DateOfBirth,
                patient.Email,
                patient.PhoneNumber,
                patient.Gender))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<PatientResponse> CreateAsync(CreatePatientRequest request, CancellationToken cancellationToken)
    {
        var patient = new Patient
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            DateOfBirth = request.DateOfBirth,
            Email = request.Email.Trim().ToLowerInvariant(),
            PhoneNumber = request.PhoneNumber.Trim(),
            Gender = request.Gender.Trim()
        };

        _dbContext.Patients.Add(patient);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Map(patient);
    }

    public async Task<PatientResponse?> UpdateAsync(Guid id, UpdatePatientRequest request, CancellationToken cancellationToken)
    {
        var patient = await _dbContext.Patients.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (patient is null)
        {
            return null;
        }

        patient.FirstName = request.FirstName.Trim();
        patient.LastName = request.LastName.Trim();
        patient.DateOfBirth = request.DateOfBirth;
        patient.Email = request.Email.Trim().ToLowerInvariant();
        patient.PhoneNumber = request.PhoneNumber.Trim();
        patient.Gender = request.Gender.Trim();
        patient.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Map(patient);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var patient = await _dbContext.Patients.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (patient is null)
        {
            return false;
        }

        _dbContext.Patients.Remove(patient);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static PatientResponse Map(Patient patient)
    {
        return new PatientResponse(
            patient.Id,
            patient.FirstName,
            patient.LastName,
            patient.DateOfBirth,
            patient.Email,
            patient.PhoneNumber,
            patient.Gender);
    }
}
