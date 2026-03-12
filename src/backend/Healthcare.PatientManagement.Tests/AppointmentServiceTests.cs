using Healthcare.PatientManagement.Api.Data;
using Healthcare.PatientManagement.Api.Domain;
using Healthcare.PatientManagement.Api.DTOs;
using Healthcare.PatientManagement.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.PatientManagement.Tests;

public class AppointmentServiceTests
{
    private static AppDbContext BuildDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task ScheduleAsync_ShouldRejectOverlappingDoctorAppointment()
    {
        await using var dbContext = BuildDbContext();
        var patientId = Guid.NewGuid();

        dbContext.Patients.Add(new Patient
        {
            Id = patientId,
            FirstName = "Sam",
            LastName = "Lee",
            DateOfBirth = new DateOnly(1988, 8, 8),
            Email = "sam@example.com",
            PhoneNumber = "555-1000",
            Gender = "Female"
        });

        dbContext.Appointments.Add(new Appointment
        {
            PatientId = patientId,
            DoctorName = "Dr. Patel",
            StartAtUtc = new DateTime(2026, 1, 5, 9, 0, 0, DateTimeKind.Utc),
            EndAtUtc = new DateTime(2026, 1, 5, 10, 0, 0, DateTimeKind.Utc),
            Reason = "Follow-up"
        });

        await dbContext.SaveChangesAsync();
        var service = new AppointmentService(dbContext);

        var request = new CreateAppointmentRequest(
            patientId,
            "Dr. Patel",
            new DateTime(2026, 1, 5, 9, 30, 0, DateTimeKind.Utc),
            new DateTime(2026, 1, 5, 10, 30, 0, DateTimeKind.Utc),
            "Consultation");

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ScheduleAsync(request, CancellationToken.None));
    }
}
