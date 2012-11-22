using System;
using Environment = ReleaseManager.Models.Environment;

namespace ReleaseManager.Queries
{
    public interface IEnvironmentQuery
    {
        Environment GetEnvironmentDetails(Uri rootUrl);}
}