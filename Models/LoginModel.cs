using System.ComponentModel.DataAnnotations;

namespace SkinDiseaseAPI.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress] // للتأكد إن الإيميل صحيح
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";
    }
}
