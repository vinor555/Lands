using Newtonsoft.Json;

namespace Lands.Models
{
    public class Currency
    {
        [JsonProperty(PropertyName = "code")]//porque las propiedades deben ir con mayuscula en c# es para mantener estandares
        public string Code { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

    }
}
