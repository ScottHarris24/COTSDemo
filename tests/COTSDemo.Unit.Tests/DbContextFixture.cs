using COTSDemo.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace COTSDemo.Unit.Tests;

public class DbContextFixture : IDisposable
{
    private COTSDemoDbContext _dbContext { get; set; } = null!;

    private static object _lock = new object();
    public COTSDemoDbContext SetupEfDbContext(IServiceProvider serviceProvider)
    {
        lock (_lock)
        {
            if (_dbContext == null)
            {
                _dbContext = serviceProvider.GetRequiredService<COTSDemoDbContext>();

                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var datatbaseType = configuration["DatabaseType"]!;

                if (datatbaseType.Equals("InMemory", StringComparison.OrdinalIgnoreCase))
                {
                    RepositoryInitializer.SeedEfData(_dbContext);
                }
            }
        }

        return _dbContext;
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
    }
}