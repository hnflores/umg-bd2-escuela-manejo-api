using System;

namespace API_ESC_MANEJO.CORE.Entities.Configuration
{
    public class ConfigurationLog
    {
        public string NameFile { get; set; }
        public string Path { get; set; }
        public string Date => DateTime.Now.ToString("dd-MM-yyyy");
    }
}
