using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReleaseManager.API.Tests.Demo_Mode
{
    [TestClass]
    public class When_No_Request_Header_Present : WebApiTestBase
    {
        [TestMethod, TestCategory("Integration")]
        public void Should_Default_Mode()
        {
            var request = WebRequest.Create(BaseUri + "/api/environments");
            var response = request.GetResponse();

            Assert.IsTrue(response.Headers.AllKeys.Contains("x-api-mode"));
            Assert.AreEqual("default", response.Headers.GetValues("x-api-mode").First());

        }
    }
}
