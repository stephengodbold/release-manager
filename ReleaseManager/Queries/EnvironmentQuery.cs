using System;
using System.Net;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using Environment = ReleaseManager.Models.Environment;

namespace ReleaseManager.Queries
{
    public class EnvironmentQuery : IEnvironmentQuery
    {
        public Environment GetEnvironmentDetails(Uri rootUrl)
        {
            var requestPath = new Uri(rootUrl, "version.csv");
            var client = new WebClient();

            var contentStream = client.OpenRead(requestPath);
            using (var parser = new TextFieldParser(contentStream))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.CommentTokens = new[] {"#"};

                var environment = new Environment {Name = rootUrl.Host};

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
}