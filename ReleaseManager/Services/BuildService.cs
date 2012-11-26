using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ReleaseManager.Services
{
    public class BuildService : IBuildService
    {
        public IEnumerable<string> GetBuilds(DateTime buildDate)
        {
            return new Collection<string>
                       {
                           "Continuous_20121126.1",
                           "Continuous_20121126.2",
                           "Continuous_20121126.3",
                           "Continuous_20121126.4",
                       };
        }
    }

    public interface IBuildService
    {
        IEnumerable<string> GetBuilds(DateTime buildDate);
    }
}