using FirstTaskProj.Models;
using FirstTaskProj.Repositories;
using System.Reflection.Metadata;

namespace FirstTaskProj.Services
{
    public class GrowPopulationService : IGrowPopulationService
    {
        private IBaseRepository<Country> Countries;
        private IBaseRepository<Region> Regions;
        private IBaseRepository<City> Cities;

        public void GrowPopulation()
        {
            var newId = Guid.NewGuid();
            var rand = new Random();
            Cities.Create(new City
            {
                Id = newId,
                Name = $"City {rand.Next()}",
                CityPopulation = rand.Next()
            });
        }
    }
}
