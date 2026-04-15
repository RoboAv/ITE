using FirstTaskProj.Tests;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Xunit;

public class CityControllerIntegrationTests : IClassFixture<SqlServerFixture>
{
    private readonly SqlServerFixture _fixture;

    public CityControllerIntegrationTests(SqlServerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetCity_ShouldReturnSeededCities_AndOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/City");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var cities = await response.Content.ReadFromJsonAsync<List<CityDto>>();

        cities.Should().NotBeNull();
        cities.Should().HaveCountGreaterThanOrEqualTo(2);
        cities!.Select(x => x.Name).Should().Contain(new[] { "Region of 17", "Region of 18" });
    }

    [Fact]
    public async Task GetCityById_ShouldReturnExistingCity_AndOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var cities = await client.GetFromJsonAsync<List<CityDto>>("/api/City");
        var existingCity = cities!.First();

        var response = await client.GetAsync($"/api/City/{existingCity.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var city = await response.Content.ReadFromJsonAsync<CityDto>();
        city.Should().NotBeNull();
        city!.Id.Should().Be(existingCity.Id);
        city.Name.Should().Be(existingCity.Name);
    }

    [Fact]
    public async Task PutCity_ShouldUpdateExistingCity_AndReturnOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var cities = await client.GetFromJsonAsync<List<CityDto>>("/api/City");
        var cityToUpdate = cities!.First();

        var payload = new
        {
            Id = cityToUpdate.Id,
            Name = "Updated city",
            LeaderName = "Updated city leader",
            RegionId = cityToUpdate.RegionId
        };

        var putResponse = await client.PutAsJsonAsync("/api/City", payload);
        putResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedCities = await client.GetFromJsonAsync<List<CityDto>>("/api/City");
        updatedCities.Should().ContainSingle(x =>
            x.Id == cityToUpdate.Id &&
            x.Name == "Updated city" &&
            x.LeaderName == "Updated city leader");
    }

    [Fact]
    public async Task PutCity_ShouldReturnNotFound_ForUnknownId()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var regions = await client.GetFromJsonAsync<List<RegionDto>>("/api/Region");
        var firstRegion = regions!.First();

        var payload = new
        {
            Id = -1,
            Name = "Ghost city",
            LeaderName = "Ghost leader",
            RegionId = firstRegion.Id
        };

        var response = await client.PutAsJsonAsync("/api/City", payload);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostCity_ShouldPersistCity_AndReturnOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var regions = await client.GetFromJsonAsync<List<RegionDto>>("/api/Region");
        var region = regions!.First();

        var city = new
        {
            Name = "Test city",
            LeaderName = "City leader",
            RegionId = region.Id
        };

        var response = await client.PostAsJsonAsync("/api/City", city);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var cities = await client.GetFromJsonAsync<List<CityDto>>("/api/City");
        cities.Should().ContainSingle(x =>
            x.Name == "Test city" &&
            x.LeaderName == "City leader" &&
            x.RegionId == region.Id);
    }

    [Fact]
    public async Task PostCity_ShouldReturnBadRequest_WhenBodyIsNull()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        using var content = new StringContent("null", Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/City", content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteCity_ShouldRemoveCity_AndReturnOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var cities = await client.GetFromJsonAsync<List<CityDto>>("/api/City");
        var cityToDelete = cities!.First();

        var deleteResponse = await client.DeleteAsync($"/api/City/{cityToDelete.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var remainingCities = await client.GetFromJsonAsync<List<CityDto>>("/api/City");
        remainingCities.Should().NotContain(x => x.Id == cityToDelete.Id);
    }

    [Fact]
    public async Task DeleteCity_ShouldReturnNotFound_ForUnknownId()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var response = await client.DeleteAsync("/api/City/-1");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private sealed record CityDto(int Id, string Name, string LeaderName, int RegionId);
    private sealed record RegionDto(int Id, string Name, string LeaderName, int CountryId);
}
