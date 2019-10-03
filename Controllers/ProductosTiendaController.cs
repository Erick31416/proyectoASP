using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosTiendaController : ControllerBase
    {
        //private readonly dbWebAPIContext _context;
        private readonly dbWebAPIContext _context;
        private readonly IHostingEnvironment server;

        //public ProductosTiendaController(dbWebAPIContext context)
        //{
        //    _context = context;
        //}
        public ProductosTiendaController(dbWebAPIContext context, IHostingEnvironment env)
        {
            _context = context;
            server = env;
        }

        // GET: api/ProductosTienda
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoTiendaDTO>>> GetProductosTienda()
        {
            try
            {
                var productos = await (from p in _context.ProductosTienda
                                       select new ProductoTiendaDTO()
                                       {
                                           IdProducto = p.IdProducto,
                                           Nombre = p.Nombre,
                                           Imagen = p.Imagen,
                                           Precio = p.Precio,
                                           Descripcion = p.Descripcion,
                                       }).ToListAsync();

                return Ok(productos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        // GET: api/ProductosTienda/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductosTienda>> GetProductosTienda(int id)
        {
            var productosTienda = await _context.ProductosTienda.FindAsync(id);

            if (productosTienda == null)
            {
                return NotFound();
            }

            return productosTienda;
        }

        [HttpGet, Route("downloadimagen")]
        public async Task<IActionResult> DownloadImagen(string ruta)
        {
            try
            {
                var rutaUploads = Path.Combine(server.WebRootPath, "uploads", ruta);
                if (System.IO.File.Exists(rutaUploads))
                {

                    var net = new System.Net.WebClient();
                    var data = net.DownloadData(rutaUploads);
                    var content = new System.IO.MemoryStream(data);
                    byte[] fileBytes = System.IO.File.ReadAllBytes(rutaUploads);
                    return new FileContentResult(fileBytes, "application/octet");
                }
                else
                {
                    throw new Exception("noexiste");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        // PUT: api/ProductosTienda/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductosTienda(int id, ProductosTienda productosTienda)
        {
            if (id != productosTienda.IdProducto)
            {
                return BadRequest();
            }

            _context.Entry(productosTienda).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductosTiendaExists(id))
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

        [HttpPost, Route("uploadimagen")]
        public async Task<IActionResult> UploadImagen()
        {
            try
            {
                var uploads = Path.Combine(server.WebRootPath, "uploads");
                foreach (var file in Request.Form.Files)
                {
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(uploads, file.FileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        // PUT: api/ProductosTienda/updateimagen
        [HttpPut, Route("updateimagen")]
        public async Task<IActionResult> UpdateImagen([FromBody] InfoCampoPicture info)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var producto = await _context.ProductosTienda.Where(p => p.IdProducto == info.IdProducto).FirstOrDefaultAsync();

                producto.Imagen = info.NombreImagen;
                _context.Entry(producto).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return Ok();
        }

        // POST: api/ProductosTienda
        [HttpPost]
        public async Task<ActionResult<ProductosTienda>> PostProductosTienda(ProductosTienda productosTienda)
        {
            _context.ProductosTienda.Add(productosTienda);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductosTienda", new { id = productosTienda.IdProducto }, productosTienda);
        }

        // DELETE: api/ProductosTienda/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductosTienda>> DeleteProductosTienda(int id)
        {
            try
            {
                var producto = await _context.ProductosTienda.FindAsync(id);
                if (producto == null)
                {
                    return NotFound();
                }

                _context.ProductosTienda.Remove(producto);

                if (producto.Imagen != null)
                {
                    var deletePath = Path.Combine(server.WebRootPath, "uploads", producto.Imagen);

                    if (System.IO.File.Exists(deletePath))
                    {
                        System.IO.File.Delete(deletePath);
                    }
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        private bool ProductosTiendaExists(int id)
        {
            return _context.ProductosTienda.Any(e => e.IdProducto == id);
        }
    }
}