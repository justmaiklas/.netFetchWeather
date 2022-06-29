using FetchWeather.Models;
using Microsoft.EntityFrameworkCore;

namespace FetchWeather.SQLite
{
    class DatabaseContext : DbContext
    {
        public DbSet<CityWeather> CityWeather { get; set; }
        public string DbPath { get; }
        public DatabaseContext()
        {
            const Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "weather.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
        
    }
}
