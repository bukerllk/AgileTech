using AgileTech_Web.Models.Dto;

namespace AgileTech_Web.Services.IServices
{
    public interface IClientService
    {
        Task<T> GetAll<T>();
        Task<T> Get<T>(int id);
        Task<T> Create<T>(ClientCreateDto dto);
        Task<T> Update<T>(ClientUpdateDto dto);
        Task<T> Delete<T>(int id);
        Task<T> GetAllHubspot<T>();
        Task<T> CreateHubspot<T>(ClientCreateHubspotDto dto);
    }
}
