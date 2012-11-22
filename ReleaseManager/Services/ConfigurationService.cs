using System.Collections.Generic;
using System.Configuration;
using System.Linq;
namespace ReleaseManager.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public IDictionary<string, string> GetEnvironments()
        {
            var environmentKeys = ConfigurationManager.AppSettings.AllKeys
                            .Where(key => key.StartsWith("Environment."));

            return environmentKeys.ToDictionary(
                environmentKey => environmentKey, 
                environmentKey => ConfigurationManager.AppSettings[environmentKey]);
        }
    }
}