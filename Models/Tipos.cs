using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public partial class Tipos
    {
        public Tipos()
        {
            Negocios = new HashSet<Negocios>();
        }

        public int IdTipo { get; set; }
        public string Tipo { get; set; }

        public ICollection<Negocios> Negocios { get; set; }
    }
}
