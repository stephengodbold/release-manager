using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using Autofac.Features.Indexed;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using ReleaseManager.API.App_Start;
using ReleaseManager.API.Controllers;
using ReleaseManager.API.Queries;
using ReleaseManager.API.Services;
using Environment = ReleaseManager.API.Models.Environment;

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
                var environmentsQuerySelector = Substitute.For<IIndex<ApiMode, IEnvironmentService>>();
                var environmentsQuery = Substitute.For<IEnvironmentService>();
                environmentsQuery.GetEnvironments().ReturnsForAnyArgs(new[] {new Environment()});
                environmentsQuerySelector[ApiMode.Demo].Returns(environmentsQuery);
                
                var environmentQuery = Substitute.For<IEnvironmentQuery>();
                var controller = new EnvironmentsController(environmentsQuerySelector, environmentQuery)
                {
                    ControllerContext = new HttpControllerContext
                    {
                        Request = new HttpRequestMessage()
                    }
                };

                controller.ControllerContext.Request.Properties.Add("x-api-mode", ApiMode.Demo);
                var response = controller.Get();
                
                Assert.IsNotNull(response);
            }

            [TestMethod]
            public void Returns_A_Response_With_Environments()
            {
                var environmentsQuerySelector = Substitute.For<IIndex<ApiMode, IEnvironmentService>>();
                var environmentsQuery = Substitute.For<IEnvironmentService>();
                environmentsQuery.GetEnvironments().ReturnsForAnyArgs(new[] { new Environment() });
                environmentsQuerySelector[ApiMode.Demo].ReturnsForAnyArgs(environmentsQuery);

                var environmentQuery = Substitute.For<IEnvironmentQuery>();
                var controller = new EnvironmentsController(environmentsQuerySelector, environmentQuery)
                {
                    ControllerContext = new HttpControllerContext
                    {
                        Request = new HttpRequestMessage()
                    }
                };

                controller.ControllerContext.Request.Properties.Add("x-api-mode", ApiMode.Demo);

                var response = controller.Get();
                Assert.AreEqual(1, response.Count());
            }
        }
    }
}
// ReSharper restore InconsistentNaming