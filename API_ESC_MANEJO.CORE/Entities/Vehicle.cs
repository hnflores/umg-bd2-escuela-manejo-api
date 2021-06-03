using System;

namespace API_ESC_MANEJO.CORE.Entities
{
    public class Vehicle
    {
        public string VehicleId { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Anio { get; set; }
        public DateTime FechaCompra { get; set; }
        public string Estado { get; set; }
    }
}
