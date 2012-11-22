using System;
using System.Net;
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
            var parser = new TextFieldParser(contentStream);

            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            var environment = new Environment { Name= rootUrl.Host };

            while(!parser.EndOfData)
            {
                var fields = parser.ReadLine();

                environment.LastReleaseDate = fields[0].ToString();
                environment.CurrentBuild = fields[1].ToString();
                environment.PreviousBuild = fields[2].ToString();
            }

            return environment;
        }
    }
}