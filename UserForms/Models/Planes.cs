using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserForms.Models
{
    public class Planes
    {
        public int Plan { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }
        public string TextoSuscribir { get; set; }
        public string[] Caracteristicas { get; set; }
        public bool Featured { get; set; }
        public string Motivator { get; set; }
        public string ShortDescription { get; set; }
    }
}
