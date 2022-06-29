using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FetchWeather.Models
{
    public class CityWeather
    {
        public Guid Id { get; set; }
        public string? City { get; set; }
        public string? Temperature { get; set; }
        public string? WindSpeed { get; set; }
        public string? Summary { get; set; }
        public DateTime? DateCreated { get; set; }

        public CityWeather()
        {
            DateCreated = DateTime.Now;
        }
    }
}
