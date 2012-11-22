using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using ReleaseManager.Models;
using ReleaseManager.Queries;
using ReleaseManager.Services;
using Shouldly;

namespace ReleaseManager.Tests
{
    [TestClass]
    public class Environment_Service
    {
        [TestClass]
        public class When_Getting_Environments
        {
            [TestMethod]
            public void Should_Return_Empty_List_For_No_Configuration()
            {
                var configService = Substitute.For<IConfigurationService>();
                configService.GetEnvironments().Returns(info => null);

                var environmentService = new EnvironmentService(null, configService);
                var environments = environmentService.GetEnvironments();

                environments.Count().ShouldBe(0);
            }

            [TestMethod]
            public void Should_Return_Only_Valid_Uris_From_Configuration()
            {
                var configService = Substitute.For<IConfigurationService>();
                var query = Substitute.For<IEnvironmentQuery>();

                configService.GetEnvironments()
                    .Returns(info => new Dictionary<string, string>
                        {
                            {"Environment.Demo", "xyz"},
                            {"Environment.Test", "http://test.com"}
                        });

                query.GetEnvironmentDetails(Arg.Any<Uri>()).ReturnsForAnyArgs(new Models.Environment());

                var environmentService = new EnvironmentService(query, configService);
                var environments = environmentService.GetEnvironments();

                environments.Count().ShouldBe(1);
            }

            [TestMethod]
            public void Should_Return_All_Configured_Environments()
            {
                var configService = Substitute.For<IConfigurationService>();
                var query = Substitute.For<IEnvironmentQuery>();

                configService.GetEnvironments()
                    .Returns(info => new Dictionary<string, string>
                        {
                            {"Environment.Demo", "http://test.com"},
                            {"Environment.Test", "http://test.com"},
                            {"Environment.UAT", "http://test.com"},
                            {"Environment.Production", "http://test.com"},
                        });

                query.GetEnvironmentDetails(Arg.Any<Uri>()).ReturnsForAnyArgs(new Models.Environment());

                var environmentService = new EnvironmentService(query, configService);
                var environments = environmentService.GetEnvironments();

                environments.Count().ShouldBe(4);
            }
        }
    }
}