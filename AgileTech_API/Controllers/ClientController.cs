using AgileTech_API.Data;
using AgileTech_API.Models;
using AgileTech_API.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace AgileTech_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        public ClientController(ILogger<ClientController> logger)
        {
            _logger = logger; 
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ClientDto>> GetClients()
        {
            _logger.LogInformation("Get clients list");
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
                _logger.LogError("Error - the id "+ id +" is not valid.");
                return BadRequest();
            }

            var client = ClientStore.clientList.FirstOrDefault(c => c.Id == id);

            if (client == null)
            {
                _logger.LogError("Error - not found client with the " + id + " id.");
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
            if (!ModelState.IsValid)
            {
                return BadRequest(clientDto);
            }

            if (ClientStore.clientList.FirstOrDefault(c => c.Email.ToLower() == clientDto.Email.ToLower()) != null)
            {
                ModelState.AddModelError("EmailExists", "The email " + clientDto.Email + " already exists.");
                return BadRequest(ModelState);
            }

            if (!ClientStore.clientList.Any()) {
                return BadRequest(clientDto);
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

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteClient(int id) 
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

            ClientStore.clientList.Remove(client);

            return NoContent();


        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateClient(int id, [FromBody] ClientDto clientDto) 
        {
            if (clientDto == null || id != clientDto.Id) 
            {
                return BadRequest();
            }

            var client = ClientStore.clientList.FirstOrDefault(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            client.Name = clientDto.Name;
            client.Email = clientDto.Email;
  
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialClient(int id, JsonPatchDocument<ClientDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var client = ClientStore.clientList.FirstOrDefault(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            patchDto.ApplyTo(client, ModelState);

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }

    }
}
