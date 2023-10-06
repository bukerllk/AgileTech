using AgileTech_API.Data;
using AgileTech_API.Models;
using AgileTech_API.Models.Dto;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public ClientController(ILogger<ClientController> logger, ApplicatioDbContext db, IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetClients()
        {
            _logger.LogInformation("Get clients list");
            IEnumerable<Client> clientList = await _db.Clients.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<Client>>(clientList));
        }

        [HttpGet("id:int", Name = "GetClientById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClientDto>> GetClientById(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error - the id "+ id +" is not valid.");
                return BadRequest();
            }

            //var client = ClientStore.clientList.FirstOrDefault(c => c.Id == id);
            var client = await _db.Clients.FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                _logger.LogError("Error - not found client with the " + id + " id.");
                return NotFound();
            }
            return Ok(_mapper.Map<ClientDto>(client));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientDto>> CreateClient([FromBody] ClientCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(createDto);
            }

            if (await _db.Clients.FirstOrDefaultAsync(c => c.Email.ToLower() == createDto.Email.ToLower()) != null)
            {
                ModelState.AddModelError("EmailExists", "The email " + createDto.Email + " already exists.");
                return BadRequest(ModelState);
            }

            if (!await _db.Clients.AnyAsync()) {
                return BadRequest(createDto);
            }

            if (createDto == null)
            {
                return BadRequest(createDto);
            }

            Client model = _mapper.Map<Client>(createDto);

            await _db.Clients.AddAsync(model);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetClientById", new { id = model.Id }, model);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteClient(int id) 
        { 
            if (id == 0) 
            { 
                return BadRequest();
            }

            var client = await _db.Clients.FirstOrDefaultAsync(c => c.Id == id);

            if (client == null) 
            {
                return NotFound();
            }

            _db.Clients.Remove(client);
            await _db.SaveChangesAsync();

            return NoContent();


        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] ClientUpdateDto updateDto) 
        {
            if (updateDto == null || id != updateDto.Id) 
            {
                return BadRequest();
            }

            Client model = _mapper.Map<Client>(updateDto);

            _db.Clients.Update(model);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialClient(int id, JsonPatchDocument<ClientUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var client = await _db.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            if (client == null) return NotFound();

            ClientUpdateDto clientDto = _mapper.Map<ClientUpdateDto>(client);


            patchDto.ApplyTo(clientDto, ModelState);

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            Client model = _mapper.Map<Client>(clientDto);


            _db.Clients.Update(model);
            await _db.SaveChangesAsync();

            return NoContent();
        }

    }
}
