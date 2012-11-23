using System.Collections.Generic;

namespace ReleaseManager.Services
{
    public interface IConfigurationService
    {
        IDictionary<string,string> GetEnvironments();
        IDictionary<string, string> GetServers();
    }
}