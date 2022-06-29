using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FetchWeather.Models
{
    public class CityWeather
    {
        public Guid Id { get; set; }
        [JsonPropertyName("city")]
        public string? City { get; set; }
        [JsonPropertyName("temperature")]
        public int Temperature { get; set; }
        [JsonPropertyName("windSpeed")]
        public double WindSpeed { get; set; }
        [JsonPropertyName("summary")]
        public string? Summary { get; set; }
        public DateTime? DateCreated { get; set; }

        public CityWeather()
        {
            DateCreated = DateTime.Now;
        }
    }
}
