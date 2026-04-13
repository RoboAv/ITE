namespace FirstTaskProj.Models
{
    public class Region : BaseModel
    {
        public string RegionName { get; set; }
        public virtual Country Country { get; set; }
        public float growthFactor { get; set; }
    }
}
