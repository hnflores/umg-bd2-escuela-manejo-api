namespace API_ESC_MANEJO.CORE.Entities.Configuration
{
    public class ConfigurationBD
    {
        public BDModel Administrator { get; set; }
    }
    public class BDModel
    {
        public string Server { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int Timeout { get; set; }
    }
}
