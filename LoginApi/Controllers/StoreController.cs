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
    public class StoreController : ControllerBase
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IUserRepository _userRepository;


        public StoreController(IStoreRepository storeRepository, IUserRepository userRepository)
        {
            _storeRepository = storeRepository;
            _userRepository = userRepository;
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStore(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Código da loja não informado");
            }

            var store = await _storeRepository.GetStoreByCodeAsync(code);
            if (store == null)
            {
                return NotFound("Loja não encontrada");
            }
            return Ok(store);


        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateStore([FromBody] Store store)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos");
            }

            try
            {
                await _storeRepository.AddStoreAsync(store);
                return Ok("Loja cadastrada com sucesso");
            }
            catch (MongoWriteException ex) when(ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                return BadRequest("Loja já cadastrada");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllStores()
        {

            var stores = await _storeRepository.GetAllStoresAsync();
            if(stores == null)
            {
                return NotFound("Nenhuma loja encontrada");
            }

            return Ok(stores);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{storeCode}")]
        public async Task<IActionResult> DeleteStore(string storeCode)
        {
            if(string.IsNullOrEmpty(storeCode))
            {
                return BadRequest("Código da loja não informado");
            }

            var store = _storeRepository.GetStoreByCodeAsync(storeCode);
            await _storeRepository.DeleteStoreAsync(storeCode);
            return Ok("Loja deletada com sucesso");
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("{storeCode}/users")]
        public async Task<IActionResult> GetUsersByStore(string storeCode)
        {
            if(string.IsNullOrEmpty(storeCode))
            {
                return BadRequest("Código da loja não informado");
            }

            var users = await _userRepository.GetUsersByStore(storeCode);
            return Ok(users);
        }



    }
}
