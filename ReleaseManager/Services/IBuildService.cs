using System;
using System.Collections.Generic;

namespace ReleaseManager.Services
{
    public interface IBuildService
    {
        IEnumerable<string> GetBuilds(DateTime buildDate);
    }
}