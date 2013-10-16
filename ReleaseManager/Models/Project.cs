using System.Collections.Generic;

namespace ReleaseManager.Models
{
    public class Project
    {
        public string Uri { get; private set; }
        public string Name { get; set; }
        public IEnumerable<Environment> Environments { get; set; }
        public Project(string uri)
        {
            Uri = uri;
        }
    }
}