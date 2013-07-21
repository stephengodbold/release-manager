using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ReleaseManager.API.Queries
{
    public class ServerConfigurationQuery : IServerConfigurationQuery
    {
        public IDictionary<string, string> Execute()
        {
            var environmentKeys = ConfigurationManager.AppSettings.AllKeys
                .Where(key => key.StartsWith("Server."));

            return environmentKeys.ToDictionary(
                environmentKey => environmentKey,
                environmentKey => ConfigurationManager.AppSettings[environmentKey]);
        }
    }

    public interface IServerConfigurationQuery
    {
        IDictionary<string, string> Execute();
    }
}