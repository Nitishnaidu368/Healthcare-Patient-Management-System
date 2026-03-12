using Healthcare.PatientManagement.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.PatientManagement.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<TreatmentPlan> TreatmentPlans => Set<TreatmentPlan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(200).IsRequired();
            entity.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Reason).HasMaxLength(500).IsRequired();
            entity.HasOne(x => x.Patient)
                .WithMany(x => x.Appointments)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(x => new { x.DoctorName, x.StartAtUtc, x.EndAtUtc });
        });

        modelBuilder.Entity<TreatmentPlan>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Diagnosis).HasMaxLength(300).IsRequired();
            entity.Property(x => x.CarePlan).HasMaxLength(2000).IsRequired();
            entity.HasOne(x => x.Patient)
                .WithMany(x => x.TreatmentPlans)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}
