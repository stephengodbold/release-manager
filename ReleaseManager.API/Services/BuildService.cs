using System;
using System.Collections.Generic;
using System.Linq;
using ReleaseManager.API.Models;
using ReleaseManager.API.Queries;

namespace ReleaseManager.API.Services
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

        public IEnumerable<Build> GetBuilds(DateTime buildDate)
        {
            var serverConfig = serverConfigurationQuery.Execute();

            return buildsForDateQuery.Execute(buildDate, 
                new Uri(serverConfig["Server.TeamFoundation"]), 
                serverConfig["Server.ProjectName"]).OrderBy(build => build);
        }
    }

    public class BuildServiceStub : IBuildService
    {
        public IEnumerable<Build> GetBuilds(DateTime buildDate)
        {
            return new[]
                {
                    new Build 
                    {
                        Date = DateTime.Today.AddDays(-1),
                        Definition = "BuildDefinition",
                        Result = "Passed",
                        Name = "BuildDefinition_20130101.2"
                    },
                    new Build 
                    {
                        Date = DateTime.Today.AddDays(-10),
                        Definition = "BuildDefinition",
                        Result = "Stopped",
                        Name = "BuildDefinition_20130501.6"
                    },
                    new Build 
                    {
                        Date = DateTime.Today.AddDays(-5),
                        Definition = "BuildDefinition",
                        Result = "Failed",
                        Name = "BuildDefinition_20130111.1"
                    },
                };
        }
    }

    public interface IBuildService
    {
        IEnumerable<Build> GetBuilds(DateTime buildDate);
    }
}