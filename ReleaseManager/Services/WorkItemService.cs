using System;
using System.Collections.Generic;
using ReleaseManager.Models;
using ReleaseManager.Queries;

namespace ReleaseManager.Services
{
    public class WorkItemService : IWorkItemService
    {
        private readonly IBuildWorkItemQuery buildWorkItemQuery;
        private readonly IBuildQuery buildQuery;
        private readonly IServerConfigurationQuery serverConfigurationQuery;

        public WorkItemService(
            IBuildWorkItemQuery buildWorkItemQuery,
            IBuildQuery buildQuery,
            IServerConfigurationQuery serverConfigurationQuery)
        {
            this.buildWorkItemQuery = buildWorkItemQuery;
            this.buildQuery = buildQuery;
            this.serverConfigurationQuery = serverConfigurationQuery;
        }

        public IEnumerable<WorkItem> GetWorkItems(
            string earlierBuild, 
            string laterBuild)
        {
            var servers = serverConfigurationQuery.Execute();
            var serverUri = new Uri(servers["TeamFoundation"]);
            var projectName = servers["ProjectName"];

            var earlierBuildDetail = buildQuery.Execute(serverUri, projectName, string.IsNullOrWhiteSpace(earlierBuild) ? laterBuild : earlierBuild);
            var laterBuildDetail = buildQuery.Execute(serverUri, projectName, laterBuild);
            var workItems = buildWorkItemQuery.Execute(earlierBuildDetail, laterBuildDetail, serverUri, projectName);

            return workItems;
        }
    }
}