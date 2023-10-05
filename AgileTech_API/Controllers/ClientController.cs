using AgileTech_API.Data;
using AgileTech_API.Models;
using AgileTech_API.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgileTech_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly ApplicatioDbContext _db;
        public ClientController(ILogger<ClientController> logger, ApplicatioDbContext db)
        {
            _logger = logger;
            _db = db;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ClientDto>> GetClients()
        {
            _logger.LogInformation("Get clients list");
            return Ok(_db.Clients.ToList());
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

            //var client = ClientStore.clientList.FirstOrDefault(c => c.Id == id);
            var client = _db.Clients.FirstOrDefault(c => c.Id == id);

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

            if (_db.Clients.FirstOrDefault(c => c.Email.ToLower() == clientDto.Email.ToLower()) != null)
            {
                ModelState.AddModelError("EmailExists", "The email " + clientDto.Email + " already exists.");
                return BadRequest(ModelState);
            }

            if (!_db.Clients.Any()) {
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

            Client model = new()
            {
                Name = clientDto.Name,
                Email = clientDto.Email,
                Created = DateTime.Now
            };

            _db.Clients.Add(model);
            _db.SaveChanges();

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

            var client = _db.Clients.FirstOrDefault(c => c.Id == id);

            if (client == null) 
            {
                return NotFound();
            }

            _db.Clients.Remove(client);
            _db.SaveChanges();

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

            Client model = new()
            {
                Id = clientDto.Id,
                Name = clientDto.Name,
                Email = clientDto.Email,
                Created = DateTime.Now
            };

            _db.Clients.Update(model);
            _db.SaveChanges();

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

            var client = _db.Clients.AsNoTracking().FirstOrDefault (c => c.Id == id);

            if (client == null) return NotFound();

            ClientDto clientDto = new()
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email
            };

            patchDto.ApplyTo(clientDto, ModelState);

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            Client model = new()
            {
                Id = clientDto.Id,
                Name = clientDto.Name,
                Email = clientDto.Email,
                Created = DateTime.Now
            };

            _db.Clients.Update(model);
            _db.SaveChanges();

            return NoContent();
        }

    }
}
