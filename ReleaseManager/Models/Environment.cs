namespace ReleaseManager.Models
{
    public class Environment
    {
        public string Uri { get; private set; }
        public string Name { get; set; }
        public string CurrentBuild { get; set; }
        public string PreviousBuild { get; set; }
        public string LastReleaseDate { get; set; }

        public Environment(string uri)
        {
            Uri = uri;
        }
    }
}