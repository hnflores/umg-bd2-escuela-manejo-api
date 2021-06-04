using System;

namespace API_ESC_MANEJO.CORE.Entities
{
    public class Customer
    {
        public string CustomerId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Correo { get; set; }
        public string Estado { get; set; }
    }
}
