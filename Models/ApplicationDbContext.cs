using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkinDiseaseAPI.Models;

namespace SkinDiseaseAPI.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<SkinDisease> SkinDiseases { get; set; }
        public DbSet<SkinImage> SkinImages { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<DoctorReview> DoctorReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // تحديد Precision لـ ConfidenceScore
            modelBuilder.Entity<Diagnosis>()
                .Property(d => d.ConfidenceScore)
                .HasColumnType("decimal(5,2)"); // Precision = 5, Scale = 2

            // ضبط العلاقات
            modelBuilder.Entity<SkinImage>()
                .HasOne(si => si.User)
                .WithMany(u => u.SkinImages)
                .HasForeignKey(si => si.UserID);

            modelBuilder.Entity<Diagnosis>()
      .HasOne(d => d.SkinImage)
      .WithMany(si => si.Diagnoses)
      .HasForeignKey(d => d.ImageID);

            modelBuilder.Entity<Diagnosis>()
                .HasOne(d => d.SkinDisease)
                .WithMany(sd => sd.Diagnoses)
                .HasForeignKey(d => d.DiseaseID);


            modelBuilder.Entity<DoctorReview>()
                .HasOne(dr => dr.Diagnosis)
                .WithMany(d => d.DoctorReviews)
                .HasForeignKey(dr => dr.DiagnosisID)
                .OnDelete(DeleteBehavior.Restrict); // غيرت من Cascade لـ Restrict

            modelBuilder.Entity<DoctorReview>()
                .HasOne(dr => dr.Doctor)
                .WithMany(u => u.DoctorReviews)
                .HasForeignKey(dr => dr.DoctorID);

            modelBuilder.Entity<DoctorReview>()
                .HasOne(dr => dr.ConfirmedDiagnosis)
                .WithMany(d => d.DoctorReviews)
                .HasForeignKey(dr => dr.ConfirmedDiagnosisID);
        }
    }
}

            