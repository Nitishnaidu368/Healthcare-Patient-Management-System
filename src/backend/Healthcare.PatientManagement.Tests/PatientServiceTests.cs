using Healthcare.PatientManagement.Api.Data;
using Healthcare.PatientManagement.Api.DTOs;
using Healthcare.PatientManagement.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.PatientManagement.Tests;

public class PatientServiceTests
{
    private static AppDbContext BuildDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldPersistPatient()
    {
        await using var dbContext = BuildDbContext();
        var service = new PatientService(dbContext);

        var request = new CreatePatientRequest(
            "Alex",
            "Morgan",
            new DateOnly(1990, 3, 10),
            "alex@example.com",
            "+1-555-0000",
            "Non-binary");

        var created = await service.CreateAsync(request, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal("alex@example.com", created.Email);
        Assert.Single(dbContext.Patients);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenPatientDoesNotExist()
    {
        await using var dbContext = BuildDbContext();
        var service = new PatientService(dbContext);

        var updated = await service.UpdateAsync(
            Guid.NewGuid(),
            new UpdatePatientRequest(
                "A",
                "B",
                new DateOnly(2000, 1, 1),
                "nobody@example.com",
                "123",
                "Unknown"),
            CancellationToken.None);

        Assert.Null(updated);
    }
}
