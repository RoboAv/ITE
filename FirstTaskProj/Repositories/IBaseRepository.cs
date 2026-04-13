using FirstTaskProj.Models;

namespace FirstTaskProj.Repositories
{
    public interface IBaseRepository <TDBModel> where TDBModel : BaseModel
    {
        public List <TDBModel> GetAll ();
        public TDBModel Get (Guid id);
        public TDBModel Create(TDBModel baseModel);
        public TDBModel Update(TDBModel baseModel);
        public void Delete (Guid id);
    }
}
