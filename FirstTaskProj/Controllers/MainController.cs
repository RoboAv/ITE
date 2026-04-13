using FirstTaskProj.Models;
using FirstTaskProj.Repositories;
using FirstTaskProj.Services;
using Microsoft.AspNetCore.Mvc;

namespace FirstTaskProj.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        public IGrowPopulationService GrowPopulationService { get; set; }
        public IBaseRepository<City> Cities { get; set; }
        public MainController(IGrowPopulationService growPopulationService, IBaseRepository<City> cities) { 
            GrowPopulationService = growPopulationService;
            Cities = cities;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(Cities.GetAll());
        }

        [HttpPost]
        public JsonResult Post()
        {
            GrowPopulationService.GrowPopulation();
            return new JsonResult(Cities.GetAll());
        }

        [HttpPut]
        public JsonResult Put(City city)
        {
            bool success = true;
            var toUpdate = Cities.Get(city.Id);

            try
            {
                if (toUpdate != null)
                {
                    Cities.Update(toUpdate);
                }
                else
                {
                    success = false;
                }
            }
            catch (Exception)
            {
                success = false;
            }

            return success ? new JsonResult("Put was successfull!") : new JsonResult("Put was unsuccessfull!");
        }

        [HttpDelete]
        public JsonResult Delete(Guid id)
        {
            bool success = true;
            var toDelete = Cities.Get(id);

            try
            {
                if (toDelete != null)
                {
                    Cities.Delete(toDelete.Id);
                }
                else
                {
                    success = false;
                }
            }
            catch (Exception)
            {
                success = false;
            }

            return success ? new JsonResult("Delete was successfull!") : new JsonResult("Delete was unsuccessfull!");
        }
    }
}
