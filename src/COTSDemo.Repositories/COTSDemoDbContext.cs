using COTSDemo.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace COTSDemo.Repositories;

//public class COTSDemoDbContext(IConfiguration _configuration) : DbContext
public class COTSDemoDbContext : DbContext

{
    private IConfiguration _configuration;

    public COTSDemoDbContext()
    {

        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json")
            .Build();

        var dataBaseType = _configuration["DatabaseType"];

        if (string.IsNullOrWhiteSpace(dataBaseType))
        {
            throw new ArgumentNullException("Database type is null");
        }
    }

    #region Public Properties

    public DbSet<CustomerEntity> Customers { get; set; } = null!;

    public DbSet<OrderEntity> Orders { get; set; } = null!;

    public DbSet<OrderDetailEntity> OrderDetails { get; set; } = null!;

    public DbSet<ProductEntity> Products { get; set; } = null!;

    #endregion Public Properties

    #region Protected Functions

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var data = GetDbTypeAndConnectionString(optionsBuilder.IsConfigured);

        switch (data.DatabaseType)
        {
            case { } when data.DatabaseType.Equals("InMemory", StringComparison.OrdinalIgnoreCase):
            {
                optionsBuilder.UseInMemoryDatabase(data.ConnectionString);
                break;
            }

            default:
            {
                optionsBuilder.UseSqlServer(data.ConnectionString);
                break;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var databaseType = _configuration["DatabaseType"];
        if (databaseType!.Equals("InMemory", StringComparison.OrdinalIgnoreCase) == false)
        {
            RepositoryInitializer.SeedEfData(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }

    #endregion Protected Functions

    private (string DatabaseType, string ConnectionString) GetDbTypeAndConnectionString(bool isConfigured)
    {

        if (_configuration == null)
        {
            return ("", "");
        }

        var databaseType = _configuration["DatabaseType"];
        var connectionString = _configuration.GetConnectionString($"{databaseType}COTSDemo");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = _configuration.GetConnectionString("COTSDemo");
        }

        return (databaseType!, connectionString!);
    }
}