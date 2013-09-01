using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReleaseManager.API.App_Start;

namespace ReleaseManager.API.Tests
{
    public class WebApiTestBase
    {
        private HttpSelfHostServer server;
        protected Uri BaseUri { get; private set; }

        [TestInitialize]
        public void Initialise()
        {
            BaseUri = new Uri("http://localhost:8181");
            var config = new HttpSelfHostConfiguration(BaseUri)
            {
                IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always,
                HostNameComparisonMode = HostNameComparisonMode.Exact
            };

            WebApiConfig.Register(config);
            FilterConfig.RegisterApiGlobalFilters(config);
            ContainerConfig.RegisterTypes(config);

            server = new HttpSelfHostServer(config);

            server.OpenAsync().ContinueWith(
                    t =>
                    {
                        if (!t.IsFaulted) return;

                        foreach (var exception in t.Exception.InnerExceptions)
                        {
                            Trace.WriteLine(exception.ToString());
                        }
                    }
                ).Wait();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            server.CloseAsync().Wait();
            server.Dispose();
        }
    }
}
