using LoginApi.Entities;

namespace LoginApi.Repositorys.Contracts
{
    public interface IStoreRepository
    {
        Task<Store> GetStoreByCodeAsync(string code);
        Task AddStoreAsync(Store store);
        Task UpdateStoreAsync(Store store);

        Task<List<Store>> GetAllStoresAsync();

        Task DeleteStoreAsync(string storeCode);
    }
}
