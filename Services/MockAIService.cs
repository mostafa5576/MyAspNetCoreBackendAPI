namespace SkinDiseaseAPI.Services
{
    public class MockAIService
    {
        public string PredictDisease(IFormFile image)
        {
            //محاكاة لنتيجة ال AI
            string[] diseases = { "Eczema", "Psoriasis", "Acne" };
            Random random = new Random();
            return diseases[random.Next(diseases.Length)];
        }

    }
}
