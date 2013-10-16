using System.Web.Http;
using ReleaseManager.Common;

namespace ReleaseManager.App_Start
{
    public class FormatterConfig
    {
        public static void RegisterFormatters(HttpConfiguration configuration)
        {
            configuration.Formatters.Add(new CsvFormatter());
        }
    }
}