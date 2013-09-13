using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using ReleaseManager.API.Common;
using ReleaseManager.API.Models;

namespace ReleaseManager.API.Queries
{
    public class BuildsForDateQuery : IBuildsForDateQuery
    {
        public IEnumerable<Build> Execute(DateTime buildDate, Uri serverUri, string projectName)
        {
            using (var collection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(serverUri))
            {
                var buildService = collection.GetService<IBuildServer>();
                var searchSpec = buildService.CreateBuildDetailSpec(projectName, "*");
                searchSpec.MaxFinishTime = buildDate.LastSecondOfDay();
                searchSpec.MinFinishTime = buildDate.FirstSecondOfDay();
                searchSpec.QueryDeletedOption = QueryDeletedOption.IncludeDeleted;
                var results = buildService.QueryBuilds(searchSpec);

                return results.Builds.Select(build =>
                    new Build
                        {
                            Name = build.BuildNumber,
                            Date = build.StartTime,
                            Definition = build.BuildDefinition.Name,
                            Result = build.Status.ToString()
                        });
            }
        }
    }

    public interface IBuildsForDateQuery
    {
        IEnumerable<Build> Execute(DateTime buildDate, Uri serverUri, string projectName);
    }
}