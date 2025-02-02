using LoginApi.Entities;
using LoginApi.Repositorys.Contracts;
using MongoDB.Driver;

namespace LoginApi.Repositorys
{
    public class StoreRepository : IStoreRepository
    {
       private readonly IMongoCollection<Store> _stores;

        public StoreRepository(IMongoDatabase database)
        {
            _stores = database.GetCollection<Store>("Stores");
        }
        public async Task AddStoreAsync(Store store)
        {
           await _stores.InsertOneAsync(store);
        }

        public async Task DeleteStoreAsync(string storeCode)
        {
           await _stores.DeleteOneAsync(s => s.StoreCode == storeCode);
        }

        public Task<List<Store>> GetAllStoresAsync()
        {
            return _stores.FindAsync(_ => true).Result.ToListAsync();
        }
        
        public Task<Store> GetStoreByCodeAsync(string code)
        {
           return _stores.FindAsync(s=> s.StoreCode == code).Result.FirstOrDefaultAsync();
        }

     

        public async Task UpdateStoreAsync(Store store)
        {
            await _stores.ReplaceOneAsync(s => s.StoreId == store.StoreId, store);
        }
    }
}
