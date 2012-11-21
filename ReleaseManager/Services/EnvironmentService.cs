using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReleaseManager.Models;

namespace ReleaseManager.Services
{
    public class EnvironmentService : IEnvironmentService
    {
        public IEnumerable<Environment> GetEnvironments()
        {
            return new Collection<Environment>
                       {
                           new Environment
                               {
                                   Name = "Demo",
                                   CurrentBuild = "R2.1",
                                   PreviousBuild = "R1.6",
                                   LastReleaseDate = "10 October 2012"
                               },
                            new Environment
                               {
                                   Name = "Test",
                                   CurrentBuild = "R2.1",
                                   PreviousBuild = "R1.6",
                                   LastReleaseDate = "21 July 2012"
                               },
                               new Environment
                               {
                                   Name = "UAT",
                                   CurrentBuild = "R2.1",
                                   PreviousBuild = "R1.6",
                                   LastReleaseDate = "5 June 2012"
                               }
                       };
        }
    }
}