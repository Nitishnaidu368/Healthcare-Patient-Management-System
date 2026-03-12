namespace Healthcare.PatientManagement.Api.DTOs;

public record CreateAppointmentRequest(
    Guid PatientId,
    string DoctorName,
    DateTime StartAtUtc,
    DateTime EndAtUtc,
    string Reason);

public record AppointmentResponse(
    Guid Id,
    Guid PatientId,
    string DoctorName,
    DateTime StartAtUtc,
    DateTime EndAtUtc,
    string Reason,
    string Status);
