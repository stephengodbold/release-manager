using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReleaseManager.Queries
{
    public class WorkItemCategoryQuery : IWorkItemCategoryQuery
    {
        public string Execute()
        {
            return "Microsoft.RequirementCategory";
        }
    }

    public interface IWorkItemCategoryQuery
    {
        string Execute();
    }
}