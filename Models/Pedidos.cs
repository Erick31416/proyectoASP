using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public partial class Pedidos
    {
        public Pedidos()
        {
            DetallesPedido = new HashSet<DetallesPedido>();
        }

        public int IdPedido { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
        public string Intent { get; set; }
        public string Status { get; set; }
        public string PaypalStatus { get; set; }

        public ICollection<DetallesPedido> DetallesPedido { get; set; }
    }
}
