using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public partial class Negocios
    {
        public int IdNegocio { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Descripcion { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public int TiposIdTipo { get; set; }

        public Tipos TiposIdTipoNavigation { get; set; }
    }
}
