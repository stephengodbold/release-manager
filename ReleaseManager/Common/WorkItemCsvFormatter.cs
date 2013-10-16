using System.Collections.Generic;
using System.Linq;
using ReleaseManager.Models;

namespace ReleaseManager.Common
{
    public class WorkItemCsvFormatter : IWorkItemFormatter<string>
    {
        public string Format(IEnumerable<WorkItem> workItems)
        {
            return string.Empty;
            //workItems
            //.Aggregate(WorkItem.StringFormat, (current, next) =>
            //    current += string.Format("{0}{1}", System.Environment.NewLine, next.ToString()));
        }
    }

    public interface IWorkItemFormatter<out T>
    {
        T Format(IEnumerable<WorkItem> workItems);
    }
}