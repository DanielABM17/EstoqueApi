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
    public class LensController : ControllerBase
    {
        private readonly ILensRepository _lensRepository;

        public LensController(ILensRepository lensRepository)
        {
            _lensRepository = lensRepository;
        }
        [Authorize(Roles = "Admin,Manager,Seller")]
        [HttpGet("{sph}/{cyl}")]
        public async Task<IActionResult> GetLens(double sph, double cyl)
        {
            var lens = await _lensRepository.GetLensByDioptryAsync(sph, cyl);
            if (lens == null)
            {
                return NotFound("Lente não encontrada");
            }
            return Ok(lens);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateLens([FromBody] Lens lens)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos");
            }
            try
            {
                await _lensRepository.AddLensAsync(lens);
                return Ok("Lente cadastrada com sucesso");
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                return BadRequest("Lente já cadastrada");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{sph}/{cyl}")]
        public async Task<IActionResult> DeleteLens(double sph, double cyl)
        {
            await _lensRepository.DeleteLensAsync(sph, cyl);
            return Ok("Lente deletada com sucesso");
        }
        [Authorize(Roles = "Admin,Manager,Seller")]
        [HttpPut]
        public async Task<IActionResult> UpdateLensAsync([FromBody] Lens lens)
        {
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Dados inválidos");
                }

                var lensFinded = await _lensRepository.GetLensByDioptryAsync(lens.Sphere, lens.Cylinder);
                if (lensFinded == null)
                {
                    return NotFound("Lente não encontrada");
                }
                await _lensRepository.UpdateLensAsync(lens);
                return Ok("Lente atualizada com sucesso");
            }


        }
    }
}
