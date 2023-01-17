using DATA.Entities;

namespace DAL.Repositories.Abstract
{
    public interface IStationRepository
    {
        Task<List<Station>> GetStationsAsync();
    }
}
