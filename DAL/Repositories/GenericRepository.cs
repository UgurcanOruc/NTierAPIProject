using DATA.Entities;
using System.Text.Json;

namespace DAL.Repositories
{
    public class GenericRepository
    {
        public async Task<T> ReadDataFromJsonFile<T>(string fileName)
        {
            T root = Activator.CreateInstance<T>();
            using (StreamReader r = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\DAL\\Data\\" + fileName + ".json"))
            {
                string json = await r.ReadToEndAsync();
                root = JsonSerializer.Deserialize<T>(json) ?? Activator.CreateInstance<T>();
            }
            return root;
        }
    }
}
