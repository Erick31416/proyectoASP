﻿using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public partial class Productos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
    }
}
