using System;
using System.Collections.Generic;
using System.Linq;
using ReleaseManager.API.Queries;

namespace ReleaseManager.API.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IEnvironmentSettingsQuery environmentQuery;
        private readonly IServerConfigurationQuery serverQuery;

        public ConfigurationService(IEnvironmentSettingsQuery environmentQuery,
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

    public interface IConfigurationService
    {
        IDictionary<string, string> GetEnvironments();
        IDictionary<string, string> GetServers();
    }

}