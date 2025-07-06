using System.ComponentModel.DataAnnotations;

namespace SkinDiseaseAPI.Models
{
    public class DoctorReview
    {
        [Key]
        public int ReviewID { get; set; }
        public long DiagnosisID { get; set; } // غيرت من int لـ long عشان يتطابق مع DiagnosisID
        public string DoctorID { get; set; } = string.Empty;
        public string? ReviewText { get; set; }
        public int? ConfirmedDiagnosisID { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;

        public Diagnosis Diagnosis { get; set; } = null!;
        public ApplicationUser Doctor { get; set; } = null!;
        public SkinDisease? ConfirmedDiagnosis { get; set; }
    }
}