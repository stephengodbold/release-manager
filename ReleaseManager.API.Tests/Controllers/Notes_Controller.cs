using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
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
    [TestClass]
    public class Notes_Controller
    {
        [TestMethod]
        public void Returns_400_For_One_Build_Specified()
        {
            var resolver = Substitute.For<IIndex<ApiMode, IWorkItemService>>();
            resolver[ApiMode.Demo].ReturnsForAnyArgs(new WorkItemServiceStub());

            var controller = new NotesController(resolver)
            {
                ControllerContext = new HttpControllerContext
                {
                    Request = new HttpRequestMessage()
                }
            };

            controller.ControllerContext.Request.Properties.Add("x-api-mode", ApiMode.Demo);

            var exception = Should.Throw<HttpResponseException>(() => controller.Get("testbuild"));
            exception.Response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void Returns_Items_For_Two_Builds_Specified()
        {
            var resolver = Substitute.For<IIndex<ApiMode, IWorkItemService>>();
            resolver[ApiMode.Demo].ReturnsForAnyArgs(new WorkItemServiceStub());

            var controller = new NotesController(resolver)
            {
                ControllerContext = new HttpControllerContext
                {
                    Request = new HttpRequestMessage()
                }
            };

            controller.ControllerContext.Request.Properties.Add("x-api-mode", ApiMode.Demo);

            var result = controller.Get("later build", "earlierBuild");

            result.ShouldNotBe(null);
            result.Items.Count().ShouldBeGreaterThan(0);
        }
    }
}
