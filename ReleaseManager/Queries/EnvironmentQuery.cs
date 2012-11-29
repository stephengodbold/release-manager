using System;
using System.IO;
using System.Net;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
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
            Stream contentStream;

            try
            {
                contentStream = client.OpenRead(requestPath);
            } catch (WebException)
            {
                return environment;
            }

            using (var parser = new TextFieldParser(contentStream))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.CommentTokens = new[] {"#"};

                while (!parser.EndOfData)
                {
                    var fields = parser.ReadFields();

                    if (fields.Count() != 3) continue;

                    environment.LastReleaseDate = fields[0];
                    environment.CurrentBuild = fields[1];
                    environment.PreviousBuild = fields[2];
                }

                return environment;
            }
        }
    }
    
    public interface IEnvironmentQuery
    {
        Environment Execute(Uri rootUri);
    }
}