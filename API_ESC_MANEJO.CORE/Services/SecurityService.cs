using API_ESC_MANEJO.CORE.Entities.Configuration;
using API_ESC_MANEJO.CORE.Interfaces;

using Microsoft.Extensions.Options;

namespace API_ESC_MANEJO.CORE.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly ConfigurationSecurity _configurationSecurity;
        public SecurityService(IOptions<ConfigurationSecurity> configurationSecurity)
        {
            _configurationSecurity = configurationSecurity.Value;
        }
        public bool ValidAuth(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            return key == _configurationSecurity.KeyAuth;
        }
    }
}
