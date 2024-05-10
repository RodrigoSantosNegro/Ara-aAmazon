﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AranhaAmazon.Utils
{
    internal class Producto
    {
        public string cod_ean { get; set; }
        public string nombre { get ; set; }
        public double precio { get; set; }
        public string url { get; set;}
        public string fecha_lectura { get; set; }
        public int valoracion { get; set; }
        public bool oferta { get; set; }
    }
}