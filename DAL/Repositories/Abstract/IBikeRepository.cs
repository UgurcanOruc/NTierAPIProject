using DATA.Entities;

namespace DAL.Repositories.Abstract
{
    public interface IBikeRepository
    {
        Task<BikeRoot> GetBikesAsync();
        Task AddBikeAsync(Bike bike);
    }
}
