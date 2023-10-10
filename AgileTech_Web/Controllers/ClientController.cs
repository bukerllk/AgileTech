using AgileTech_Web.Models;
using AgileTech_Web.Models.Dto;
using AgileTech_Web.Services.IServices;
using AutoMapper;
using AutoMapper.Internal;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Reflection;


namespace AgileTech_Web.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public ClientController(IClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }
        public async Task<IActionResult> IndexClient()
        {

            List<ClientDto> clientList = new();

            var response = await _clientService.GetAll<APIResponse>();

            if (response != null && response.IsSuccess) 
            {
                clientList = JsonConvert.DeserializeObject<List<ClientDto>>(Convert.ToString(response.Result));
            }
            return View(clientList);

        }

        public async Task<IActionResult> HubspotClient()
        {

            List<ClientHubspotDto> clientList = new();

            var responseHubspot = await _clientService.GetAllHubspot<APIResponse>();

            Console.WriteLine(string.Join("----sebas, ", responseHubspot.Result));

            //Console.WriteLine("pajaro" + responseHubspot );

 
            if (responseHubspot != null)
            {
              clientList = JsonConvert.DeserializeObject<List<ClientHubspotDto>>(Convert.ToString(responseHubspot.Result));
            }
            return NoContent();
            //return View(clientList);
        }

        public async Task<IActionResult> SendTo(int id) 
        {

            ClientDto clientDto = new();
            var responseApi = await _clientService.Get<APIResponse>(id);
            if (responseApi != null && responseApi.IsSuccess)
            {
                clientDto = JsonConvert.DeserializeObject<ClientDto>(Convert.ToString(responseApi.Result));

                var data = new ClientCreateHubspotDto
                {
                    properties = new
                    {
                        email = clientDto.Email,
                        firstname = clientDto.Name,
                        idapi = clientDto.Id
                    }
                };

                var response = await _clientService.CreateHubspot<APIResponse>(data);
                if (response != null && response.IsSuccess)
                {
                   return RedirectToAction(nameof(HubspotClient));
                }

            }
            return NotFound();
        }
    }
}
