using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ReleaseManager.API.Queries
{
    public class EnvironmentSettingsQuery : IEnvironmentSettingsQuery
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

    public interface IEnvironmentSettingsQuery
    {
        IDictionary<string, string> Execute();
    }
}