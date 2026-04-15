using FirstTaskProj.Tests;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Xunit;

public class MainControllerIntegrationTests : IClassFixture<SqlServerFixture>
{
    private readonly SqlServerFixture _fixture;
    public MainControllerIntegrationTests(SqlServerFixture fixture)
    {
        _fixture = fixture;
    }

    // Group 1: GET
    [Fact]
    public async Task GetCity_ShouldReturnSeededCities_AndOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/Main/city");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var cities = await response.Content.ReadFromJsonAsync<List<CityDto>>();

        cities.Should().NotBeNull();
        cities.Should().HaveCountGreaterThanOrEqualTo(2);
        cities!.Select(x => x.Name).Should().Contain(new[] { "Region of 17", "Region of 18" });
    }

    [Fact]
    public async Task GetRegion_ShouldReturnSeededRegions_AndOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/Main/region");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var regions = await response.Content.ReadFromJsonAsync<List<RegionDto>>();

        regions.Should().NotBeNull();
        regions.Should().HaveCountGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetCountry_ShouldReturnSeededCountries_AndOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/Main/country");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var countries = await response.Content.ReadFromJsonAsync<List<CountryDto>>();

        countries.Should().NotBeNull();
        countries.Should().HaveCountGreaterThanOrEqualTo(2);
        countries!.Select(x => x.Name).Should().Contain(new[] { "Alliance", "Resistanse" });
    }

    [Fact]
    public async Task GetFullCountryView_ShouldReturnData_AndOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/Main/country/all");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var rows = await response.Content.ReadFromJsonAsync<List<FullCountryViewDto>>();

        rows.Should().NotBeNull();
        rows.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAllPagination_ShouldReturnRequestedPageSize_AndOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/Main/country/allPag?page=1&pageSize=1");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        json.RootElement.GetProperty("page").GetInt32().Should().Be(1);
        json.RootElement.GetProperty("pageSize").GetInt32().Should().Be(1);
        json.RootElement.GetProperty("totalCount").GetInt32().Should().BeGreaterThan(0);
        json.RootElement.GetProperty("data").GetArrayLength().Should().Be(1);
    }

    // Group 2: PUT
    [Fact]
    public async Task PutCountry_ShouldUpdateExistingCountry_AndReturnOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var countries = await client.GetFromJsonAsync<List<CountryDto>>("/api/Main/country");
        var countryToUpdate = countries!.First();

        var payload = new
        {
            Id = countryToUpdate.Id,
            Name = "Updated country",
            LeaderName = "Updated leader",
        };

        var putResponse = await client.PutAsJsonAsync("/api/Main/country", payload);
        putResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedCountries = await client.GetFromJsonAsync<List<CountryDto>>("/api/Main/country");
        updatedCountries.Should().ContainSingle(x =>
            x.Id == countryToUpdate.Id &&
            x.Name == "Updated country" &&
            x.LeaderName == "Updated leader");
    }

    [Fact]
    public async Task PutCity_ShouldReturnNotFound_ForUnknownId()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var regions = await client.GetFromJsonAsync<List<RegionDto>>("/api/Main/region");
        var firstRegion = regions!.First();

        var payload = new
        {
            Id = -1,
            Name = "Ghost city",
            LeaderName = "Ghost leader",
            RegionId = firstRegion.Id,
            Region = new
            {
                Id = firstRegion.Id,
                Name = firstRegion.Name,
                LeaderName = firstRegion.LeaderName,
                CountryId = firstRegion.CountryId,
                Country = new
                {
                    Id = 1,
                    Name = "Alliance",
                    LeaderName = "Walles"
                }
            }
        };

        var response = await client.PutAsJsonAsync("/api/Main/city", payload);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // Group 3: POST
    [Fact]
    public async Task PostCountry_ShouldPersistCountry_InSqlServer_AndReturnOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var country = new
        {
            Name = "Testland",
            LeaderName = "Leader X"
        };

        var response = await client.PostAsJsonAsync("/api/Main/country", country);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var countries = await client.GetFromJsonAsync<List<CountryDto>>("/api/Main/country");
        countries.Should().ContainSingle(x =>
            x.Name == "Testland" &&
            x.LeaderName == "Leader X");
    }

    [Fact]
    public async Task PostCountry_ShouldReturnBadRequest_WhenBodyIsNull()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        using var content = new StringContent("null", Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/Main/country", content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // Group 4: DELETE
    [Fact]
    public async Task DeleteCity_ShouldRemoveCity_AndReturnOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var cities = await client.GetFromJsonAsync<List<CityDto>>("/api/Main/city");
        var cityToDelete = cities!.First();

        var deleteResponse = await client.DeleteAsync($"/api/Main/city/{cityToDelete.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var remainingCities = await client.GetFromJsonAsync<List<CityDto>>("/api/Main/city");
        remainingCities.Should().NotContain(x => x.Id == cityToDelete.Id);
    }

    [Fact]
    public async Task DeleteCity_ShouldReturnNotFound_ForUnknownId()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var response = await client.DeleteAsync("/api/Main/city/-1");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteRegion_ShouldReturnNotFound_ForUnknownId()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var response = await client.DeleteAsync("/api/Main/region/-1");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteCountry_ShouldReturnNotFound_ForUnknownId()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var response = await client.DeleteAsync("/api/Main/country/-1");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private sealed record CityDto(int Id, string Name, string LeaderName, int RegionId);
    private sealed record RegionDto(int Id, string Name, string LeaderName, int CountryId);
    private sealed record CountryDto(int Id, string Name, string LeaderName);
    private sealed record FullCountryViewDto(
        int? CityId,
        int CountryId,
        int? RegionId,
        string? RegionName,
        string? CountryName,
        string? CityName,
        string? RegionLeaderName,
        string? CityLeaderName,
        string? CountryLeaderName);
}