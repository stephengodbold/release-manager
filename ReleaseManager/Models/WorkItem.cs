namespace ReleaseManager.Models
{
    public class WorkItem
    {
        private const string WorkItemStringFormat = "{0},{1},{2},{3}";

        public string Id { get; set; }
        public string Description { get; set; }
        public string Release { get; set; }
        public string State { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((WorkItem) obj);
        }

        public static string StringFormat { get { return "Id, Description,State,Release"; } }

        public override string ToString()
        {
            return string.Format(WorkItemStringFormat, new[] { Id, Description, State, Release });
        }

        protected bool Equals(WorkItem other)
        {
            return string.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}
