using System.Collections.Generic;
using System.Linq;
using ReleaseManager.API.Models;

namespace ReleaseManager.API.Common
{
    public class WorkItemCsvFormatter : IWorkItemFormatter<string>
    {
        public string Format(IEnumerable<WorkItem> workItems)
        {
            return workItems
                .Aggregate(WorkItem.StringFormat, (current, next) =>
                    current += string.Format("{0}{1}", System.Environment.NewLine, next.ToString()));
        }
    }

    public interface IWorkItemFormatter<out T>
    {
        T Format(IEnumerable<WorkItem> workItems);
    }
}