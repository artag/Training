using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoronavirusApiService.ApiModels
{
    public class ApiData
    {
        [JsonProperty("latest")]
        public Latest Latest { get; set; }

        [JsonProperty("locations")]
        public List<Location> Locations { get; set; }
    }
}