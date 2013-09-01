using System;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReleaseManager.API.Tests.Demo_Mode
{
    [TestClass]
    // ReSharper disable InconsistentNaming
    public class When_Asked_For_Demo_Environments : WebApiTestBase
    {
        [TestMethod, TestCategory("Integration")]
        public void Should_Respond_With_Demo_Header()
        {
            var request = WebRequest.Create(BaseUri + "/api/environments");
            request.Headers.Add("x-api-mode", "demo");
            var response = request.GetResponse();

            Assert.IsTrue(response.Headers.AllKeys.Contains("x-api-mode"));
        } 
    }
}
// ReSharper restore InconsistentNaming
