using DAL.Repositories.Abstract;
using DATA.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IStationRepository _stationRepo;

        public StationController(IStationRepository stationRepo)
        {
            _stationRepo = stationRepo;
        }

        [HttpGet]
        public async Task<ActionResult> GetStations()
        {
            var model = await _stationRepo.GetStationsAsync();
            var result = JsonSerializer.Serialize(model);
            return Ok(result);
        }
    }
}
