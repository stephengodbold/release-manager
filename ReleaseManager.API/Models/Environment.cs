namespace ReleaseManager.API.Models
{
    public class Environment
    {
        public string Name { get; set; }
        public string CurrentBuild { get; set; }
        public string PreviousBuild { get; set; }
        public string LastReleaseDate { get; set; }
    }
}