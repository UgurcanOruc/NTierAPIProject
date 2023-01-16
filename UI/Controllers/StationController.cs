using DATA.Entities;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class StationController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var stations = new StationRoot();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7233/api/");
                var result = await client.GetAsync("Station");

                if (result.IsSuccessStatusCode)
                {
                    stations = await result.Content.ReadFromJsonAsync<StationRoot>();
                }
            }

            return View();
        }
    }
}
