using FirstTaskProj.Database;
using FirstTaskProj.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstTaskProj.Repositories
{
    public class BaseRepository<TDBModel> : IBaseRepository<TDBModel> where TDBModel : BaseModel
    {
        private ApplicationContext Context { get; set; }

        public BaseRepository(ApplicationContext context) 
        {
            Context = context;
        }
        public List<TDBModel> GetAll()
        {
            return Context.Set<TDBModel>().ToList<TDBModel>();
        }

        public TDBModel Get (int id)
        {
            return Context.Set<TDBModel>().FirstOrDefault(m => m.Id == id);
        }

        public TDBModel Create(TDBModel baseModel)
        {
            Context.Set<TDBModel>().Add(baseModel);
            Context.SaveChanges();
            return baseModel;
        }

        public TDBModel Update(TDBModel baseModel)
        {
            var modelToUpdate = Context.Set<TDBModel>().FirstOrDefault(m => m.Id == baseModel.Id);
            if (modelToUpdate != null)
            {
                modelToUpdate = baseModel;
            }
            Context.Update(modelToUpdate);
            Context.SaveChanges();
            return modelToUpdate;
        }

        public void Delete(int id)
        {
            var modelToDelete = Context.Set<TDBModel>().FirstOrDefault(m => m.Id == id);
            Context.Set<TDBModel>().Remove(modelToDelete);
        }
    }
}
