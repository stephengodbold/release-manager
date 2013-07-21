using System.Configuration;

namespace ReleaseManager.API.Queries
{
    public class WorkItemCategoryQuery : IWorkItemCategoryQuery
    {
        public string Execute()
        {
            var configuration = new AppSettingsReader();
            var value = configuration.GetValue("WorkItem.Category", typeof(string));

            return value == null ? string.Empty : value.ToString();
        }
    }

    public interface IWorkItemCategoryQuery
    {
        string Execute();
    }
}