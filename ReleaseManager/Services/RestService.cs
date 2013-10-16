using System.Collections.Generic;
using System.Net;
using System.Web;
using RestSharp;

namespace ReleaseManager.Services
{
    public class RestService : IRestService
    {
        private readonly IConfigurationService _configurationService;

        public RestService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public T GetModel<T>(string resource, Dictionary<string,string> parameters = null) 
            where T : new()
        {
            var rootUri = _configurationService.GetRootUri();
            var demoMode = _configurationService.GetMode();

            var client = new RestClient(rootUri);
            var request = new RestRequest(resource, Method.GET);
            request.AddHeader("x-api-mode", demoMode);

            AddRequestUrlParameters(request, parameters);

            var response = client.Execute<T>(request);
            if ((response.ErrorException != null) || 
                (!string.IsNullOrWhiteSpace(response.ErrorMessage)))
            {
                throw new WebException(response.ErrorMessage, response.ErrorException);
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpException((int) response.StatusCode, response.StatusDescription);
            }

            return response.Data;
        }

        private void AddRequestUrlParameters(RestRequest request, Dictionary<string, string> parameters)
        {
            if ((parameters == null) || (parameters.Count == 0))
                return;

            foreach (var param in parameters)
            {
                if (!string.IsNullOrWhiteSpace(param.Value))
                {
                    request.AddParameter(param.Key, param.Value, ParameterType.UrlSegment);
                }
            }
        }
    }

    public interface IRestService
    {
        T GetModel<T>(string resource, Dictionary<string, string> parameters = null)
            where T : new();
    }
}