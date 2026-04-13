namespace FirstTaskProj.Models
{
    public class City : BaseModel
    {
        public string CityName { get; set; }
        public virtual Region Region { get; set; }
        public int CityPopulation { get; set; }
    }
}
