using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.ModelsDTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NegociosController : ControllerBase
    {
        private readonly dbWebAPIContext _context;

        public NegociosController(dbWebAPIContext context)
        {
            _context = context;
        }

        // GET: api/Negocios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NegocioDTO>>> GetNegocios()
        {
            try
            {
                var negocios = await (from n in _context.Negocios
                                      select new NegocioDTO()
                                      {
                                          IdNegocio = n.IdNegocio,
                                          Nombre = n.Nombre,
                                          Direccion = n.Direccion,
                                          Descripcion = n.Descripcion,
                                          Lat = n.Lat,
                                          Lng = n.Lng,
                                          TiposIdTipo = n.TiposIdTipo,
                                          TiposTipo = n.TiposIdTipoNavigation.Tipo
                                      }).ToListAsync();

                return Ok(negocios);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        // GET: api/Negocios/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNegocios([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var negocios = await _context.Negocios.FindAsync(id);

            if (negocios == null)
            {
                return NotFound();
            }

            return Ok(negocios);
        }

        // PUT: api/Negocios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNegocios([FromRoute] int id, [FromBody] Negocios negocios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != negocios.IdNegocio)
            {
                return BadRequest();
            }

            _context.Entry(negocios).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NegociosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Negocios
        [HttpPost]
        public async Task<IActionResult> PostNegocios([FromBody] Negocios negocios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Negocios.Add(negocios);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNegocios", new { id = negocios.IdNegocio }, negocios);
        }

        // DELETE: api/Negocios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNegocios([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var negocios = await _context.Negocios.FindAsync(id);
            if (negocios == null)
            {
                return NotFound();
            }

            _context.Negocios.Remove(negocios);
            await _context.SaveChangesAsync();

            return Ok(negocios);
        }

        private bool NegociosExists(int id)
        {
            return _context.Negocios.Any(e => e.IdNegocio == id);
        }
    }
}