using DAL.Repositories.Abstract;
using DATA.Entities;
using System.Text.Json;

namespace DAL.Repositories.Concrete
{
    public class StationRepository : IStationRepository
    {
        public async Task AddStationAsync(Station station)
        {
            var stationRoot = await GetStationsAsync();
            stationRoot.Data.Stations.Add(station);
            using (StreamWriter w = new StreamWriter("C:\\WebApi\\DAL\\Data\\station.json"))
            {
                string json = JsonSerializer.Serialize(stationRoot);
                await w.WriteAsync(json);
            }
        }

        public async Task<StationRoot> GetStationsAsync()
        {
            StationRoot stationRoot = new();
            using (StreamReader r = new StreamReader("C:\\WebApi\\DAL\\Data\\station.json"))
            {
                string json = await r.ReadToEndAsync();
                stationRoot = JsonSerializer.Deserialize<StationRoot>(json) ?? new();
            }
            return stationRoot;
        }
    }
}
