namespace FirstTaskProj.Models
{
    public class Region : BaseModel
    {
        public int CountryId { get; set; }
        public required Country Country { get; set; }
    }
}
