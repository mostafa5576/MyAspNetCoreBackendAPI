using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkinDiseaseAPI.Models;
using SkinDiseaseAPI.Services;
using System.Linq;
using System.Security.Claims;

namespace SkinDiseaseAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DetectionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly MockAIService _aiService;

        public DetectionController(ApplicationDbContext context)
        {
            _context = context;
            _aiService = new MockAIService();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile image) // أزلنا [Consumes]
        {
            if (image == null || image.Length == 0)
                return BadRequest("لم يتم رفع صورة.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");

            var skinImage = new SkinImage
            {
                UserID = userId,
                ImageURL = $"http://example.com/images/{Guid.NewGuid().ToString()}.jpg",
                UploadDate = DateTime.Now
            };

            _context.SkinImages.Add(skinImage);
            await _context.SaveChangesAsync();

            return Ok(new { imageId = skinImage.ImageID, message = "Image uploaded successfully." });
        }

        [HttpPost("predict")]
        public async Task<IActionResult> Predict(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("لم يتم رفع صورة.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");

            // تخزين الصورة أولاً
            var skinImage = new SkinImage
            {
                UserID = userId,
                ImageURL = $"http://example.com/images/{Guid.NewGuid().ToString()}.jpg",
                UploadDate = DateTime.Now
            };
            _context.SkinImages.Add(skinImage);
            await _context.SaveChangesAsync();

            // التنبؤ باستخدام الـ AI
            string aiResult = _aiService.PredictDisease(image);
            var disease = _context.SkinDiseases.FirstOrDefault(d => d.Name == aiResult);

            // تخزين التشخيص مع معالجة إذا المرض مش موجود
            int? diseaseId = disease?.Id;
            if (diseaseId == null)
            {
                return BadRequest(new { message = $"Disease '{aiResult}' not found in database. Please add it first." });
            }

            var diagnosis = new Diagnosis
            {
                ImageID = skinImage.ImageID, // دلوقتي long يتطابق
                DiseaseID = diseaseId.Value,
                ConfidenceScore = 0.85m,
                DiagnosisDate = DateTime.Now
            };
            _context.Diagnoses.Add(diagnosis);
            await _context.SaveChangesAsync();

            return Ok(new { imageId = skinImage.ImageID, disease = aiResult, confidence = diagnosis.ConfidenceScore });
        }

        [HttpGet("results")]
        public IActionResult GetResults()
        {
            var results = _context.Diagnoses
                .Include(d => d.SkinImage)
                .Include(d => d.SkinDisease)
                .ToList();
            if (results.Count == 0)
                return NotFound("لا توجد نتائج تشخيص.");

            return Ok(results);
        }

        [HttpGet("disease/{id}")]
        public IActionResult GetDisease(int id)
        {
            var disease = _context.SkinDiseases.FirstOrDefault(d => d.Id == id);
            if (disease == null)
                return NotFound("المرض غير موجود.");

            return Ok(disease);
        }

        [HttpPost("add")]
        public IActionResult AddDisease(SkinDisease disease)
        {
            if (disease == null)
                return BadRequest("البيانات غير صحيحة.");

            _context.SkinDiseases.Add(disease);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetDisease), new { id = disease.Id }, disease);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSkinDisease(int id, [FromBody] SkinDisease updatedDisease)
        {
            if (id != updatedDisease.Id)
                return BadRequest("الـ ID غير متطابق.");

            var existingDisease = _context.SkinDiseases.Find(id);
            if (existingDisease == null)
                return NotFound("المرض غير موجود.");

            existingDisease.Name = updatedDisease.Name;
            existingDisease.Description = updatedDisease.Description;
            existingDisease.Symptoms = updatedDisease.Symptoms;
            existingDisease.Treatment = updatedDisease.Treatment;
            existingDisease.DateDiscovered = updatedDisease.DateDiscovered;

            _context.Entry(existingDisease).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSkinDisease(int id)
        {
            var disease = _context.SkinDiseases.Find(id);
            if (disease == null)
                return NotFound("المرض غير موجود.");

            _context.SkinDiseases.Remove(disease);
            _context.SaveChanges();

            return NoContent();
        }
    }
}