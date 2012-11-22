using System;
using Environment = ReleaseManager.Models.Environment;

namespace ReleaseManager.Queries
{
    public class EnvironmentQuery : IEnvironmentQuery
    {
        public Environment GetEnvironmentDetails(Uri rootUrl)
        {
            return new Environment
                       {
                           Name = rootUrl.Host,
                           CurrentBuild = "R1.0",
                           PreviousBuild = "R0.9",
                           LastReleaseDate = "10th October 2012"
                       };
        }
    }
}