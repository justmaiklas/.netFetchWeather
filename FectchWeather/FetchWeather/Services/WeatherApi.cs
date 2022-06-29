using System.Net;
using FetchWeather.Models;

namespace FetchWeather.Services
{
    public class WeatherApi
    {
        public async Task<HttpResponseMessage> GetCityWeather(string city)
        {
            const string baseUrl = "https://weather-api.isun.ch/api/weathers/";
            const string authorizationToken = "Bearer aaa4b0147521e4daadf0929e858b4c71aaa4b0147521e4daadf0929e858b4c71";
            var finalUrl = baseUrl + city;
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "text/plain");
                client.DefaultRequestHeaders.Add("Authorization", authorizationToken);
                var response = await client.GetAsync(finalUrl);
                
                return response;

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
