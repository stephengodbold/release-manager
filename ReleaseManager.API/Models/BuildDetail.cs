using System;

namespace ReleaseManager.API.Models
{
    public class BuildDetail
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Definition { get; set; }
        public int EarliestChangesetId { get; set; }
        public int LatestChangsetId { get; set; }

        public string BranchRoot { get; set; }
        public string BranchRootVersion { get; set; }
        public DateTime BranchCreatedDate { get; set; }
    }
}