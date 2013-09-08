using System;
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
                environmentsQuerySelector[ApiMode.Demo].Returns(new EnvironmentServiceStub());
                
                var controller = new EnvironmentsController(environmentsQuerySelector)
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
                environmentsQuery.List().ReturnsForAnyArgs(new[] { new Environment() });
                environmentsQuerySelector[ApiMode.Demo].ReturnsForAnyArgs(environmentsQuery);

                var controller = new EnvironmentsController(environmentsQuerySelector)
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

        [TestClass]
        public class When_Asked_For_Specific_Environment
        {
            private const string TestUriString = "http://test-env/test";

            [TestMethod]
            public void With_A_Valid_Url_Returns_An_Environment()
            {
                var environmentsQuerySelector = Substitute.For<IIndex<ApiMode, IEnvironmentService>>();
                var environmentsQuery = Substitute.For<IEnvironmentService>();
                environmentsQuery.Get(new Uri(TestUriString)).ReturnsForAnyArgs(new Environment());
                environmentsQuerySelector[ApiMode.Demo].ReturnsForAnyArgs(environmentsQuery);

                var controller = new EnvironmentsController(environmentsQuerySelector)
                {
                    ControllerContext = new HttpControllerContext
                    {
                        Request = new HttpRequestMessage()
                    }
                };

                controller.ControllerContext.Request.Properties.Add("x-api-mode", ApiMode.Demo);

                var response = controller.Get(TestUriString);

                Assert.IsNotNull(response);
            }

            [TestMethod]
            public void With_An_Invalid_Url_Throws_Uri_Format_Exception()
            {
                var environmentsQuerySelector = Substitute.For<IIndex<ApiMode, IEnvironmentService>>();
                var environmentsQuery = Substitute.For<IEnvironmentService>();
                environmentsQuery.Get(new Uri(TestUriString)).ReturnsForAnyArgs(new Environment());
                environmentsQuerySelector[ApiMode.Demo].ReturnsForAnyArgs(environmentsQuery);

                var controller = new EnvironmentsController(environmentsQuerySelector)
                {
                    ControllerContext = new HttpControllerContext
                    {
                        Request = new HttpRequestMessage()
                    }
                };

                controller.ControllerContext.Request.Properties.Add("x-api-mode", ApiMode.Demo);

                var thrown = false;

                try
                {
                    controller.Get("Not a valid url");
                }
                catch (UriFormatException ex)
                {
                    thrown = true;
                }

                Assert.IsTrue(thrown, "No uri format exception was thrown");
            }

            [TestMethod]
            public void With_A_Relative_Url_Throws_Uri_Format_Exception()
            {
                var environmentsQuerySelector = Substitute.For<IIndex<ApiMode, IEnvironmentService>>();
                var environmentsQuery = Substitute.For<IEnvironmentService>();
                environmentsQuery.Get(new Uri(TestUriString)).ReturnsForAnyArgs(new Environment());
                environmentsQuerySelector[ApiMode.Demo].ReturnsForAnyArgs(environmentsQuery);

                var controller = new EnvironmentsController(environmentsQuerySelector)
                {
                    ControllerContext = new HttpControllerContext
                    {
                        Request = new HttpRequestMessage()
                    }
                };

                controller.ControllerContext.Request.Properties.Add("x-api-mode", ApiMode.Demo);

                var thrown = false;

                try
                {
                    controller.Get("~/test/test");
                }
                catch (UriFormatException ex)
                {
                    thrown = true;
                }

                Assert.IsTrue(thrown, "No uri format exception was thrown");
            }
        }
    }
}
// ReSharper restore InconsistentNaming