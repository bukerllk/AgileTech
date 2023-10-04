using System.Security.Principal;

namespace AgileTech_API.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
    }
}
