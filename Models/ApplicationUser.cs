using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SkinDiseaseAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsVerified { get; set; } = false;
        public string UserType { get; set; } = "Patient"; // "Patient" or "Doctor"

        public List<SkinImage> SkinImages { get; set; } = new(); // تأكد إن دي موجودة

        public List<DoctorReview> DoctorReviews { get; set; } = new();
    }
}