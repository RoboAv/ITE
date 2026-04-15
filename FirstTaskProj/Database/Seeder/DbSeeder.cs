using FirstTaskProj.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstTaskProj.Database.Seeder
{
    public class DbSeeder
    {
        private static void SeedCountries (ApplicationContext context)
        {
            if (context.Countries.Any())
            {
                return;
            }

            List<Country> countries = new List<Country>();
            countries.AddRange(
                new Country { Name = "Alliance", LeaderName = "Walles" },
                new Country { Name = "Resistanse", LeaderName = "Elija" }
            );

            for (int i = 0; i < countries.Count; i++)
            {
                context.Countries.Add(countries[i]);
            }
            context.SaveChanges();
        }
        private static void SeedRegions (ApplicationContext context)
        {
            if (context.Regions.Any())
            {
                return;
            }
            List<Country> countries = context.Countries.ToList();
            List<Region> regions = new List<Region>();
            regions.AddRange(
                new Region { Name = "Region of 17", LeaderName = "Walles", CountryId = countries[0].Id},
                new Region { Name = "Region of 18", LeaderName = "Elija", CountryId = countries[0].Id}
            );

            for (int i = 0; i < regions.Count; i++)
            {
                context.Regions.Add(regions[i]);
            }
            context.SaveChanges();
        }
        private static void SeedCities (ApplicationContext context)
        {
            if (context.Cities.Any())
            {
                return;
            }

            List<Region> regions = context.Regions.ToList();
            List<City> cities = new List<City>();

            cities.AddRange(
                new City { Name = "Region of 17", LeaderName = "Walles", RegionId = regions[0].Id},
                new City { Name = "Region of 18", LeaderName = "Elija", RegionId = regions[0].Id}
            );

            for (int i = 0; i < cities.Count; i++)
            {
                context.Cities.Add(cities[i]);
            }
            context.SaveChanges();
        }
        public static void Seed (ApplicationContext context)
        {

            SeedCountries(context);

            SeedRegions(context);

            SeedCities(context);
        }
    }
}
