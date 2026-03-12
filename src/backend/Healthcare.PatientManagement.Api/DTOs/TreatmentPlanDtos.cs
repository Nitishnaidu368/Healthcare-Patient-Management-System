namespace Healthcare.PatientManagement.Api.DTOs;

public record CreateTreatmentPlanRequest(
    Guid PatientId,
    string Diagnosis,
    string CarePlan,
    DateOnly ReviewDate);

public record TreatmentPlanResponse(
    Guid Id,
    Guid PatientId,
    string Diagnosis,
    string CarePlan,
    DateOnly ReviewDate,
    bool IsActive);
