using System;
using System.Diagnostics;
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
            BaseUri = new Uri("http://localhost/Temporary_Listen_Addresses/");
            var config = new HttpSelfHostConfiguration(BaseUri);

            WebApiConfig.Register(config);
            FilterConfig.RegisterGlobalFilters(config);
            ContainerConfig.RegisterTypes(config);

            server = new HttpSelfHostServer(config);

            server.OpenAsync().ContinueWith(
                    request =>
                    {
                        if (!request.IsFaulted) return;

                        foreach (var exception in request.Exception.InnerExceptions)
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
