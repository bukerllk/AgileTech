using AgileTech_API.Models.Dto;

namespace AgileTech_API.Data
{
    public static class ClientStore
    {
        public static List<ClientDto> clientList = new List<ClientDto> 
        {
            new ClientDto{Id=1, Name="Alis navarrete", Email="alis@gmail.com"},
            new ClientDto{Id=2, Name="Rafael Hernandez", Email="rafa@gmail.com"},
            new ClientDto{Id=3, Name="Eduard Russy", Email="eduardrussy@gmail.com"},
            new ClientDto{Id=4, Name="Juan Russy", Email="juan@gmail.com"},
            new ClientDto{Id=5, Name="Dilan Russy", Email="dilan@gmail.com"}
        };
    }
}
