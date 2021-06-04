using System;
using System.Collections.Generic;

namespace API_ESC_MANEJO.CORE.Entities
{
    public class Contract
    {
        public int ContractId { get; set; }
        public string VehicleId { get; set; }
        public int UserId { get; set; } //conductor
        public string CustomerId { get; set; }
        public int NumeroSesiones { get; set; }
        public decimal Costo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
        public List<Sesion> Sesiones { get; set; }
        public string Conductor { get; set; }
        public string Cliente { get; set; }
        public string Vehiculo { get; set; }
    }
}
