using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Net;
using Environment = ReleaseManager.API.Models.Environment;

namespace ReleaseManager.API.Queries
{
    public class EnvironmentQuery : IEnvironmentQuery
    {
        
        public Environment Execute(Uri rootUri)
        {
            var requestPath = new Uri(rootUri, "version.csv");
            var client = new WebClient();
            var environment = new Environment { Name = rootUri.Host };
            var tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());

            try
            {
                client.DownloadFile(requestPath, tempFilePath);

                using (var shell = PowerShell.Create())
                {
                    var parameters = new Dictionary<string, object>
                                         {
                                             {"Path", tempFilePath},
                                             {"Delimiter", ','}
                                         };

                    var command = shell.AddCommand("Import-Csv").AddParameters(parameters);
                    var results = command.Invoke();

                    var resultsFirst = results[0];

                    environment.LastReleaseDate = resultsFirst.Properties["ReleaseDate"].Value.ToString();
                    environment.PreviousBuild = resultsFirst.Properties["PreviousVersion"].Value.ToString();
                    environment.CurrentBuild = resultsFirst.Properties["CurrentVersion"].Value.ToString();
                }

                return environment;
            } 
            catch (WebException)
            {
                return environment;
            }
            finally
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }
    }
    
    public interface IEnvironmentQuery
    {
        Environment Execute(Uri rootUri);
    }
}