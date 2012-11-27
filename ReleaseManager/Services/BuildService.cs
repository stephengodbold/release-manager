using System;
using System.Collections.Generic;
using System.Linq;
using ReleaseManager.Queries;

namespace ReleaseManager.Services
{
    public class BuildService : IBuildService
    {
        private readonly IBuildsForDateQuery buildsForDateQuery;
        private readonly IServerConfigurationQuery serverConfigurationQuery;

        public BuildService(IBuildsForDateQuery buildsForDateQuery,
            IServerConfigurationQuery serverConfigurationQuery)
        {
            this.buildsForDateQuery = buildsForDateQuery;
            this.serverConfigurationQuery = serverConfigurationQuery;
        }

        public IEnumerable<string> GetBuilds(DateTime buildDate)
        {
            var serverConfig = serverConfigurationQuery.Execute();
            return buildsForDateQuery.Execute(buildDate, 
                new Uri(serverConfig["Server.TeamFoundation"]), 
                serverConfig["Server.ProjectName"]).OrderBy(build => build);
        }
    }

    public interface IBuildService
    {
        IEnumerable<string> GetBuilds(DateTime buildDate);
    }
}