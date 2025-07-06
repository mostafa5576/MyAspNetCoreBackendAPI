using System.ComponentModel.DataAnnotations;

namespace SkinDiseaseAPI.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress] // للتأكد إن الإيميل صحيح
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        [Required]
        public string FullName { get; set; } = "";

        [Required]
        public int Age { get; set; }

        [Required]
        public string Gender { get; set; } = "";
    }
}