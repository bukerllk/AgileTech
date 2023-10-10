
using System.ComponentModel.DataAnnotations;

namespace AgileTech_Web.Models.Dto
{
    public class ClientCreateHubspotDto
    {
        [Required]
        public object properties { get; set; }


    }
}
