using DATA.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Models.ViewModel;
using System.Linq;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        public async Task<JsonResult> GetBikeList()
        {
            return new JsonResult(await GetBikeListAsync());
        }

        public async Task<JsonResult> GetStationDensity()
        {
            var stations = await GetStationsFromAPI();
            var bikes = await GetBikesFromAPI();
            var model = new List<StationDensityViewModel>();
            foreach (var station in stations)
            {
                model.Add(
                    new StationDensityViewModel()
                    {
                        Name = station.Name,
                        Y = bikes.Where(b => b.StationId == station.StationId).Count()
                    }
                );
            }
            return new JsonResult(model.Where(m => m.Y > 0).ToList());
        }

        [HttpGet]
        public async Task<ActionResult> CreateBike()
        {
            ViewBag.StationSelectList = await GetStationSelectList();
            return View(new Bike());
        }

        [HttpPost]
        public async Task<ActionResult> CreateBike(Bike model)
        {
            if (ModelState.IsValid)
            {
                var client = new HttpClient();
                var x = await client.PostAsJsonAsync("https://localhost:7233/api/Bike/AddBike", model);
                return RedirectToAction("Index");
            }
            ViewBag.StationSelectList = await GetStationSelectList();
            return View(model);
        }

        public async Task<List<SelectListItem>> GetStationSelectList()
        {
            var stations = await GetStationsFromAPI();
            var selectList = new List<SelectListItem>();
            foreach (var station in stations.Distinct())
            {
                selectList.Add(new SelectListItem() { Text = station.Name, Value = station.StationId });
            }

            return selectList;
        }

        private async Task<List<BikeViewModel>> GetBikeListAsync()
        {
            var bikes = await GetBikesFromAPI();
            var stations = await GetStationsFromAPI();

            var viewModel = new List<BikeViewModel>();

            if (bikes != null)
            {
                foreach (var bike in bikes.Where(b => b.IsDisabled == 0).ToList())
                {
                    viewModel.Add(GetBikeViewModel(bike, stations));
                }
            }

            return viewModel;
        }

        private async Task<List<Bike>> GetBikesFromAPI()
        {
            var bikes = new List<Bike>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7233/api/");

                var bikesResult = await client.GetAsync("Bike/GetBikes");
                if (bikesResult.IsSuccessStatusCode)
                {
                    bikes = await bikesResult.Content.ReadFromJsonAsync<List<Bike>>();
                }
            }
            return bikes ?? new List<Bike>();
        }

        private async Task<List<Station>> GetStationsFromAPI()
        {
            var stations = new List<Station>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7233/api/");

                var bikesResult = await client.GetAsync("Station");
                if (bikesResult.IsSuccessStatusCode)
                {
                    stations = await bikesResult.Content.ReadFromJsonAsync<List<Station>>();
                }
            }
            return stations ?? new List<Station>();
        }

        private BikeViewModel GetBikeViewModel(Bike bike, List<Station> stations)
        {
            var station = stations.Where(s => s.StationId == bike.StationId).FirstOrDefault() ?? new Station();
            return new BikeViewModel()
            {
                Name = bike?.Name,
                StationName = station?.Name,
                StationAddress = station?.Address
            };
        }
    }
}