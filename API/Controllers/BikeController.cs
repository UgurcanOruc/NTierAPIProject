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
        public async Task<ActionResult> GetBikes()
        {
            var model = await _bikeRepo.GetBikesAsync();
            var result = JsonSerializer.Serialize(model);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddBike(Bike bike)
        {
            await _bikeRepo.AddBikeAsync(bike);
            return Ok();
        }
    }
}
