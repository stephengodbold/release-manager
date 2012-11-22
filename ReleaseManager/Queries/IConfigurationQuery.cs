using System.Collections.Generic;

namespace ReleaseManager.Queries
{
    public interface IConfigurationQuery
    {
        IDictionary<string, string> Execute();
    }
}