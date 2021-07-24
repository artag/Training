using System;
using Newtonsoft.Json;

namespace CoronavirusApiService.ApiModels
{
    public class Location
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("country_population")]
        public int? CountryPopulation { get; set; }

        [JsonProperty("province")]
        public string Province { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("coordinates")]
        public Coordinates Coordinates { get; set; }

        [JsonProperty("latest")]
        public Latest Latest { get; set; }
    }
}