namespace FirstTaskProj.Repositories
{
    public interface IBaseRepository <BaseModel>
    {
        public List <BaseModel> GetAll ();
        public BaseModel Get (int id);
        public BaseModel Create(BaseModel baseModel);
        public BaseModel Update(BaseModel baseModel);
        public void Delete (int id);
    }
}
