using AgileTech_Utility;
using AgileTech_Web.Models;
using AgileTech_Web.Services.IServices;
using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AgileTech_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory _httpClient { get; set; }
        private string _hubspotToken;

        public BaseService(IHttpClientFactory httpClient, IConfiguration configuration)
        {
            this.responseModel = new();
            _httpClient = httpClient;
            _hubspotToken = configuration.GetValue<string>("ServiceUrls:HUBSPOT_TOKEN");
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {

            try
            {
                string json = JsonConvert.SerializeObject(apiRequest.Data, Formatting.Indented);
                
                var client = _httpClient.CreateClient("AgileTechAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);

                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                }
            

                switch (apiRequest.APIType)
                {
                    case DS.APIType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case DS.APIType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case DS.APIType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null;

                if (apiRequest.IsHubspot != null)
                {
                    
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _hubspotToken);
                    
                    apiResponse = await client.SendAsync(message);
                    var apiContent = await apiResponse.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<object>(apiContent);
                    //var result = JsonConvert.DeserializeObject<T>(apiContent);

                    var dto = new APIResponse
                    {
                        IsSuccess = true,
                        Result = result
                    };
                    var res = JsonConvert.SerializeObject(dto);
                    var APIResponse = JsonConvert.DeserializeObject<T>(res);
                    return APIResponse;

                }
                else
                {
                    apiResponse = await client.SendAsync(message);
                    var apiContent = await apiResponse.Content.ReadAsStringAsync();
                    var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return APIResponse;

                }

            }
            catch (Exception ex)
            {

                var dto = new APIResponse
                {
                    Errors = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;

            }
        }
    }
}
