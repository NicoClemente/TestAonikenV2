using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestAoniken.Models;
using TestAoniken.Servicios;
using TestAoniken.Errors;

namespace TestAoniken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicacionController : ControllerBase
    {
        private readonly IPublicacionService _publicacionService;

        public PublicacionController(IPublicacionService publicacionService)
        {
            _publicacionService = publicacionService;
        }

        [HttpGet("pendientes")]
        public async Task<IActionResult> ObtenerPublicacionesPendientes()
        {
            var resultado = await _publicacionService.ObtenerPublicacionesPendientesAsync();
            if (!resultado.IsSuccess)
            {
                return StatusCode(500, resultado.Error?.Message);
            }
            var publicaciones = resultado.Value;
            if (publicaciones == null || publicaciones.Count == 0)
            {
                return NotFound();
            }
            return Ok(publicaciones);
        }

        [HttpPut("aprobar/{id}")]
        public async Task<IActionResult> AprobarPublicacion(int id)
        {
            var resultado = await _publicacionService.AprobarPublicacionAsync(id);
            if (resultado.IsSuccess)
            {
                return Ok(true);
            }
            return BadRequest(resultado.Error?.Message);
        }

        [HttpDelete("rechazar/{id}")]
        public async Task<IActionResult> RechazarPublicacion(int id)
        {
            var resultado = await _publicacionService.RechazarPublicacionAsync(id);
            if (resultado.IsSuccess)
            {
                return Ok(true);
            }
            return BadRequest(resultado.Error?.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPublicacion(int id, [FromBody] Publicacion publicacionActualizada)
        {
            var resultado = await _publicacionService.ActualizarPublicacionAsync(id, publicacionActualizada);
            if (resultado.IsSuccess)
            {
                return Ok(resultado.Value);
            }
            return NotFound(resultado.Error?.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPublicacion(int id)
        {
            var resultado = await _publicacionService.EliminarPublicacionAsync(id);
            if (resultado.IsSuccess)
            {
                return Ok();
            }
            return NotFound(resultado.Error?.Message);
        }
    }
}