﻿using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public partial class Usuarios
    {
        public int IdUsuario { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
