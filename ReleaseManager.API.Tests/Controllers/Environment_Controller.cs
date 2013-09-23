using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Autofac.Features.Indexed;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using ReleaseManager.API.App_Start;
using ReleaseManager.API.Controllers;
using ReleaseManager.API.Services;
using Shouldly;
using Environment = ReleaseManager.API.Models.Environment;

namespace ReleaseManager.API.Tests.Controllers
{
// ReSharper disable InconsistentNaming
    public static class Environment_Controller
    {
        [TestClass]
        public class When_Asked_For_Environments
        {
            private const string TestUriString = "http://test-env/test";

            [TestMethod]
            public void Returns_A_Valid_Demo_Response()
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
                
                response.ShouldNotBeSameAs(null);
            }

            [TestMethod]
            public void Returns_An_Environment_For_A_Valid_Url()
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
            public void Returns_404_For_An_Invalid_Url()
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

                try
                {
                    controller.Get("Not a valid url");
                }
                catch (HttpResponseException ex)
                {
                    ex.Response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
                }

                
            }

            [TestMethod]
            public void Returns_404_For_A_Relative_Url()
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

                try
                {
                    controller.Get("~/builds/1");
                }
                catch (HttpResponseException ex)
                {
                    ex.Response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
                }
            }
        }
    }
}
// ReSharper restore InconsistentNaming