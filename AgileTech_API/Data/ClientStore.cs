using AgileTech_API.Models.Dto;

namespace AgileTech_API.Data
{
    public static class ClientStore
    {
        public static List<ClientDto> clientList = new List<ClientDto> 
        {
            new ClientDto{Id=1, Name="Alis navarrete", Email="alis@gmail.com"},
            new ClientDto{Id=2, Name="Rafael Hernandez", Email="rafa@gmail.com"}
        };
    }
}
