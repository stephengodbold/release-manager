using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ReleaseManager.Queries
{
    public class ConfigurationQuery : IEnvironmentConfigurationQuery
    {
        public IDictionary<string, string> Execute()
        {
            var environmentKeys = ConfigurationManager.AppSettings.AllKeys
                .Where(key => key.StartsWith("Environment."));

            return environmentKeys.ToDictionary(
                environmentKey => environmentKey,
                environmentKey => ConfigurationManager.AppSettings[environmentKey]);
        }
    }

    public interface IEnvironmentConfigurationQuery
    {
        IDictionary<string, string> Execute();
    }
}