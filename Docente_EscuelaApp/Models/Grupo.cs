using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docente_EscuelaApp.Models
{
    public class Grupo
    {
        public int Id { get; set; }

        public string Grado { get; set; } = null!;

        public string Seccion { get; set; } = null!;
    }
}
