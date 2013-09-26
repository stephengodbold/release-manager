using System;
using System.Collections.Generic;
using System.Linq;
using ReleaseManager.API.Common;
using ReleaseManager.API.Models;
using ReleaseManager.API.Queries;
using ReleaseManager.Common;

namespace ReleaseManager.API.Services
{
    public class WorkItemService : IWorkItemService
    {
        private readonly IBuildWorkItemQuery buildWorkItemQuery;
        private readonly IBuildQuery buildQuery;
        private readonly IServerConfigurationQuery serverConfigurationQuery;
        private readonly IWorkItemFormatter<string> workItemFormatter;

        public WorkItemService(
            IBuildWorkItemQuery buildWorkItemQuery,
            IBuildQuery buildQuery,
            IServerConfigurationQuery serverConfigurationQuery,
            IWorkItemFormatter<string> workItemFormatter
            )
        {
            this.buildWorkItemQuery = buildWorkItemQuery;
            this.buildQuery = buildQuery;
            this.serverConfigurationQuery = serverConfigurationQuery;
            this.workItemFormatter = workItemFormatter;
        }

        public ReleaseNotes GetReleaseNotes(
            string earlierBuild,
            string laterBuild)
        {
            if (string.IsNullOrWhiteSpace(earlierBuild))
            {
                throw new ArgumentOutOfRangeException("earlierBuild", earlierBuild, "Earlier build not set");
            }

            var servers = serverConfigurationQuery.Execute();
            var serverUri = new Uri(servers["Server.TeamFoundation"]);
            var projectName = servers["Server.ProjectName"];

            var earlierBuildDetail = buildQuery.Execute(serverUri, projectName, string.IsNullOrWhiteSpace(earlierBuild) ? laterBuild : earlierBuild);
            var laterBuildDetail = buildQuery.Execute(serverUri, projectName, laterBuild);

            if (earlierBuildDetail == null || laterBuildDetail == null)
            {
                return new ReleaseNotes();
            }

            var workItems = buildWorkItemQuery.Execute(earlierBuildDetail, 
                                                    laterBuildDetail, 
                                                    serverUri, 
                                                    projectName).ToArray();

            var releaseNotes = new ReleaseNotes
                                   {
                                       Items = workItems,
                                       PreviousRelease = earlierBuild,
                                       CurrentRelease = laterBuild,
                                       States = GetStates(),
                                       Title = "Release Notes",
                                       CsvItems = workItemFormatter.Format(workItems)
                                   };

            return releaseNotes;
        }

        public IEnumerable<string> GetStates()
        {
            return new[] { "Active", "Resolved", "Testing" };
        }
    }

    public class WorkItemServiceStub : IWorkItemService
    {
        public ReleaseNotes GetReleaseNotes(string earlierBuild, string laterBuild)
        {
            var items = new[]
            {
                new WorkItem
                {
                    Description = "Work Item: Do some stuff",
                    Id = "1",
                    Release = "Release 1",
                    State = "Resolved"
                },
                new WorkItem
                {
                    Description = "Work Item: Do some stuff",
                    Id = "2",
                    Release = "Release 1",
                    State = "Resolved"
                },
                new WorkItem
                {
                    Description = "Work Item: Do some stuff",
                    Id = "3",
                    Release = "Release 1",
                    State = "Testing"
                },
                new WorkItem
                {
                    Description = "Work Item: Do some stuff",
                    Id = "4",
                    Release = "Release 2",
                    State = "Ready for Development"
                },
                new WorkItem
                {
                    Description = "Work Item: Do some stuff",
                    Id = "5",
                    Release = "Release 3",
                    State = "Active"
                }
            };

            return new ReleaseNotes
                       {
                           PreviousRelease = "DemoBuild_20130401.1",
                           CurrentRelease = "DemoBuild_20130402.2",
                           Title = string.Format("Release Notes {0}", DateTime.Today.ToShortDateString()),
                           CsvItems = new WorkItemCsvFormatter().Format(items),
                           Items = items,
                           States = GetStates()
                       };
        }

        public IEnumerable<string> GetStates()
        {
            return new[] { "Active", "Resolved", "Testing" };
        }
    }

    public interface IWorkItemService
    {
        ReleaseNotes GetReleaseNotes(string earlierBuild, string laterBuild);
        IEnumerable<string> GetStates();
    }
}