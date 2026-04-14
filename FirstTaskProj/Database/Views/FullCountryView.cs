namespace FirstTaskProj.Database.Views
{
    public class FullCountryView
    {
        public int CityId { get; set; }
        public int CoutnryId { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string RegionLeaderName { get; set; }
        public string CityLeaderName { get; set; }
        public string CountryLeaderName { get; set; }
    }
}