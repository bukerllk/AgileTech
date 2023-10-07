using AgileTech_API.Data;
using AgileTech_API.Models;
using AgileTech_API.Repository.IRepository;

namespace AgileTech_API.Repository
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        private readonly ApplicatioDbContext _db;

        public ClientRepository(ApplicatioDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Client> Update(Client entity)
        {
            entity.Created = DateTime.Now;
            _db.Clients.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
