using DAL.Repositories.Abstract;
using DATA.Entities;

namespace DAL.Repositories.Concrete
{
    public class StationRepository :GenericRepository, IStationRepository
    {
        public async Task<List<Station>> GetStationsAsync()
        {
            var root = await ReadDataFromJsonFile<StationRoot>("station");
            return root.Data.Stations;
        }
    }
}
