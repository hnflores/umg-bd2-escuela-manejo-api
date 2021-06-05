namespace API_ESC_MANEJO.CORE.Entities
{
    public class User
    {
        public int ColaboradorId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }       
        public string Correo { get; set; }
        public string Password { get; set; }
        public string Estado { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public string Colonia { get; set; }
    }
}
