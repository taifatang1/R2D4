using System.Threading.Tasks;
using R2D4Web.Clients.Models;
using RestSharp;

namespace R2D4Web.Clients
{
    public interface IRestClientWrapper
    {
        Task<IRestResponse> ExecuteAsync(RestRequest request);
        Task<IRestResponse<T>> ExecuteAsync<T>(RestRequest request);
    }
}