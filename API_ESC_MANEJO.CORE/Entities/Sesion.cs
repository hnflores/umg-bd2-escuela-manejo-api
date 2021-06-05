using System;

namespace API_ESC_MANEJO.CORE.Entities
{
    public class Sesion
    {
        public int SesionId { get; set; }
        public int ContractId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
    }
}
