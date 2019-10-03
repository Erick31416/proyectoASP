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
    public class TiposController : ControllerBase
    {
        private readonly dbWebAPIContext _context;

        public TiposController(dbWebAPIContext context)
        {
            _context = context;
        }

        // GET: api/Tipos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoDTO>>> GetTipos()
        {
            try
            {
                var tipos = await (from t in _context.Tipos
                                   select new TipoDTO()
                                   {
                                       IdTipo = t.IdTipo,
                                       Tipo = t.Tipo
                                   }).ToListAsync();

                return tipos;
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        // GET: api/Tipos/Combo
        [HttpGet("Combo")]
        public async Task<ActionResult<IEnumerable<TipoComboDTO>>> GetTiposCombo()
        {
            try
            {
                var tipos = await (from t in _context.Tipos
                                   select new TipoComboDTO()
                                   {
                                       Label = t.Tipo,
                                       Value = t.IdTipo
                                   }).ToListAsync();

                return tipos;
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        // GET: api/Tipos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTipos([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tipos = await _context.Tipos.FindAsync(id);

            if (tipos == null)
            {
                return NotFound();
            }

            return Ok(tipos);
        }

        // PUT: api/Tipos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipos([FromRoute] int id, [FromBody] Tipos tipos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipos.IdTipo)
            {
                return BadRequest();
            }

            _context.Entry(tipos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TiposExists(id))
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

        // POST: api/Tipos
        [HttpPost]
        public async Task<IActionResult> PostTipos([FromBody] Tipos tipos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Tipos.Add(tipos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipos", new { id = tipos.IdTipo }, tipos);
        }

        // DELETE: api/Tipos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipos([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tipos = await _context.Tipos.FindAsync(id);
            if (tipos == null)
            {
                return NotFound();
            }

            _context.Tipos.Remove(tipos);
            await _context.SaveChangesAsync();

            return Ok(tipos);
        }

        private bool TiposExists(int id)
        {
            return _context.Tipos.Any(e => e.IdTipo == id);
        }
    }
}