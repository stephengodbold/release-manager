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

        public IEnumerable<Environment> List()
        {
            var environments = configurationService.GetEnvironments();
            return environments == null ? 
                new Environment[] {} : 
                environments.Select(env => environmentQuery.Execute(new Uri(env.Value)));
        }

        public Environment Get(Uri environmentUri)
        {
            return environmentQuery.Execute(environmentUri);
        }
    }

    public interface IEnvironmentService
    {
        IEnumerable<Environment> List();
        Environment Get(Uri environmentUri);
    }

    public class EnvironmentServiceStub : IEnvironmentService
    {
        public IEnumerable<Environment> List()
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
                                   PreviousBuild = "Build 1"
                               },
                       };
        }

        public Environment Get(Uri environmentUri)
        {
            return new Environment
            {
                Name = "Demo Environment",
                CurrentBuild = "Demo_Build20130604.03",
                PreviousBuild = "Demo_Build20130525.04",
                LastReleaseDate = "4th June 2013"
            };
        }
    }
}