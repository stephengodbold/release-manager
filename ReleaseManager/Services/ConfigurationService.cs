using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ReleaseManager.Queries;

namespace ReleaseManager.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IEnvironmentConfigurationQuery environmentQuery;
        private readonly IServerConfigurationQuery serverQuery;

        public ConfigurationService(IEnvironmentConfigurationQuery environmentQuery,
            IServerConfigurationQuery serverQuery)
        {
            this.environmentQuery = environmentQuery;
            this.serverQuery = serverQuery;
        }

        public IDictionary<string, string> GetEnvironments()
        {
            var configurationValues = environmentQuery.Execute();

            return configurationValues
                .Where(config => Uri.IsWellFormedUriString(config.Value, UriKind.Absolute))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public IDictionary<string, string> GetServers()
        {
            return serverQuery.Execute();
        }
    }
}