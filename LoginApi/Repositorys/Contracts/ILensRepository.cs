using LoginApi.Entities;

namespace LoginApi.Repositorys.Contracts
{
    public interface ILensRepository
    {
        Task<Lens> GetLensByDioptryAsync(double sph, double cyl);
        Task AddLensAsync(Lens lens);
        Task UpdateLensAsync(Lens lens);
        Task<List<Lens>> GetAllLensAsync();
        Task DeleteLensAsync(double sph, double cyl);
    }
}
