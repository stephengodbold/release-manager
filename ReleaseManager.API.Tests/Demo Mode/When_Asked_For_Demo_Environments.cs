using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReleaseManager.API.App_Start;
using Shouldly;

namespace ReleaseManager.API.Tests.Demo_Mode
{
    [TestClass]
    // ReSharper disable InconsistentNaming
    public class When_Set_To_Mode : WebApiTestBase
    {
        [TestMethod, TestCategory("Integration")]
        public void Should_Respond_With_Production_Header()
        {
            var request = WebRequest.Create(BaseUri + "/api/environments");
            request.Headers.Add("x-api-mode", ApiMode.Production.ToString());
            var responseHeaders = request.GetResponse().Headers;

            responseHeaders.AllKeys.ShouldContain("x-api-mode");
            responseHeaders.Get("x-api-mode").ShouldBe(ApiMode.Production.ToString(), Case.Insensitive);
        }
        
        [TestMethod, TestCategory("Integration")]
        public void Should_Respond_With_Demo_Header()
        {
            var request = WebRequest.Create(BaseUri + "/api/environments");
            request.Headers.Add("x-api-mode", ApiMode.Demo.ToString());
            var responseHeaders = request.GetResponse().Headers;
                
            responseHeaders.AllKeys.ShouldContain("x-api-mode");
            responseHeaders.Get("x-api-mode").ShouldBe(ApiMode.Demo.ToString(), Case.Insensitive);
        }

        [TestMethod, TestCategory("Integration")]
        public void Should_Default_Mode_To_Production()
        {
            var request = WebRequest.Create(BaseUri + "/api/environments");
            var responseHeaders = request.GetResponse().Headers;

            responseHeaders.AllKeys.ShouldContain("x-api-mode");
            responseHeaders.Get("x-api-mode").ShouldBe(ApiMode.Production.ToString(), Case.Insensitive);
        }
    }
}
// ReSharper restore InconsistentNaming
