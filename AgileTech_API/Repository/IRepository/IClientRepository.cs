using AgileTech_API.Models;

namespace AgileTech_API.Repository.IRepository
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client> Update(Client entity);
    }
}
