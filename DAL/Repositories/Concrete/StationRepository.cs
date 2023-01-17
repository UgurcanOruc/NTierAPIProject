using DAL.Repositories.Abstract;
using DATA.Entities;
using System.Text.Json;

namespace DAL.Repositories.Concrete
{
    public class StationRepository : IStationRepository
    {
        public async Task<List<Station>> GetStationsAsync()
        {
            var root = await GetStationsInnerAsync();
            return root.Data.Stations;
        }

        public async Task<StationRoot> GetStationsInnerAsync()
        {
            StationRoot stationRoot = new();
            using (StreamReader r = new StreamReader("C:\\Users\\ugurc\\UgurcanOruc\\NTierAPIProject\\DAL\\Data\\station.json"))
            {
                string json = await r.ReadToEndAsync();
                stationRoot = JsonSerializer.Deserialize<StationRoot>(json) ?? new();
            }
            return stationRoot;
        }
    }
}
