using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using ReleaseManager.Models;
using WorkItem = ReleaseManager.Models.WorkItem;

namespace ReleaseManager.Queries
{
    public class BuildWorkItemQuery : IBuildWorkItemQuery
    {
        private readonly IWorkItemCategoryQuery workItemCategoryQuery;

        public BuildWorkItemQuery(IWorkItemCategoryQuery workItemCategoryQuery)
        {
            this.workItemCategoryQuery = workItemCategoryQuery;
        }

        public IEnumerable<WorkItem> Execute(BuildDetail earliestBuild, BuildDetail latestBuild, Uri serverUri, string projectName)
        {
            if (earliestBuild == null || latestBuild == null)
            {
                return new WorkItem[] {};
            }

            using (var collection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(serverUri))
            {
                var builds = earliestBuild.BranchRoot.Equals(latestBuild.BranchRoot, StringComparison.InvariantCultureIgnoreCase) ?
                    GetBuildsFromOneBranch(earliestBuild, latestBuild, projectName, collection) :
                    GetBuildsAcrossBranches(earliestBuild, latestBuild, projectName, collection);

                return GetWorkItemsForBuilds(builds, collection, projectName);
            }
        }

        private IEnumerable<IBuildDetail> GetBuildsAcrossBranches(
            BuildDetail earliestBuild,
            BuildDetail latestBuild,
            string projectName,
            TfsConnection collection)
        {
            var buildServer = collection.GetService<IBuildServer>();
            var versionControlServer = collection.GetService<VersionControlServer>();
            var targetBranch = versionControlServer.QueryBranchObjects(new ItemIdentifier(latestBuild.BranchRoot),
                                    RecursionType.None);

            //All builds up until the branch
            var searchSpec = buildServer.CreateBuildDetailSpec(projectName, earliestBuild.Definition);
            searchSpec.MinFinishTime = earliestBuild.Date;
            searchSpec.MaxFinishTime = targetBranch.First().DateCreated;
            searchSpec.QueryDeletedOption = QueryDeletedOption.IncludeDeleted;
            var firstResultSet = buildServer.QueryBuilds(searchSpec);

            //Builds until last build on the target branch
            searchSpec = buildServer.CreateBuildDetailSpec(projectName, latestBuild.Definition);
            searchSpec.MaxFinishTime = latestBuild.Date;
            searchSpec.QueryDeletedOption = QueryDeletedOption.IncludeDeleted;
            var secondResultSet = buildServer.QueryBuilds(searchSpec);

            var builds = firstResultSet.Builds.Union(secondResultSet.Builds);

            return builds;
        }

        private IEnumerable<IBuildDetail> GetBuildsFromOneBranch(
                        BuildDetail earliestBuild,
                        BuildDetail latestBuild,
                        string projectName,
                        TfsConnection collection)
        {

            var buildServer = collection.GetService<IBuildServer>();
            var searchSpec = buildServer.CreateBuildDetailSpec(projectName, earliestBuild.Definition);
            searchSpec.MinFinishTime = earliestBuild.Date;
            searchSpec.MaxFinishTime = latestBuild.Date;
            searchSpec.QueryDeletedOption = QueryDeletedOption.IncludeDeleted;

            var results = buildServer.QueryBuilds(searchSpec);
            return results.Builds;
        }

        private IEnumerable<WorkItem> GetWorkItemsForBuilds(
            IEnumerable<IBuildDetail> results,
            TfsConnection collection,
            string projectName)
        {
            var workItemService = collection.GetService<WorkItemStore>();
            var configuredCategory = workItemCategoryQuery.Execute();

            var category = workItemService.Projects[projectName].Categories
                .FirstOrDefault(c => c.ReferenceName.Equals(configuredCategory,
                                        StringComparison.InvariantCultureIgnoreCase));

            if (category == null)
            {
                throw new ArgumentOutOfRangeException("WorkItem.Category",
                    configuredCategory,
                    "The configured category name was not found");
            }

            IEnumerable<WorkItem> allWorkItems = new WorkItem[] {};

            foreach (var summaries in results.Select(InformationNodeConverters.GetAssociatedWorkItems))
            {
                var workItems = new Collection<WorkItem>();

                foreach (var wi in summaries
                            .Select(summary => workItemService.GetWorkItem(summary.WorkItemId))
                            .Where(wi => category.Contains(wi.Type)))
                {
                    workItems.Add(new WorkItem
                                      {
                                          Id = wi.Id.ToString(CultureInfo.InvariantCulture),
                                          Description = wi.Title,
                                          State = wi.State,
                                          Release = wi.Fields.Contains("TargetRelease") ? 
                                            wi.Fields["TargetRelease"].Value.ToString() : 
                                            string.Empty
                                      });
                }

                allWorkItems = allWorkItems.Union(workItems).ToArray();
            }

            return allWorkItems.OrderByDescending(wi => wi.Id).ToArray();
        }
    }

    public interface IBuildWorkItemQuery
    {
        IEnumerable<WorkItem> Execute(BuildDetail earlierBuildDetail, BuildDetail laterBuildDetail, Uri serverUri, string projectName);
    }
}