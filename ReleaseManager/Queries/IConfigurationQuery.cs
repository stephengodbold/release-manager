using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ReleaseManager.Queries
{
    public interface IConfigurationQuery
    {
        IDictionary<string, string> Execute();
    }

    public class EnvironmentConfigurationQuery : IConfigurationQuery
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
}