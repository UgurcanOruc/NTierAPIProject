using DAL.Repositories.Abstract;
using DATA.Entities;
using System.Text.Json;
using System;
using System.Runtime.CompilerServices;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace DAL.Repositories.Concrete
{
    public class BikeRepository : GenericRepository, IBikeRepository
    {
        public async Task AddBikeAsync(Bike bike)
        {
            var bikeRoot = await ReadDataFromJsonFile<BikeRoot>("bike");
            bikeRoot.Data.Bikes.Add(bike);
            using (StreamWriter w = new StreamWriter(Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\DAL\\Data\\bike.json"))
            {
                string json = JsonSerializer.Serialize(bikeRoot);
                await w.WriteAsync(json);
            }
        }

        public async Task<List<Bike>> GetBikesAsync()
        {
            var root = await ReadDataFromJsonFile<BikeRoot>("bike");
            return root.Data.Bikes;
        }
    }
}
