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
        foreach (var argument in args)
        {
            if (argument.StartsWith("--") && argument.EndsWith("cities") && !citiesArgumentFound)
            {
                citiesArgumentFound = true;
                continue;
            }
            if (!citiesArgumentFound) continue;
            var city = argument.EndsWith(",") ? argument.Replace(",", "") : argument;
            Console.WriteLine($"Found city {city}. Getting weather info..");
            var responseMessage = await _weatherApi.GetCityWeather(city);
            
            Console.WriteLine($"Data: {await responseMessage.Content.ReadAsStringAsync()}");
            Console.WriteLine("Saving to database");
            //todo: Implement saving to database;

        }

        if (!citiesArgumentFound)
        {
            throw new Exception("--cities argument not found");
        }
        

    }
}