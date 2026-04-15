using Microsoft.Data.SqlClient;
using Xunit;


namespace FirstTaskProj.Tests
{
    public class SqlServerFixture : IAsyncLifetime
    {
        private const string LocalDbInstance = @"(localdb)\MSSQLLocalDB";
        
        public string CreateDatabaseConnectionString(string? databaseName = null)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = LocalDbInstance,
                InitialCatalog = databaseName ?? $"IntegrationTests_{Guid.NewGuid():N}",
                IntegratedSecurity = true,
                TrustServerCertificate = true,
                MultipleActiveResultSets = true
            };

            return builder.ConnectionString;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
