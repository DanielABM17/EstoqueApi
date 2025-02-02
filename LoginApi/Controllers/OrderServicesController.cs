using EstoqueApi.Entities.Enums;
using LoginApi.Entities;
using LoginApi.Repositorys.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace LoginApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderServicesController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderServicesController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{serviceNumber}")]
        public async Task<IActionResult> GetOrderService(int serviceNumber)
        {
            var orderService = await _orderService.GetOrderServiceAsync(serviceNumber);
            return Ok(orderService);
        }
        [Authorize(Roles = "Admin,Manager,Seller")]
        [HttpPost]
        public async Task<IActionResult> AddOrder(OrderService orderService)
        {if(!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos");
            }

        if(!EnumExtension.ValidateLocalStatus(orderService.Status, orderService.Local))
            {
                return BadRequest("Status inválido");
            }
            await _orderService.AddOrderServiceAsync(orderService);
            return Ok();    
        }
        [Authorize(Roles = "Admin,Manager,Seller")]
        [HttpGet("/getall/{storeCode}")]
        public async Task<IActionResult> GetAllOrders(string storeCode)
        {

            var orderList= await _orderService.GetOrderServicesAsync(storeCode);
            if (orderList == null)
            {
                return NotFound("Nenhuma ordem encontrada");
            }
            return Ok(orderList);
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpPut]
        public async Task<IActionResult> UpdateOrder(OrderService orderService)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos");
            }
            await _orderService.UpdateOrderServiceAsync(orderService);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("/delete/{serviceNumber}")]
        public async Task<IActionResult> DeleteOrder(int serviceNumber)
        {
            if(serviceNumber == 0)
            {
                return base.BadRequest("Número de serviço inválido");
            }
            await _orderService.DeleteOrderServiceAsync(serviceNumber);
            return Ok();
        }
        [Authorize(Roles = "Admin,Manager,Seller")]
        [HttpGet("/status")]
        public async Task<IActionResult> GetOrderServiceByStatus(string storeCode, string status)
        {
            var orderList = await _orderService.GetOrderServiceByStatusAsync(storeCode, status);
            if (orderList == null)
            {
                return NotFound("Nenhuma ordem encontrada");
            }
            return Ok(orderList);
        }

        [Authorize(Roles = "Admin,Manager,Seller")]
        [HttpPatch("/status")]
        public async Task UpdateStatus(int serviceNumber,string storeCode, string status)
        {
            var orderService = await _orderService.GetOrderServiceAsync(serviceNumber);
            orderService.Status = (Status)Enum.Parse(typeof(Status), status);
            orderService.UpdatedDate = DateTime.Now;    
            await _orderService.UpdateOrderServiceAsync(orderService);
        }


    }
}
