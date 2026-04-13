namespace FirstTaskProj.Models
{
    public class City : BaseModel
    {
        public virtual Region Region { get; set; }
        public int CityPopulation { get; set; }
    }
}
