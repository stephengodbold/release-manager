using System;
using System.Collections.Generic;
using System.Linq;
using ReleaseManager.Queries;
using Environment = ReleaseManager.Models.Environment;

namespace ReleaseManager.Services
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
                environments.Select(env => environmentQuery.GetEnvironmentDetails(new Uri(env.Value)));
        }
    }
}