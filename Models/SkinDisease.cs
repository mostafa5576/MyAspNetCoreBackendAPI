using System.Collections.Generic;

namespace SkinDiseaseAPI.Models
{
    public class SkinDisease
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Symptoms { get; set; }
        public required string Treatment { get; set; }
        public DateTime DateDiscovered { get; set; }
        public string SeverityLevel { get; set; } = "Mild"; // "Mild", "Moderate", "Severe"

        public List<Diagnosis> Diagnoses { get; set; } = new(); // علاقة مع Diagnosis
        public List<DoctorReview> DoctorReviews { get; set; } = new(); // علاقة مع DoctorReview
    }
}