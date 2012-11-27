namespace ReleaseManager.Models
{
    public class WorkItem
    {
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
