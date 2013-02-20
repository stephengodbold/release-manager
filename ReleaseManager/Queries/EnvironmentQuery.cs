using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net;
using Environment = ReleaseManager.Models.Environment;

namespace ReleaseManager.Queries
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
                var sessionState = InitialSessionState.CreateDefault();

                using (var shell = PowerShell.Create(sessionState))
                {
                    var parameters = new Dictionary<string, object>
                                         {
                                             {"Path", tempFilePath},
                                             {"Delimiter", ','}
                                         };
                    var command = shell.AddCommand("Import-Csv").AddParameters(parameters);
                    var results = command.Invoke();

                    foreach (dynamic result in results)
                    {
                        environment.LastReleaseDate = result.ReleaseDate;
                        environment.PreviousBuild = result.PreviousVersion;
                        environment.CurrentBuild = result.CurrentVersion;
                    }
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