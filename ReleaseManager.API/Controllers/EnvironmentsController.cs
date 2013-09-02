using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Web.Http;
using ReleaseManager.API.Common;
using ReleaseManager.API.Queries;
using Environment = ReleaseManager.API.Models.Environment;

namespace ReleaseManager.API.Controllers
{
    public class EnvironmentsController : ApiController, IEnvironmentController
    {
        private readonly IEnvironmentSettingsQuery environmentsQuery;
        private readonly IEnvironmentQuery environmentQuery;

        public EnvironmentsController(IEnvironmentSettingsQuery environmentsQuery, IEnvironmentQuery environmentQuery)
        {
            this.environmentsQuery = environmentsQuery;
            this.environmentQuery = environmentQuery;
        }

        public IDictionary<string, string> Get()
        {
            var environments = environmentsQuery.Execute();
            return environments;
        }

        //public IDictionary<string, string> GetDemo()
        //{
        //    return new Dictionary<string, string> { { "Demo", "Demo" } };
        //}

        public Environment Get(string uri)
        {
            var environmentUri = new Uri(uri);
            var environment = environmentQuery.Execute(environmentUri);

            return environment;
        }
    }

    public interface IEnvironmentController
    {
        IDictionary<string, string> Get();
        Environment Get(string uri);
    }
}
