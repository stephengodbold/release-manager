using System;

namespace ReleaseManager.Models
{
    public class WorkItem
    {
        public string Uri { get; private set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public string Release { get; set; }
        public string State { get; set; }

        public WorkItem(string uri)
        {
            Uri = uri;
        }
    }
}
