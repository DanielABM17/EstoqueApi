using LoginApi.Entities;
using LoginApi.Repositorys.Contracts;
using MongoDB.Driver;

namespace LoginApi.Repositorys
{
    public class LensRepository : ILensRepository
    {
        private readonly IMongoCollection<Lens> _lens;
        public LensRepository(IMongoDatabase database)
        {
            _lens = database.GetCollection<Lens>("Lens");
        }
        public Task AddLensAsync(Lens lens)
        {
            return _lens.InsertOneAsync(lens);
        }

        public async Task DeleteLensAsync(double sph, double cyl)
        {
            await _lens.DeleteOneAsync(l => l.Sphere == sph && l.Cylinder == cyl);
        }

        public async Task<List<Lens>> GetAllLensAsync()
        {
            return await _lens.FindAsync(_ => true).Result.ToListAsync();
        }

        public async Task<Lens> GetLensByDioptryAsync(double sph, double cyl)
        {
            return await _lens.FindAsync(l => l.Sphere == sph && l.Cylinder == cyl).Result.FirstOrDefaultAsync();
        }

        public async Task UpdateLensAsync(Lens lens)
        {
            var lensFinded = await _lens.FindAsync(l => l.Sphere == lens.Sphere && l.Cylinder == lens.Cylinder).Result.FirstOrDefaultAsync();

            if (lensFinded == null || lensFinded.Quantidade == 0 || lensFinded.version != lens.version)
            {
                throw new Exception("A lente foi modificada antes de confirmar sua operação, por favor recarregar a pagina.");
            }

            _lens.ReplaceOne(l => l.Sphere == lens.Sphere && l.Cylinder == lens.Cylinder, lens);
        }
    }
}
