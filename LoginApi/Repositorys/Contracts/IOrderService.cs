using LoginApi.Entities;

namespace LoginApi.Repositorys.Contracts
{
    public interface IOrderService
    {
        Task AddOrderServiceAsync(OrderService orderService);
        Task DeleteOrderServiceAsync(int serviceNumber);
        Task<OrderService> GetOrderServiceAsync(int serviceNumber);
        Task<IEnumerable<OrderService>> GetOrderServicesAsync(string storeCode);
        Task UpdateOrderServiceAsync(OrderService orderService);

        Task<ICollection<OrderService>> GetOrderServiceByStatusAsync(string storeCode, string status);
    }
}
