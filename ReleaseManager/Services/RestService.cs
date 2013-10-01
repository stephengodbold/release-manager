using System.Collections.Generic;
using System.Net;
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

            AddRequestParameters(request, parameters);

            var response = client.Execute<T>(request);
            if ((response.ErrorException != null) || (!string.IsNullOrWhiteSpace(response.ErrorMessage)))
            {
                throw new WebException(response.ErrorMessage, response.ErrorException);
            }

            return response.Data;
        }

        private void AddRequestParameters(RestRequest request, Dictionary<string, string> parameters)
        {
            if ((parameters == null) || (parameters.Count == 0))
                return;

            foreach (var param in parameters)
            {
                request.AddParameter(param.Key, param.Value);
            }
        }
    }

    public interface IRestService
    {
        T GetModel<T>(string resource, Dictionary<string, string> parameters = null)
            where T : new();
    }
}