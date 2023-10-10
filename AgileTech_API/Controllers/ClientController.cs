using AgileTech_API.Data;
using AgileTech_API.Models;
using AgileTech_API.Models.Dto;
using AgileTech_API.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AgileTech_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IClientRepository _clientRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public ClientController(ILogger<ClientController> logger, IClientRepository clientRepo, IMapper mapper)
        {
            _logger = logger;
            _clientRepo = clientRepo;
            _mapper = mapper;
            _response = new();

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetClients()
        {
            try
            {
                _logger.LogInformation("Get clients list");

                IEnumerable<Client> clientList = await _clientRepo.GetAll();

                _response.Result = _mapper.Map<IEnumerable<Client>>(clientList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
               
           
        }

        [HttpGet("{id:int}", Name = "GetClientById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetClientById(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error - the id " + id + " is not valid.");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                //var client = ClientStore.clientList.FirstOrDefault(c => c.Id == id);
                var client = await _clientRepo.Get(c => c.Id == id);

                if (client == null)
                {
                    _logger.LogError("Error - not found client with the " + id + " id.");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ClientDto>(client);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
            
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateClient([FromBody] ClientCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _clientRepo.Get(c => c.Email.ToLower() == createDto.Email.ToLower()) != null)
                {
                    ModelState.AddModelError("EmailExists", "The email " + createDto.Email + " already exists.");
                    return BadRequest(ModelState);
                }


                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                Client model = _mapper.Map<Client>(createDto);

                model.Created = DateTime.Now;
                await _clientRepo.Create(model);

                _response.Result = model;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetClientById", new { id = model.Id }, _response);

            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteClient(int id) 
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var client = await _clientRepo.Get(c => c.Id == id);

                if (client == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _clientRepo.Delete(client);

                _response.StatusCode=HttpStatusCode.NoContent;
                return Ok(_response);

            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);


        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] ClientUpdateDto updateDto) 
        {
            try
            {
                if (updateDto == null || id != updateDto.Id)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                Client model = _mapper.Map<Client>(updateDto);

                await _clientRepo.Update(model);

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);

            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);


        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialClient(int id, JsonPatchDocument<ClientUpdateDto> patchDto)
        {
            try
            {
                if (patchDto == null || id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var client = await _clientRepo.Get(c => c.Id == id, traked: false);

                if (client == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response); 
                }

                ClientUpdateDto clientDto = _mapper.Map<ClientUpdateDto>(client);


                patchDto.ApplyTo(clientDto, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Client model = _mapper.Map<Client>(clientDto);


                await _clientRepo.Update(model);

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);

            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
           
        }

    }
}
