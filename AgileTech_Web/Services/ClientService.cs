using AgileTech_Utility;
using AgileTech_Web.Models;
using AgileTech_Web.Models.Dto;
using AgileTech_Web.Services.IServices;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;

namespace AgileTech_Web.Services
{
    public class ClientService : BaseService, IClientService
    {
        public readonly IHttpClientFactory _httpClient;
        private string _clientUrl;
        private string _hubspotUrl;


        public ClientService(IHttpClientFactory httpClient, IConfiguration configuration) :base(httpClient, configuration)
        {
            _httpClient = httpClient;
            _clientUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
            _hubspotUrl = configuration.GetValue<string>("ServiceUrls:HUBSPOT_URL");
        }

        public Task<T> Get<T>(int id)
        {
            Console.WriteLine(_clientUrl + "/api/client/" + id);
            return SendAsync<T>(new APIRequest()
            {
                APIType = DS.APIType.GET,
                Url = _clientUrl + "/api/client/" + id
            });
        }

        public Task<T> GetAll<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = DS.APIType.GET,
                Url = _clientUrl + "/api/Client"
            });
        }

        public Task<T> Create<T>(ClientCreateDto dto)
        {
            return SendAsync<T>(new APIRequest() 
            { 
                APIType = DS.APIType.POST,
                Data = dto,
                Url = _clientUrl + "/api/client"
            });
        }

        public Task<T> Update<T>(ClientUpdateDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = DS.APIType.PUT,
                Data = dto,
                Url = _clientUrl + "/api/client/" + dto.Id
            });
        }

        public Task<T> Delete<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = DS.APIType.DELETE,
                Url = _clientUrl + "/api/client/" + id
            });
        }

        public Task<T> GetAllHubspot<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = DS.APIType.GET,
                Url = _hubspotUrl + "/contacts?limit=10&archived=false",
                IsHubspot = "yes"
            });
        }

        public Task<T> CreateHubspot<T>(ClientCreateHubspotDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = DS.APIType.POST,
                Data = dto,
                Url = _clientUrl = _hubspotUrl + "/contacts",
                IsHubspot = "yes"
            });

        }
    }
}
