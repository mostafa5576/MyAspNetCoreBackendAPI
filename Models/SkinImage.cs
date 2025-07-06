using System.ComponentModel.DataAnnotations;

namespace SkinDiseaseAPI.Models
{
    public class SkinImage
    {
        [Key]
        public long ImageID { get; set; }
        public string UserID { get; set; } = string.Empty; // غيرت من long لـ string
        public string ImageURL { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public string? DeviceType { get; set; }
        public string? ImageMetadata { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public List<Diagnosis> Diagnoses { get; set; } = new();
    }
}