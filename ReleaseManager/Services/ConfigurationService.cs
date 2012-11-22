using System.Collections.Generic;

namespace ReleaseManager.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public IDictionary<string, string> GetEnvironments()
        {
            var config = new Dictionary<string, string>
                             {
                                 {"Demo", "http://sgdemo.studygroup.com"}
                             };
            return config;
        }
    }
}