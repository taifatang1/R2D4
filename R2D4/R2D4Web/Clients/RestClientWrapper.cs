using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using GlobalResources;
using R2D4Web.Clients.Models;
using RestSharp;

namespace R2D4Web.Clients
{
    public class RestRequestBuilder
    {
        public IRestRequest Request = new RestRequest();
        public RestRequestBuilder Configure(string resource, Method method)
        {
            Request.Resource = resource;
            Request.Method = method;
            return this;
        }
        public RestRequestBuilder AddHeader(string key, string value)
        {
            Request.AddHeader(key, value);
            return this;
        }
        public RestRequestBuilder AddQueryString(string key, string value)
        {
            Request.AddQueryParameter(key, value);
            return this;
        }
    }
    public class RestClientWrapper : IRestClientWrapper
    {
        private readonly IRestClient _client;
        private readonly IRestRequestValidator _validator;

        public RestClientWrapper(string apiBaseUrl, IRestRequestValidator validator)
        {
            _validator = validator;
            _client = new RestClient(apiBaseUrl);
        }
        public async Task<IRestResponse> ExecuteAsync(RestRequest request)
        {
            if (_validator.Validate(request).IsValid)
            {
                return await _client.ExecuteTaskAsync(request);
            }
            else
            {
                var response = new RestResponse { ErrorMessage = _validator.Validate(request).ToString() };
                return response;
            }
        }
        public async Task<IRestResponse<T>> ExecuteAsync<T>(RestRequest request)
        {
            if (_validator.Validate(request).IsValid)
            {
                return await _client.ExecuteTaskAsync<T>(request);
            }
            else
            {
                throw new ArgumentNullException(nameof(request), _validator.Validate(request).ToString());
            }
        }
    }

    public interface IRestRequestValidator
    {
        ExecutionModel Validate(RestRequest request);
    }

    public class BasicRestRequestValidator : IRestRequestValidator
    {
        public ExecutionModel Validate(RestRequest request)
        {
            var executionModel = new ExecutionModel();

            if (String.IsNullOrEmpty(request.Resource))
            {
                executionModel.ErrorMessages.Add(ErrorMessage.ResourceIsMissing);
            }

            if (request.Parameters.All(x => x.Type != ParameterType.HttpHeader))
            {
                executionModel.ErrorMessages.Add(ErrorMessage.HeaderIsMissing);
            }

            if (executionModel.ErrorMessages.Count == 0)
            {
                executionModel.IsValid = true;
            }

            return executionModel;
        }
    }
}