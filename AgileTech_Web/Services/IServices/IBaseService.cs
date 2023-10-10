using AgileTech_Web.Models;

namespace AgileTech_Web.Services.IServices
{
    public interface IBaseService
    {
        public APIResponse responseModel { get; set; }

        Task<T> SendAsync<T>(APIRequest apiRequest); 
    }
}
