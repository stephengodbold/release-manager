using System;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReleaseManager.API.Tests.Demo_Mode
{
    [TestClass]
    // ReSharper disable InconsistentNaming
    public class When_Set_To_Mode : WebApiTestBase
    {
        [TestMethod, TestCategory("Integration")]
        public void Should_Respond_With_Demo_Header()
        {
            var request = WebRequest.Create(BaseUri + "/api/environments");
            request.Headers.Add("x-api-mode", "demo");
            var responseHeaders = request.GetResponse().Headers;
                
            Assert.IsTrue(responseHeaders.AllKeys.Contains("x-api-mode"));
            Assert.IsTrue(responseHeaders.Get("x-api-mode").Equals("demo", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod, TestCategory("Integration")]
        public void Should_Default_Mode()
        {
            var request = WebRequest.Create(BaseUri + "/api/environments");
            var responseHeaders = request.GetResponse().Headers;

            Assert.IsTrue(responseHeaders.AllKeys.Contains("x-api-mode"));
            Assert.IsTrue(responseHeaders.Get("x-api-mode").Equals("demo", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
// ReSharper restore InconsistentNaming
