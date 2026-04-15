namespace FirstTaskProj.Models
{
    public class Region : BaseModel
    {
        public int CountryId { get; set; }
        public virtual Country? Country { get; set; }
    }
}
