using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReleaseManager.Models;

namespace ReleaseManager.Services
{
    public interface ICsvFormatter
    {
        string EncodeAsCsv(IEnumerable<WorkItem> items);
    }

    public class CsvFormatter : ICsvFormatter
    {
        private const string HeaderRow = "\"Id\",\"Description\",\"State\"";
        private const string CsvFormat = "{0},{1},{2}";

        public string EncodeAsCsv(IEnumerable<WorkItem> items)
        {
            var csv = items.Aggregate(new StringBuilder(),
                    (s, item) => s.AppendFormat(CsvFormat,
                                new[]
                                    {
                                        item.Id, 
                                        item.Description, 
                                        item.State
                                })
                        .Append(System.Environment.NewLine));

            csv.Insert(0, System.Environment.NewLine);
            csv.Insert(0, HeaderRow);

            return csv.ToString();
        }
    }
}