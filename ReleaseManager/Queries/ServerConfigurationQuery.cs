using System.Collections.Generic;

namespace ReleaseManager.Queries
{
    public class ServerConfigurationQuery : IServerConfigurationQuery
    {
        public IDictionary<string, string> Execute()
        {
            return new Dictionary<string, string>
                       {
                           {"TeamFoundation", "http://tfs.studygroup.com/tfs/StudyGroup"},
                           {"ProjectName", "StudyGlobalDev" }
                       };
        }
    }

    public interface IServerConfigurationQuery
    {
        IDictionary<string, string> Execute();
    }
}