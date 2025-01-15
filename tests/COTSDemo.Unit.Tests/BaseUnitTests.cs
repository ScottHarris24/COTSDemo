using COTSDemo.Abstractions.Entities;
using COTSDemo.Abstractions.Interfaces;
using COTSDemo.Repositories;
using COTSDemo.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace COTSDemo.Unit.Tests;

public abstract class BaseUnitTests  : IClassFixture<DbContextFixture>
{
    protected COTSDemoDbContext _dbContext;
    protected IServiceProvider _serviceProvider;

    public BaseUnitTests()
    {
        _serviceProvider = GetServiceProvider(true);
        _dbContext = SetupEfDbContext(_serviceProvider);
    }

    public BaseUnitTests(DbContextFixture dbContextFixture, bool addServices)
    {
        _serviceProvider = GetServiceProvider(addServices);
        _dbContext = dbContextFixture.SetupEfDbContext(_serviceProvider);
    }

    public COTSDemoDbContext SetupEfDbContext(IServiceProvider serviceProvider)
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

        return _dbContext;
    }


    #region Protected Properties and Functions

    protected IServiceProvider ServiceProvider
    {
        get
        {
            if (_serviceProvider == null)
            {
                _serviceProvider = GetServiceProvider(true);
            }

            return _serviceProvider;
        }
    }

    protected IServiceProvider GetServiceProvider(bool addServices = true)
    {
        if (_serviceProvider == null)
        {
            var host = CreateHostBuilder(addServices).Build();
            _serviceProvider = host.Services;
        }

        return _serviceProvider;
    }

    #endregion Protected Properties and Functions


    #region Private Support Functions3

    static IHostBuilder CreateHostBuilder(bool addServices = true)
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                if (addServices)
                {
                    services
                        //.AddTransient(x => new COTSDemoDbContext(true))
                        .AddTransient<COTSDemoDbContext>()
                        .AddTransient<ICustomerService, CustomerService>()
                        .AddTransient<ICustomerRepository<CustomerEntity>, CustomerRepository>();
                }
            });

    }

    #endregion Private Support Functions

}