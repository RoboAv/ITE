using Microsoft.Identity.Client;

namespace FirstTaskProj.Models
{
    public class City : BaseModel
    {
        public int RegionId { get; set; }
        public virtual Region? Region { get; set; }
    }
}
