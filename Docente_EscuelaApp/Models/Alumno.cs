using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docente_EscuelaApp.Models
{
    public class Alumno
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Matricula { get; set; } = null!;

        public int? IdGrupo { get; set; }

    }
}
