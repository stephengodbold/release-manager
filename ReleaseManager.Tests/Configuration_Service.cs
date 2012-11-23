using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using ReleaseManager.Queries;
using ReleaseManager.Services;
using Shouldly;

namespace ReleaseManager.Tests
{
    [TestClass]
    public class Configuration_Service
    {
        [TestClass]
        public class When_Getting_Configuration
        {
            [TestMethod]
            public void Should_Return_Only_Valid_Uris_From_Configuration()
            {
                var envConfigQuery = Substitute.For<IEnvironmentConfigurationQuery>();
                var serverConfigQuery = Substitute.For<IServerConfigurationQuery>();
                var query = Substitute.For<IEnvironmentQuery>();

                envConfigQuery.Execute()
                    .Returns(info => new Dictionary<string, string>
                        {
                            {"Environment.Demo", "xyz"},
                            {"Environment.Test", "http://test.com"}
                        });

                query.Execute(Arg.Any<Uri>()).ReturnsForAnyArgs(new Models.Environment());

                var configService = new ConfigurationService(envConfigQuery, serverConfigQuery);
                var environments = configService.GetEnvironments();

                environments.Count().ShouldBe(1);
            }
        }
    }
}
