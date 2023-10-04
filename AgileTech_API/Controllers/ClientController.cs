using AgileTech_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgileTech_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Client> GetClients()
        {
            return new List<Client> {
                new Client{Id=1, Name="Eduard Russy", Email="eduardrussy@gmail.com"},
                new Client{Id=2, Name="Juan Russy", Email="juan@gmail.com"},
                new Client{Id=3, Name="Dilan Russy", Email="dilan@gmail.com"}
            };
        }
    }
}
