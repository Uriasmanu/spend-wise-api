using Microsoft.AspNetCore.Mvc;
using spend_wise_api.models;
using spend_wise_api.services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace spend_wise_api.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrosController : ControllerBase
    {
        private readonly RegistrosService _registrosService;

        public RegistrosController(RegistrosService registrosService)
        {
            _registrosService = registrosService;
        }

        // GET: api/registros
        [HttpGet]
        public async Task<IActionResult> GetRegistros()
        {
            var registros = await _registrosService.GetRegistrosAsync();
            return Ok(registros);
        }

        // GET: api/registros/{identificador}
        [HttpGet("{identificador}")]
        public async Task<IActionResult> GetRegistro(string identificador)
        {
            try
            {
                var registro = await _registrosService.GetRegistroAsync(identificador);
                return Ok(registro);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/registros
        [HttpPost]
        public async Task<IActionResult> AddRegistro([FromBody] Registros registro)
        {
            await _registrosService.AddRegistroAsync(registro);
            return CreatedAtAction(nameof(GetRegistro), new { identificador = registro.Identificador }, registro);
        }

        // PUT: api/registros/{identificador}
        [HttpPut("{identificador}")]
        public async Task<IActionResult> UpdateRegistro(string identificador, [FromBody] Registros updatedRegistro)
        {
            try
            {
                await _registrosService.UpdateRegistroAsync(identificador, updatedRegistro);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/registros/{identificador}
        [HttpDelete("{identificador}")]
        public async Task<IActionResult> DeleteRegistro(string identificador)
        {
            try
            {
                await _registrosService.DeleteRegistroAsync(identificador);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
