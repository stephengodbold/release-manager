using System;
using System.Collections.Generic;
using ReleaseManager.Models;

namespace ReleaseManager.Services
{
    public interface IWorkItemService
    {
        IEnumerable<WorkItem> GetWorkItems(string earlierBuild, string laterBuild);
    }
}