using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docente_EscuelaApp.Models
{
    public class Docente
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string ApellidoPaterno { get; set; } = null!;

        public string ApellidoMaterno { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        public int Edad { get; set; }

        public int TipoDocente { get; set; }

        public int IdUsuario { get; set; }
        public short Periodo { get; set; }
        public int IdAsigantura { get; set; }
        public string NombreAsignatura { get; set; } = null!;
    }
}
