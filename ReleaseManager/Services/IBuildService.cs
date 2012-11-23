using System;
using System.Collections.Generic;
using ReleaseManager.Models;

namespace ReleaseManager.Services
{
    public interface IBuildService
    {
        IEnumerable<BuildDetail> GetBuilds(DateTime buildDate);
    }
}