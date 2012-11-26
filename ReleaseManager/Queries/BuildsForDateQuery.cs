using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using ReleaseManager.Common;

namespace ReleaseManager.Queries
{
    public class BuildsForDateQuery : IBuildsForDateQuery
    {
        public IEnumerable<string> Execute(DateTime buildDate, Uri serverUri, string projectName)
        {
            using (var collection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(serverUri))
            {
                var buildService = collection.GetService<IBuildServer>();
                var searchSpec = buildService.CreateBuildDetailSpec(projectName, "*");
                searchSpec.MaxFinishTime = buildDate.LastSecondOfDay();
                searchSpec.MinFinishTime = buildDate.FirstSecondOfDay();
                searchSpec.QueryDeletedOption = QueryDeletedOption.IncludeDeleted;
                var results = buildService.QueryBuilds(searchSpec);

                return results.Builds.Select(build => build.BuildNumber);
            }
        }
    }

    public interface IBuildsForDateQuery
    {
        IEnumerable<string> Execute(DateTime buildDate, Uri serverUri, string projectName);
    }
}