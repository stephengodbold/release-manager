using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using System.Web.Helpers;
using Castle.MicroKernel.ComponentActivator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using ReleaseManager.API.Controllers;
using ReleaseManager.API.Queries;

namespace ReleaseManager.API.Tests.Controllers
{
// ReSharper disable InconsistentNaming
    public static class Environment_Controller
    {
        [TestClass]
        public class When_Asked_For_Environments
        {
            [TestMethod]
            public void Returns_A_Valid_Response()
            {
                var environments = new Dictionary<string, string> {{"a", "b"}};

                var environmentsQuery = Substitute.For<IEnvironmentSettingsQuery>();
                environmentsQuery.Execute().ReturnsForAnyArgs(environments);

                var environmentQuery = Substitute.For<IEnvironmentQuery>();
                
                var controller = new EnvironmentsController(environmentsQuery, environmentQuery);
                var response = controller.Get();
                
                Assert.IsNotNull(response);
            }

            [TestMethod]
            public void Returns_A_Response_With_Environments()
            {
                var environments = new Dictionary<string, string> { { "EnvironmentX", "http://url.com" } };

                var environmentsQuery = Substitute.For<IEnvironmentSettingsQuery>();
                environmentsQuery.Execute().ReturnsForAnyArgs(environments);

                var environmentQuery = Substitute.For<IEnvironmentQuery>();

                var controller = new EnvironmentsController(environmentsQuery, environmentQuery);
                var response = controller.Get();

                Assert.IsNotNull(response["EnvironmentX"]);
            }
        }
    }
}
// ReSharper restore InconsistentNaming