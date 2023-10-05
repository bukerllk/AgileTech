
using System.ComponentModel.DataAnnotations;

namespace AgileTech_API.Models.Dto
{
    public class ClientDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
