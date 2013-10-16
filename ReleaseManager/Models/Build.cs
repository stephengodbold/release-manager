using System;

namespace ReleaseManager.Models
{
    public class Build
    {
        public string Uri { get; private set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Definition { get; set; }

        public Build(string uri)
        {
            Uri = uri;
        }
    }
}