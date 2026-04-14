using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstTaskProj.Migrations
{
    /// <inheritdoc />
    public partial class fullCountryView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW FullCountryView AS
                SELECT 
                co.Id as CountryID, co.Name as CountryName, co.LeaderName as CountryLeaderName,
                r.Id as RegionID, r.Name as RegionName, r.LeaderName as RegionLeaderName,
                ci.Id as CityID, ci.Name as CityName, ci.LeaderName as CityLeaderName
                FROM Countries co
                LEFT JOIN Regions r ON r.CountryId = co.Id
                LEFT JOIN Cities ci ON ci.RegionId = r.Id;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
