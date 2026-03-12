namespace Healthcare.PatientManagement.Api.Domain;

public class TreatmentPlan
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PatientId { get; set; }
    public Patient? Patient { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string CarePlan { get; set; } = string.Empty;
    public DateOnly ReviewDate { get; set; }
    public bool IsActive { get; set; } = true;
}
