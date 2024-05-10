using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AranhaAmazon.Utils
{
    internal class Producto
    {
        public string nombre { get ; set; }
        public float precio { get; set; }
        public string url { get; set;}
        public string fecha_lectura { get; set; }
        public float valoracion { get; set; }
        public bool oferta { get; set; }
        public string categoria { get; set; }
        public float precio_antes_oferta { get; set; }
    }
}
