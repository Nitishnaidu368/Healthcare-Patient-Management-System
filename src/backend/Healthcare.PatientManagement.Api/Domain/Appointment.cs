namespace Healthcare.PatientManagement.Api.Domain;

public class Appointment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PatientId { get; set; }
    public Patient? Patient { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public DateTime StartAtUtc { get; set; }
    public DateTime EndAtUtc { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = "Scheduled";
}
