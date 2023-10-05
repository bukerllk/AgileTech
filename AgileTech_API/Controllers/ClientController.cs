using AgileTech_API.Data;
using AgileTech_API.Models;
using AgileTech_API.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgileTech_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<ClientDto> GetClients()
        {
            return ClientStore.clientList;
            /*return new List<ClientDto> {
                new ClientDto{Id=1, Name="Eduard Russy", Email="eduardrussy@gmail.com"},
                new ClientDto{Id=2, Name="Juan Russy", Email="juan@gmail.com"},
                new ClientDto{Id=3, Name="Dilan Russy", Email="dilan@gmail.com"}
            };*/
        }
    }
}
