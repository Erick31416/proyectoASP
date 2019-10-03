using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public partial class ProductosTienda
    {
        public ProductosTienda()
        {
            DetallesPedido = new HashSet<DetallesPedido>();
        }

        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }

        public ICollection<DetallesPedido> DetallesPedido { get; set; }
    }
}
