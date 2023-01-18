using DATA.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Models.ViewModel;
using System.Linq;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index(HomeIndexViewModel model)
        {
            model.Bikes = await GetBikeList();

            if (model.ChartModel == null) model.ChartModel = await GetStationDensity();

            if (model.Search != null)
            {
                model.Bikes = model.Bikes.Where(b => b.Name.ToLower().Contains(model.Search.Trim().ToLower())
                            || (b.StationName != null ? b.StationName.ToLower().Contains(model.Search.Trim().ToLower()) : false)
                            ||(b.StationAddress != null ? b.StationAddress.ToLower().Contains(model.Search.Trim().ToLower()) : false)).ToList();
            }
            return View(model);
        }
        
        public async Task<JsonResult> GetSearchResults(string search)
        {
            var bikeList = await GetBikeList();
            bikeList.Where(b => b.Name.ToLower().Contains(search.ToLower())
                            || b.StationName.ToLower().Contains(search.ToLower())
                            || b.StationAddress.ToLower().Contains(search.ToLower()));
            return new JsonResult(bikeList);
        }

        public async Task<List<StationDensityViewModel>> GetStationDensity()
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
            return model.Where(m => m.Y > 0).ToList();
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

        private async Task<List<BikeViewModel>> GetBikeList()
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