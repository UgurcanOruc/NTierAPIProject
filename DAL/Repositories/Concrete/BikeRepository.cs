using DAL.Repositories.Abstract;
using DATA.Entities;
using System.Text.Json;
using System;
using System.Runtime.CompilerServices;

namespace DAL.Repositories.Concrete
{
    public class BikeRepository : IBikeRepository
    {
        public async Task AddBikeAsync(Bike bike)
        {
            var bikeRoot = await GetBikesInnerAsync();
            bikeRoot.Data.Bikes.Add(bike);
            using (StreamWriter w = new StreamWriter("C:\\Users\\ugurc\\UgurcanOruc\\NTierAPIProject\\DAL\\Data\\bike.json"))
            {
                string json = JsonSerializer.Serialize(bikeRoot);
                await w.WriteAsync(json);
            }
        }

        public async Task<List<Bike>> GetBikesAsync()
        {
            var root = await GetBikesInnerAsync();
            return root.Data.Bikes;
        }

        private async Task<BikeRoot> GetBikesInnerAsync()
        {
            BikeRoot bikeRoot = new();
            using (StreamReader r = new StreamReader("C:\\Users\\ugurc\\UgurcanOruc\\NTierAPIProject\\DAL\\Data\\bike.json"))
            {
                string json = await r.ReadToEndAsync();
                bikeRoot = JsonSerializer.Deserialize<BikeRoot>(json) ?? new();
            }
            return bikeRoot;
        }
    }
}
