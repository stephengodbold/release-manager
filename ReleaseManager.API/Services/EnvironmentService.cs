using System;
using System.Collections.Generic;
using System.Linq;
using ReleaseManager.API.Queries;
using Environment = ReleaseManager.API.Models.Environment;

namespace ReleaseManager.API.Services
{
    public class EnvironmentService : IEnvironmentService
    {
        private readonly IEnvironmentQuery environmentQuery;
        private readonly IConfigurationService configurationService;

        public EnvironmentService(IEnvironmentQuery query, 
            IConfigurationService configuration)
        {
            environmentQuery = query;
            configurationService = configuration;
        }

        public IEnumerable<Environment> GetEnvironments()
        {
            var environments = configurationService.GetEnvironments();
            return environments == null ? 
                new Environment[] {} : 
                environments.Select(env => environmentQuery.Execute(new Uri(env.Value)));
        }
    }

    public interface IEnvironmentService
    {
        IEnumerable<Environment> GetEnvironments();
    }

    public class StubEnvironmentService : IEnvironmentService
    {
        public IEnumerable<Environment> GetEnvironments()
        {
            return new[]
                       {
                           new Environment
                               {
                                   CurrentBuild = "Build 1",
                                   LastReleaseDate = DateTime.Now.AddDays(-1).ToShortDateString(),
                                   Name = "Demo",
                                   PreviousBuild = string.Empty
                               },
                            new Environment
                               {
                                   CurrentBuild = "Build 2",
                                   LastReleaseDate = DateTime.Now.AddDays(-20).ToShortDateString(),
                                   Name = "Test",
                                   PreviousBuild = string.Empty
                               },
                       };
        }
    }
}