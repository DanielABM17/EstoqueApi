using LoginApi.Entities;
using LoginApi.Repositorys.Contracts;
using MongoDB.Driver;

namespace LoginApi.Repositorys
{
    public class OrderServiceRepository : IOrderService
    {
        private readonly IMongoCollection<OrderService> _orderServiceCollection;
        private readonly IMongoCollection<Lens> _lens;
        public OrderServiceRepository(IMongoDatabase database)
        {
            _orderServiceCollection = database.GetCollection<OrderService>("OrderServices");
            _lens = database.GetCollection<Lens>("Lenses");
        }
        public async Task AddOrderServiceAsync(OrderService orderService)
        {
            using var session = await _orderServiceCollection.Database.Client.StartSessionAsync();
            session.StartTransaction();

            try
            {
                if (orderService.LeftLens != null)
                {
                    var leftLens = await _lens.Find(l => l.Sphere == orderService.LeftLens.Sphere && l.Cylinder == orderService.LeftLens.Cylinder).FirstOrDefaultAsync();
                    
                    if(leftLens == null || leftLens.Quantidade<=0 || leftLens.version!= orderService.LeftLens.version)
                    {
                        throw new Exception("Lente não encontrada");
                    }
                    leftLens.Quantidade -= 1;

                    _lens.ReplaceOne(l => l.Sphere == leftLens.Sphere && l.Cylinder == leftLens.Cylinder, leftLens);

                }

                if (orderService.RightLens != null)
                {

                    var rightLens = await _lens.Find(l => l.Sphere == orderService.RightLens.Sphere && l.Cylinder == orderService.RightLens.Cylinder).FirstOrDefaultAsync();
                   
                    if(rightLens == null || rightLens.Quantidade <= 0 || rightLens.version != orderService.RightLens.version)
                    {
                        throw new Exception("Lente não encontrada");
                    }

                    rightLens.Quantidade -= 1;
                    _lens.ReplaceOne(l => l.Sphere == rightLens.Sphere && l.Cylinder == rightLens.Cylinder, rightLens);
                }
                await _orderServiceCollection.InsertOneAsync(orderService);
                await session.CommitTransactionAsync();
            }
            catch
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }

        public async Task DeleteOrderServiceAsync(int serviceNumber)
        {
            await _orderServiceCollection.DeleteOneAsync(o => o.OrderNumber == serviceNumber);
        }

        public async Task<OrderService> GetOrderServiceAsync(int serviceNumber)
        {
            if (serviceNumber == 0)
            {
                return null;
            }
            return await _orderServiceCollection.Find(o => o.OrderNumber == serviceNumber).FirstOrDefaultAsync();
        }

        public async Task<ICollection<OrderService>> GetOrderServiceByStatusAsync(string storeCode, string status)
        {
            var filter = Builders<OrderService>.Filter.Eq(o => o.StoreCode, storeCode) & Builders<OrderService>.Filter.Eq(o => o.Status.ToString(), status);
            return await _orderServiceCollection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<OrderService>> GetOrderServicesAsync(string storeCode)
        {
            var filter = Builders<OrderService>.Filter.Eq(o => o.StoreCode, storeCode);
            return await _orderServiceCollection.Find(filter).ToListAsync();
        }

        public async Task UpdateOrderServiceAsync(OrderService orderService)
        {   
            await _orderServiceCollection.ReplaceOneAsync(o => o.OrderNumber == orderService.OrderNumber, orderService);
        }
    }
}
