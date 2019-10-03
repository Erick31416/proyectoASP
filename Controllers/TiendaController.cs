using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class TiendaController : Controller
    {
        private readonly dbWebAPIContext _context;
        private readonly IHostingEnvironment server;

        public TiendaController(dbWebAPIContext context, IHostingEnvironment env)
        {
            _context = context;
            server = env;
        }

        // GET: api/Tienda
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

        // GET: api/Tienda/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoTiendaDTO>> GetProductoTienda(int id)
        {
            try
            {
                var producto = await (from p in _context.ProductosTienda
                                      where p.IdProducto == id
                                      select new ProductoTiendaDTO()
                                      {
                                          IdProducto = p.IdProducto,
                                          Nombre = p.Nombre,
                                          Imagen = p.Imagen,
                                          Precio = p.Precio,
                                          Descripcion = p.Descripcion,
                                      }).FirstOrDefaultAsync();

                return producto;
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        // GET: api/Tienda/downloadimagen
        [HttpGet, Route("downloadimagen")]
        public async Task<IActionResult> DownloadImagenTienda(string ruta)
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
    }
}