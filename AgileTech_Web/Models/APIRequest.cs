using static AgileTech_Utility.DS;

namespace AgileTech_Web.Models
{
    public class APIRequest
    {
        public APIType APIType { get; set; } = APIType.GET;

        public string Url { get; set; }

        public object Data { get; set; }

        public string IsHubspot { get; set; } = null;
    }
}
