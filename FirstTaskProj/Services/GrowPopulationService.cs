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

        public void RenameCountry(int id, string newName)
        {
        }
    }
}
