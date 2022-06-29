using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using FetchWeather.Models;
using FetchWeather.Services;
using FetchWeather.SQLite;

namespace FetchWeather;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine(args.Length);
        var citiesArgumentFound = false;
        var _weatherApi = new WeatherApi();
        DatabaseContext dbContext= new DatabaseContext();
        Console.WriteLine($"Database path: {dbContext.DbPath}.");
        foreach (var argument in args)
        {
            if (argument.StartsWith("--") && argument.EndsWith("cities") && !citiesArgumentFound)
            {
                citiesArgumentFound = true;
                continue;
            }
            if (!citiesArgumentFound) continue;
            var city = argument.EndsWith(",") ? argument.Replace(",", "") : argument;
            Console.WriteLine($"Getting {city} weather info..");
            var responseMessage = await _weatherApi.GetCityWeather(city);
            if (responseMessage.StatusCode != HttpStatusCode.OK && responseMessage.StatusCode != HttpStatusCode.InternalServerError)
            {
                Console.WriteLine($"Error with \"{city}\". Check city spelling. Status code: {responseMessage.StatusCode}");
                continue;
            }
            Console.WriteLine($"Got {city} weather info. Status code: {responseMessage.StatusCode}");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var weatherInfo = JsonSerializer.Deserialize<CityWeather>(jsonData);
            if (weatherInfo == null)
            {
                Console.WriteLine($"Error while parsing JSON info for \"{city}\". Json data: {jsonData}");
                continue;
            }
            Console.WriteLine($"Weather in {weatherInfo.City} city. Temperature: {weatherInfo.Temperature}. Wind speed: {weatherInfo.WindSpeed}. Summary: {weatherInfo.Summary}");
            //todo: Implement saving to database;
            dbContext.CityWeather.Add(weatherInfo);
            await dbContext.SaveChangesAsync();

            Console.WriteLine("Done saving to db");

        }

        if (!citiesArgumentFound)
        {
            throw new Exception("--cities argument not found");
        }
        

    }
}