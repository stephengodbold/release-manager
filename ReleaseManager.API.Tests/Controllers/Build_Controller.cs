using System;
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

namespace ReleaseManager.API.Tests.Controllers
{
// ReSharper disable InconsistentNaming
    public static class Build_Controller
    {
        [TestClass]
        public class When_Asked_For_Builds
        {
            [TestMethod]
            public void Returns_A_Valid_Response()
            {
                var buildServiceByMode = Substitute.For<IIndex<ApiMode, IBuildService>>();
                buildServiceByMode[ApiMode.Demo].Returns(new BuildServiceStub());
                
                var controller = new BuildsController(buildServiceByMode)
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
            public void Returns_404_For_Future_Date()
            {
                var buildServiceByMode = Substitute.For<IIndex<ApiMode, IBuildService>>();
                buildServiceByMode[ApiMode.Demo].Returns(new BuildServiceStub());
                var controller = new BuildsController(buildServiceByMode)
                {
                    ControllerContext = new HttpControllerContext
                    {
                        Request = new HttpRequestMessage()
                    }
                };

                HttpResponseException exception = null;

                try
                {
                    controller.Get(DateTime.Today.AddDays(1));
                }
                catch (HttpResponseException ex)
                {
                    exception = ex;
                }

                exception.ShouldNotBe(null);
                exception.Response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            }

            [TestMethod]
            public void Returns_404_For_Unknown_Build()
            {
                var buildServiceByMode = Substitute.For<IIndex<ApiMode, IBuildService>>();
                buildServiceByMode[ApiMode.Demo].Returns(new BuildServiceStub());
                var controller = new BuildsController(buildServiceByMode)
                {
                    ControllerContext = new HttpControllerContext
                    {
                        Request = new HttpRequestMessage()
                    }
                };

                HttpResponseException exception = null;

                try
                {
                    controller.Get("Not a known build");
                }
                catch (HttpResponseException ex)
                {
                    exception = ex;
                }

                exception.ShouldNotBe(null);
                exception.Response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            }
        }
    }
}
// ReSharper restore InconsistentNaming