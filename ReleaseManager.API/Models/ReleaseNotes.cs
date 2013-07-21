using System.Collections.Generic;

namespace ReleaseManager.API.Models
{
    public class ReleaseNotes
    {
        public string Title { get; set; }
        public string CurrentRelease { get; set; }
        public string PreviousRelease { get; set; }
        public IEnumerable<WorkItem> Items { get; set; }
        public IEnumerable<string> States { get; set; }
        public string CsvItems { get; set; }
    }
}