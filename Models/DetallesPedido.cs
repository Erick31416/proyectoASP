using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public partial class DetallesPedido
    {
        public int IdDetalle { get; set; }
        public int PedidosIdPedido { get; set; }
        public int ProductosIdProducto { get; set; }
        public int Cantidad { get; set; }

        public Pedidos PedidosIdPedidoNavigation { get; set; }
        public ProductosTienda ProductosIdProductoNavigation { get; set; }
    }
}
