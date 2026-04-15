using FirstTaskProj.Tests;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Xunit;

public class CountryControllerIntegrationTests : IClassFixture<SqlServerFixture>
{
    private readonly SqlServerFixture _fixture;

    public CountryControllerIntegrationTests(SqlServerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetCountry_ShouldReturnSeededCountries_AndOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/Country");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var countries = await response.Content.ReadFromJsonAsync<List<CountryDto>>();

        countries.Should().NotBeNull();
        countries.Should().HaveCountGreaterThanOrEqualTo(2);
        countries!.Select(x => x.Name).Should().Contain(new[] { "Alliance", "Resistanse" });
    }

    [Fact]
    public async Task GetCountryById_ShouldReturnExistingCountry_AndOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var countries = await client.GetFromJsonAsync<List<CountryDto>>("/api/Country");
        var existingCountry = countries!.First();

        var response = await client.GetAsync($"/api/Country/{existingCountry.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var country = await response.Content.ReadFromJsonAsync<CountryDto>();
        country.Should().NotBeNull();
        country!.Id.Should().Be(existingCountry.Id);
        country.Name.Should().Be(existingCountry.Name);
    }

    [Fact]
    public async Task GetFullCountryView_ShouldReturnData_AndOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/Country/all");
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

        var response = await client.GetAsync("/api/Country/allPag?page=1&pageSize=1");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        json.RootElement.GetProperty("page").GetInt32().Should().Be(1);
        json.RootElement.GetProperty("pageSize").GetInt32().Should().Be(1);
        json.RootElement.GetProperty("totalCount").GetInt32().Should().BeGreaterThan(0);
        json.RootElement.GetProperty("data").GetArrayLength().Should().Be(1);
    }

    [Fact]
    public async Task PutCountry_ShouldUpdateExistingCountry_AndReturnOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var countries = await client.GetFromJsonAsync<List<CountryDto>>("/api/Country");
        var countryToUpdate = countries!.First();

        var payload = new
        {
            Id = countryToUpdate.Id,
            Name = "Updated country",
            LeaderName = "Updated leader",
        };

        var putResponse = await client.PutAsJsonAsync("/api/Country", payload);
        putResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedCountries = await client.GetFromJsonAsync<List<CountryDto>>("/api/Country");
        updatedCountries.Should().ContainSingle(x =>
            x.Id == countryToUpdate.Id &&
            x.Name == "Updated country" &&
            x.LeaderName == "Updated leader");
    }

    [Fact]
    public async Task PutCountry_ShouldReturnNotFound_ForUnknownId()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var payload = new
        {
            Id = -1,
            Name = "Ghost country",
            LeaderName = "Ghost leader"
        };

        var response = await client.PutAsJsonAsync("/api/Country", payload);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

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

        var response = await client.PostAsJsonAsync("/api/Country", country);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var countries = await client.GetFromJsonAsync<List<CountryDto>>("/api/Country");
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
        var response = await client.PostAsync("/api/Country", content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteCountry_ShouldRemoveCountry_AndReturnOk()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var regions = await client.GetFromJsonAsync<List<RegionDto>>("/api/Region");
        var countries = await client.GetFromJsonAsync<List<CountryDto>>("/api/Country");
        var countryIdsWithRegions = regions!.Select(r => r.CountryId).ToHashSet();
        var countryToDelete = countries!.First(c => !countryIdsWithRegions.Contains(c.Id));

        var deleteResponse = await client.DeleteAsync($"/api/Country/{countryToDelete.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var remainingCountries = await client.GetFromJsonAsync<List<CountryDto>>("/api/Country");
        remainingCountries.Should().NotContain(c => c.Id == countryToDelete.Id);
    }

    [Fact]
    public async Task DeleteCountry_ShouldReturnNotFound_ForUnknownId()
    {
        using var factory = new CustomWebApplicationFactory(_fixture.CreateDatabaseConnectionString());
        using var client = factory.CreateClient();

        var response = await client.DeleteAsync("/api/Country/-1");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private sealed record CountryDto(int Id, string Name, string LeaderName);
    private sealed record RegionDto(int Id, string Name, string LeaderName, int CountryId);
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
