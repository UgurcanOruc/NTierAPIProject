using DAL.Repositories.Abstract;
using DATA.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikeController : ControllerBase
    {
        private readonly IBikeRepository _bikeRepo;

        public BikeController(IBikeRepository bikeRepo)
        {
            _bikeRepo = bikeRepo;
        }

        [HttpGet]
        public async Task<JsonResult> GetBikes()
        {
            var model = await _bikeRepo.GetBikesAsync();
            var result = JsonSerializer.Serialize(model);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<OkResult> AddBike(Bike bike)
        {
            await _bikeRepo.AddBikeAsync(bike);
            return Ok();
        }
    }
}
