using System;
using System.Linq;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using ReleaseManager.Models;

namespace ReleaseManager.Queries
{
    public class BuildQuery : IBuildQuery
    {
        public BuildDetail Execute(Uri projectCollectionUri, string projectName, string buildName)
        {
            using (var collection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(projectCollectionUri))
            {
                var buildDetail = GetBuildDetail(collection, projectName, buildName);
                var changes = InformationNodeConverters.GetAssociatedChangesets(buildDetail);

                var branchRoot = string.Empty;
                var branchDate = DateTime.MinValue;
                var branchVersion = string.Empty;

                if (changes.Any())
                {
                    var versionControl = collection.GetService<VersionControlServer>();
                    var change = versionControl.GetChangesForChangeset(changes.First().ChangesetId, false, 1, null);
                    var branch = GetBranch(collection, change.First());

                    branchRoot = branch.Properties.RootItem.Item;
                    branchDate = branch.DateCreated;
                    branchVersion = branch.Properties.RootItem.Version.DisplayString;
                }

                return new BuildDetail
                           {
                               Name = buildName,
                               Definition = buildDetail.BuildDefinition.Name,
                               Date = buildDetail.FinishTime,
                               EarliestChangesetId = changes.Min(c => c.ChangesetId),
                               LatestChangsetId = changes.Max(c => c.ChangesetId),
                               BranchRoot = branchRoot,
                               BranchCreatedDate = branchDate,
                               BranchRootVersion = branchVersion
                           };
            }
        }

        private static BranchObject GetBranch(TfsConnection collection, Change change)
        {
            var versionControl = collection.GetService<VersionControlServer>();
            var serverPath = change.Item.ServerItem;
            
            while (!serverPath.Equals("$"))
            {
                var item = new ItemIdentifier(serverPath);
                var branch = versionControl.QueryBranchObjects(item, RecursionType.OneLevel);

                if (branch.Any())
                {
                    return branch.First();
                }

                serverPath = serverPath.Remove(serverPath.LastIndexOf('/'));
            }

            return null;
        }

        private static IBuildDetail GetBuildDetail(TfsConnection collection, string projectName, string buildName)
        {
            var buildServer = collection.GetService<IBuildServer>();
            var searchSpec = buildServer.CreateBuildDetailSpec(projectName, "*");
            searchSpec.BuildNumber = buildName;
            searchSpec.QueryDeletedOption = QueryDeletedOption.IncludeDeleted;
            var results = buildServer.QueryBuilds(searchSpec);
            
            return results.Builds.First();
        }
    }

    public interface IBuildQuery
    {
        BuildDetail Execute(Uri projectCollectionUri, string projectName, string buildName);
    }
}