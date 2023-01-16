using DATA.Entities;

namespace DAL.Repositories.Abstract
{
    public interface IStationRepository
    {
        Task<StationRoot> GetStationsAsync();
        Task AddStationAsync(Station station);
    }
}
