using System.Collections.Generic;
using System.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using ReleaseManager.API.Controllers;
using ReleaseManager.API.Queries;

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
                var environments = new Dictionary<string, string> {{"a", "b"}};

                var environmentQuery = Substitute.For<IEnvironmentSettingsQuery>();
                environmentQuery.Execute().ReturnsForAnyArgs(environments);
                    
                var controller = new EnvironmentController(environmentQuery);
                var response = controller.List();
                
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Data);
                Assert.IsFalse(response.Data.ToString().Equals("null"));
            }

            [TestMethod]
            public void Returns_A_Json_Response_With_Environments()
            {
                var environments = new Dictionary<string, string> { { "EnvironmentX", "http://url.com" } };

                var environmentQuery = Substitute.For<IEnvironmentSettingsQuery>();
                environmentQuery.Execute().ReturnsForAnyArgs(environments);

                var controller = new EnvironmentController(environmentQuery);
                var response = controller.List();

                var deserialisedResult = Json.Decode(response.Data.ToString());

                Assert.IsNotNull(deserialisedResult.EnvironmentX);
            }
        }
    }
}
// ReSharper restore InconsistentNaming