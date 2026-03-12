namespace Healthcare.PatientManagement.Api.DTOs;

public record CreatePatientRequest(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Email,
    string PhoneNumber,
    string Gender);

public record UpdatePatientRequest(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Email,
    string PhoneNumber,
    string Gender);

public record PatientResponse(
    Guid Id,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Email,
    string PhoneNumber,
    string Gender);
