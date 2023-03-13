using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docente_EscuelaApp.Models
{
    public class Calificacion
    {
        public int IdAsignatura { get; set; }

        public int IdAlumno { get; set; }

        public int IdDocente { get; set; }

        public int IdPeriodo { get; set; }

        public double Calificacion1 { get; set; }

        public int Unidad { get; set; }
    }
}
