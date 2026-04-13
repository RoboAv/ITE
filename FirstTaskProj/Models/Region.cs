namespace FirstTaskProj.Models
{
    public class Region : BaseModel
    {
        public virtual Country Country { get; set; }
        public float growthFactor { get; set; }
    }
}
