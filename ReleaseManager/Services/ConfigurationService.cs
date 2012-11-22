using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ReleaseManager.Queries;

namespace ReleaseManager.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationQuery query;

        public ConfigurationService(IConfigurationQuery query)
        {
            this.query = query;
        }

        public IDictionary<string, string> GetEnvironments()
        {
            var configurationValues = query.Execute();

            return configurationValues
                .Where(config => Uri.IsWellFormedUriString(config.Value, UriKind.Absolute))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}