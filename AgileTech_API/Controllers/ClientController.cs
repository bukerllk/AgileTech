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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ClientDto>> GetClients()
        {
            return Ok(ClientStore.clientList);
        }

        [HttpGet("id:int", Name = "GetClientById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ClientDto> GetClientById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var client = ClientStore.clientList.FirstOrDefault(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ClientDto> CreateClient([FromBody] ClientDto clientDto) 
        {
            if (!ClientStore.clientList.Any()) { 
                return BadRequest();
            }

            if (clientDto == null)
            {
                return BadRequest(clientDto);
            }

            if (clientDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            clientDto.Id = ClientStore.clientList.OrderByDescending(c => c.Id).FirstOrDefault().Id + 1;
            ClientStore.clientList.Add(clientDto);

            return CreatedAtRoute("GetClientById", new { id = clientDto.Id }, clientDto);
        }
        

    }
}
