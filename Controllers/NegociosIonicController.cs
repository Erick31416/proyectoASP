using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.ModelsDTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NegociosIonicController : ControllerBase
    {
        private readonly dbWebAPIContext db;

        public NegociosIonicController(dbWebAPIContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<NegocioDTO>> GetNegociosIonic()
        {
            try
            {
                var negocios = await (from n in db.Negocios
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

        // GET: api/NegociosIonic/tipos
        [HttpGet("tipos")]
        public async Task<ActionResult<IEnumerable<TipoDTO>>> GetTipos()
        {
            try
            {
                var tipos = await (from t in db.Tipos
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

        // GET: api/NegociosIonic/radio
        [HttpGet("radio")]
        public async Task<ActionResult<IEnumerable<NegocioIonicDTO>>> GetNegociosRadio(decimal lat, decimal lng, int tipo, int radio)
        {
            var negocios = (from n in db.Negocios
                            where n.TiposIdTipo == tipo
                            select new NegocioIonicDTO()
                            {
                                Nombre = n.Nombre,
                                Direccion = n.Direccion,
                                Descripcion = n.Descripcion,
                                Lat = n.Lat,
                                Lng = n.Lng,
                                Distancia = (6371 * Math.Acos(Math.Cos(radians(lat))
                                * Math.Cos(radians(n.Lat)) * Math.Cos(radians(n.Lng) - radians(lng))
                                 + Math.Sin(radians(lat))
                                 * Math.Sin(radians(n.Lat))))
                            }).Where(x => x.Distancia < radio);

            return await negocios.ToListAsync();
        }

        // http://localhost:52034/api/NegociosIonic/Radio?lat=42.826559&lng=-1.6193751&tipo=4&radio=1000

        private double radians(decimal angle)
        {
            double angulo = (double)angle;
            return Math.PI * angulo / 180.0;
        }

    }
}