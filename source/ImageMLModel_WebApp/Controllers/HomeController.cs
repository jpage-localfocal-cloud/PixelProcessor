
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImageMLModel_ebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Predict(Microsoft.AspNetCore.Http.IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ViewBag.Prediction = "No file selected.";
                return View("Index");
            }

            var client = _httpClientFactory.CreateClient();

            using var content = new MultipartFormDataContent();
            using var stream = imageFile.OpenReadStream();
            content.Add(new StreamContent(stream), "imageFile", imageFile.FileName);
            //http://localhost:54960
            var response = await client.PostAsync("http://localhost:54960/predict", content);
            var result = await response.Content.ReadAsStringAsync();

            ViewBag.Prediction = result;
            return View("Index");
        }
    }
}