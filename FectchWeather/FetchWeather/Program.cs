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
        if (args.Length == 0)
        {

            Log(LogType.Error, "Missing arguments. Terminating..");
            return;
        }

        var citiesArgumentFound = false;
        var weatherApi = new WeatherApi();
        await using var dbContext= new DatabaseContext();
        Log(LogType.Debug, $"Database path: {dbContext.DbPath}.");
        foreach (var argument in args)
        {
            if (argument.StartsWith("--") && argument.EndsWith("cities") && !citiesArgumentFound)
            {
                citiesArgumentFound = true;
                continue;
            }
            if (!citiesArgumentFound) continue;
            var city = argument.EndsWith(",") ? argument.Replace(",", "") : argument;
            Log(LogType.Info, $"Getting {city} weather info..");
            var responseMessage = await weatherApi.GetCityWeather(city);
            if (responseMessage.StatusCode != HttpStatusCode.OK && responseMessage.StatusCode != HttpStatusCode.InternalServerError)
            {
                Log(LogType.Error, $"\"{city}\". Check city spelling. Status code: {responseMessage.StatusCode}");
                continue;
            }

            if (responseMessage.StatusCode == HttpStatusCode.InternalServerError)
            {
                Log(LogType.Error, "Internal server error. Could not fetch data! Terminating..");
                return;
            }

            Log(LogType.Ok, $"Got {city} weather info. Status code: {responseMessage.StatusCode}");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var weatherInfo = JsonSerializer.Deserialize<CityWeather>(jsonData);
            if (weatherInfo == null)
            {
                Log(LogType.Error, $"Can not parsing JSON info for city \"{city}\". Json data: {jsonData}");
                continue;
            }

            Log(LogType.Info, $"Weather in {weatherInfo.City} city. Temperature: {weatherInfo.Temperature}. Wind speed: {weatherInfo.WindSpeed}. Summary: {weatherInfo.Summary}\n");
            dbContext.CityWeather.Add(weatherInfo);
            await dbContext.SaveChangesAsync();

        }

        if (!citiesArgumentFound)
        {
            Log(LogType.Error, "--cities argument not found. Terminating..");
        }
        

    }

    private enum LogType
    {
        Error,
        Ok,
        Info,
        Debug
    }

    private static void Log(LogType logType, string message)
    {
        switch (logType)
        {
            case LogType.Error:
                Console.WriteLine($"[ERROR] {message}.");
                Console.WriteLine("Press any key to continue..");
                Console.ReadKey();
                break;
            case LogType.Ok:
                Console.WriteLine($"[OK] {message}");
                break;
            case LogType.Info:
                Console.WriteLine($"[INFO] {message}");
                break;
            case LogType.Debug:
                Console.WriteLine($"[DEBUG] {message}");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
        }
    }
}