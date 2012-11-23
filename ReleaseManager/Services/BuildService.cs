using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReleaseManager.Models;

namespace ReleaseManager.Services
{
    public class BuildService : IBuildService
    {
        public IEnumerable<BuildDetail> GetBuilds(DateTime buildDate)
        {
            return new Collection<BuildDetail>();
        }
    }
}