using FirstTaskProj.Tests;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Xunit;

public class RegionControllerIntegrationTests : IClassFixture<SqlServerFixture>
{
    private readonly SqlServerFixture _fixture;

    public RegionControllerIntegrationTests(SqlServerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetRegion_ShouldReturnSeededRegions_AndOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/Region");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var regions = await response.Content.ReadFromJsonAsync<List<RegionDto>>();

        regions.Should().NotBeNull();
        regions.Should().HaveCountGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetRegionById_ShouldReturnExistingRegion_AndOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var regions = await client.GetFromJsonAsync<List<RegionDto>>("/api/Region");
        var existingRegion = regions!.First();

        var response = await client.GetAsync($"/api/Region/{existingRegion.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var region = await response.Content.ReadFromJsonAsync<RegionDto>();
        region.Should().NotBeNull();
        region!.Id.Should().Be(existingRegion.Id);
        region.Name.Should().Be(existingRegion.Name);
    }

    [Fact]
    public async Task PutRegion_ShouldUpdateExistingRegion_AndReturnOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var regions = await client.GetFromJsonAsync<List<RegionDto>>("/api/Region");
        var regionToUpdate = regions!.First();

        var payload = new
        {
            Id = regionToUpdate.Id,
            Name = "Updated region",
            LeaderName = "Updated region leader",
            CountryId = regionToUpdate.CountryId
        };

        var putResponse = await client.PutAsJsonAsync("/api/Region", payload);
        putResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedRegions = await client.GetFromJsonAsync<List<RegionDto>>("/api/Region");
        updatedRegions.Should().ContainSingle(x =>
            x.Id == regionToUpdate.Id &&
            x.Name == "Updated region" &&
            x.LeaderName == "Updated region leader");
    }

    [Fact]
    public async Task PutRegion_ShouldReturnNotFound_ForUnknownId()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var countries = await client.GetFromJsonAsync<List<CountryDto>>("/api/Country");
        var firstCountry = countries!.First();

        var payload = new
        {
            Id = -1,
            Name = "Ghost region",
            LeaderName = "Ghost region leader",
            CountryId = firstCountry.Id
        };

        var response = await client.PutAsJsonAsync("/api/Region", payload);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostRegion_ShouldPersistRegion_AndReturnOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var countries = await client.GetFromJsonAsync<List<CountryDto>>("/api/Country");
        var country = countries!.First();

        var region = new
        {
            Name = "Test region",
            LeaderName = "Region leader",
            CountryId = country.Id
        };

        var response = await client.PostAsJsonAsync("/api/Region", region);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var regions = await client.GetFromJsonAsync<List<RegionDto>>("/api/Region");
        regions.Should().ContainSingle(x =>
            x.Name == "Test region" &&
            x.LeaderName == "Region leader" &&
            x.CountryId == country.Id);
    }

    [Fact]
    public async Task PostRegion_ShouldReturnBadRequest_WhenBodyIsNull()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        using var content = new StringContent("null", Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/Region", content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteRegion_ShouldRemoveRegion_AndReturnOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var cities = await client.GetFromJsonAsync<List<CityDto>>("/api/City");
        var regions = await client.GetFromJsonAsync<List<RegionDto>>("/api/Region");
        var regionIdsWithCities = cities!.Select(c => c.RegionId).ToHashSet();
        var regionToDelete = regions!.First(r => !regionIdsWithCities.Contains(r.Id));

        var deleteResponse = await client.DeleteAsync($"/api/Region/{regionToDelete.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var remainingRegions = await client.GetFromJsonAsync<List<RegionDto>>("/api/Region");
        remainingRegions.Should().NotContain(r => r.Id == regionToDelete.Id);
    }

    [Fact]
    public async Task DeleteRegion_ShouldReturnNotFound_ForUnknownId()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var response = await client.DeleteAsync("/api/Region/-1");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private sealed record CityDto(int Id, string Name, string LeaderName, int RegionId);
    private sealed record RegionDto(int Id, string Name, string LeaderName, int CountryId);
    private sealed record CountryDto(int Id, string Name, string LeaderName);
}
