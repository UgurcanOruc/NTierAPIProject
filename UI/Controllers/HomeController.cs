using DATA.Entities;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using UI.Models;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var model = await PrepareViewModel();

            return View(model);
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
                        StationName = station.Name,
                        BikeCount = bikes.Where(b => b.StationId == station.StationId).Count()
                    }
                );
            }
            return new JsonResult(model);
        }

        private async Task<List<BikeViewModel>> PrepareViewModel()
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

                var bikesResult = await client.GetAsync("Bike");
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
            return new BikeViewModel()
            {
                BikeId = bike.BikeId,
                StationId = bike.StationId,
                Name = bike.Name,
                Longitude = bike.Longitude,
                Latitude = bike.Latitude,
                IsReversed = bike.IsReversed,
                IsDisabled = bike.IsDisabled,
                Station = stations.Where(s => s.StationId == bike.StationId).FirstOrDefault() ?? new Station(),
            };
        }
    }
}