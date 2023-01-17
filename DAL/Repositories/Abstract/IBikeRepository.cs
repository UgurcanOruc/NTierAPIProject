using DATA.Entities;

namespace DAL.Repositories.Abstract
{
    public interface IBikeRepository
    {
        Task<List<Bike>> GetBikesAsync();
        Task AddBikeAsync(Bike bike);
    }
}
