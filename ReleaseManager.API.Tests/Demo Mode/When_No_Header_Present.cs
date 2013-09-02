using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReleaseManager.API.Tests.Demo_Mode
{
    // ReSharper disable InconsistentNaming
    [TestClass]
    public class When_No_Request_Header_Present : WebApiTestBase
    {
        [TestMethod, TestCategory("Integration")]
        public void Should_Default_Mode()
        {
            var request = WebRequest.Create(BaseUri + "/api/environments");
            var responseHeaders = request.GetResponse().Headers;

            Assert.IsTrue(responseHeaders.AllKeys.Contains("x-api-mode"));
            Assert.AreEqual("default", responseHeaders.Get("x-api-mode"));
        }
    }
    // ReSharper restore InconsistentNaming
}
