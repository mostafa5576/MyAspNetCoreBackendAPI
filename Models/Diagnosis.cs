using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkinDiseaseAPI.Models
{
    public class Diagnosis
    {
        [Key]
        public long DiagnosisID { get; set; } // غيرت من int لـ long عشان يتطابق مع الـ Schema

        [ForeignKey("SkinImage")]
        public long ImageID { get; set; }
        public SkinImage? SkinImage { get; set; }

        [ForeignKey("SkinDisease")]
        public int DiseaseID { get; set; }
        public SkinDisease? SkinDisease { get; set; }

        public decimal ConfidenceScore { get; set; }
        public DateTime DiagnosisDate { get; set; }

        // ✅ أضف العلاقة مع DoctorReview
        public ICollection<DoctorReview> DoctorReviews { get; set; } = new List<DoctorReview>();
    }
}