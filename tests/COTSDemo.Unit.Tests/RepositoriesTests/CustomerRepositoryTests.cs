using COTSDemo.Abstractions.Entities;
using COTSDemo.Abstractions.Enums;
using COTSDemo.Abstractions.Interfaces;
using COTSDemo.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Priority;

namespace COTSDemo.Unit.Tests.RepositoriesTests;

// NOTE:    Using nuget package to order unit tests because some tests will add and remove
//          items to the database which will cause different counts depending on the timing of when
//          some tests run.  This will force running of unit tests in order so those items that add/remove 
//          items will not interfere with other tests

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class CustomerRepositoryTests(DbContextFixture dbContextFixture) : BaseUnitTests(dbContextFixture, true)
{
    #region Public Tests

    [Theory, Priority(1)]
    [InlineData(1, "First 1", "Last 1", "Billing Address 1", "Shipping Address 1", "User1", "2025-1-1", "2025-1-1")]
    [InlineData(2, "First 2", "Last 2", "Billing Address 2", "Shipping Address 2", "User1", "2025-1-1", "2025-1-1")]
    [InlineData(3, "First 3", "Last 3", "Billing Address 3", "Shipping Address 3", "User1", "2025-1-4", "2025-1-6")]
    public async Task GetAsync_ShouldReturn_CorrectInlineValues(int id, string firstName, string lastName,
        string billingAddress, string shippingAddress, string lastUpdatedBy, string created, string lastUpdated)
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();

        // Action
        var response = await repository.GetAsync(id);
        var customer = response.Data as CustomerEntity;
        var createdDate = DateTime.Parse(created);
        var lastUpdatedDate = DateTime.Parse(lastUpdated);

        // Assert
        Assert.Equal(RepositoryReturnStatusType.Success, response.ReturnStatus);
        Assert.NotNull(customer);

        Assert.Equal(id, customer.Id);
        Assert.Equal(lastName, customer.LastName);
        Assert.Equal(firstName, customer.FirstName);
        Assert.Equal(billingAddress, customer.BillingAddress);
        Assert.Equal(shippingAddress, customer.ShippingAddress);

        Assert.Equal(createdDate, customer.Created);
        Assert.Equal(lastUpdatedDate, customer.LastUpdated);
        Assert.Equal(lastUpdatedBy, customer.LastUpdatedBy);
    }

    [Fact, Priority(1)]
    public async Task GetCustomerAsync_ShouldReturn_AllOrdersWithAllOrderDetails_ForGivenCustomer()
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();

        // Action
        var response = await repository.GetCustomerAsync(1, OrderShippedStatusType.All, OrderDetailStatusType.All);
        var customer = response.Data as CustomerEntity;

        // Assert
        Assert.Equal(RepositoryReturnStatusType.Success, response.ReturnStatus);
        Assert.NotNull(customer);

        AssertCustomerData(customer);

        Assert.NotNull(customer.Orders);
        Assert.Equal(5, customer.Orders.Count);

        var orders = customer.Orders.ToList();
        Assert.Null(orders[2].OrderDetails);
        Assert.Null(orders[3].OrderDetails);
        Assert.Equal(3, orders[0].OrderDetails.Count);
        Assert.Equal(2, orders[1].OrderDetails.Count);
        Assert.Single(orders[4].OrderDetails);
    }

    [Fact, Priority(1)]
    public async Task GetCustomerAsync_ShouldReturn_ShippedOrdersWithAllOrderDetails_ForGivenCustomer()
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();

        // Action
        var response = await repository.GetCustomerAsync(1, OrderShippedStatusType.Shipped, OrderDetailStatusType.All);
        var customer = response.Data as CustomerEntity;

        // Assert
        Assert.Equal(RepositoryReturnStatusType.Success, response.ReturnStatus);
        Assert.NotNull(customer);

        AssertCustomerData(customer);

        Assert.NotNull(customer.Orders);
        Assert.Equal(3, customer.Orders.Count);

        var orders = customer.Orders.ToList();
        Assert.Null(orders[1].OrderDetails);
        Assert.Equal(2, orders[0].OrderDetails.Count);
        Assert.Single(orders[2].OrderDetails);
    }

    [Fact, Priority(1)]
    public async Task GetCustomerAsync_ShouldReturn_NotShippedOrdersWithAllOrderDetails_ForGivenCustomer()
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();

        // Action
        var response = await repository.GetCustomerAsync(1, OrderShippedStatusType.NotShipped, OrderDetailStatusType.All);
        var customer = response.Data as CustomerEntity;

        // Assert
        Assert.Equal(RepositoryReturnStatusType.Success, response.ReturnStatus);
        Assert.NotNull(customer);

        AssertCustomerData(customer);

        Assert.NotNull(customer.Orders);
        Assert.Equal(2, customer.Orders.Count);

        var orders = customer.Orders.ToList();
        Assert.Null(orders[1].OrderDetails);
        Assert.Equal(3, orders[0].OrderDetails.Count);
    }

    [Fact, Priority(1)]
    public async Task GetCustomerAsync_ShouldReturn_AllOrdersWithOrderDetailInProgress_ForGivenCustomer()
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();

        // Action
        var response = await repository.GetCustomerAsync(1, OrderShippedStatusType.All, OrderDetailStatusType.InProgress);
        var customer = response.Data as CustomerEntity;

        // Assert
        Assert.Equal(RepositoryReturnStatusType.Success, response.ReturnStatus);
        Assert.NotNull(customer);

        AssertCustomerData(customer);

        Assert.NotNull(customer.Orders);
        Assert.Equal(5, customer.Orders.Count);

        var orders = customer.Orders.ToList();
        Assert.Null(orders[1].OrderDetails);
        Assert.Null(orders[2].OrderDetails);
        Assert.Null(orders[3].OrderDetails);
        Assert.Null(orders[4].OrderDetails);
        Assert.Single(orders[0].OrderDetails);
    }

    [Fact, Priority(1)]
    public async Task GetCustomerAsync_ShouldReturn_AllOrdersWithOrderDetailFulfilled_ForGivenCustomer()
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();

        // Action
        var response = await repository.GetCustomerAsync(1, OrderShippedStatusType.All, OrderDetailStatusType.Fulfilled);
        var customer = response.Data as CustomerEntity;

        // Assert
        Assert.Equal(RepositoryReturnStatusType.Success, response.ReturnStatus);
        Assert.NotNull(customer);

        AssertCustomerData(customer);

        Assert.NotNull(customer.Orders);
        Assert.Equal(5, customer.Orders.Count);

        var orders = customer.Orders.ToList();
        Assert.Null(orders[2].OrderDetails);
        Assert.Null(orders[3].OrderDetails);
        Assert.Null(orders[4].OrderDetails);
        Assert.Equal(2, orders[0].OrderDetails.Count);
        Assert.Equal(2, orders[1].OrderDetails.Count);
    }

    [Fact, Priority(1)]
    public async Task GetCustomerAsync_ShouldReturn_AllOrdersWithOrderDetailBackorder_ForGivenCustomer()
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();

        // Action
        var response = await repository.GetCustomerAsync(1, OrderShippedStatusType.All, OrderDetailStatusType.Backorder);
        var customer = response.Data as CustomerEntity;

        // Assert
        Assert.Equal(RepositoryReturnStatusType.Success, response.ReturnStatus);
        Assert.NotNull(customer);

        AssertCustomerData(customer);

        Assert.NotNull(customer.Orders);
        Assert.Equal(5, customer.Orders.Count);

        var orders = customer.Orders.ToList();
        Assert.Null(orders[0].OrderDetails);
        Assert.Null(orders[1].OrderDetails);
        Assert.Null(orders[2].OrderDetails);
        Assert.Null(orders[3].OrderDetails);
        Assert.Single(orders[4].OrderDetails);
    }

    [Fact, Priority(1)]
    public async Task GetCustomerAsync_ShouldReturn_AllOrdersWithNoOrderDetails_ForGivenCustomer()
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();

        // Action
        var response = await repository.GetCustomerAsync(1, OrderShippedStatusType.All);
        var customer = response.Data as CustomerEntity;

        // Assert
        Assert.Equal(RepositoryReturnStatusType.Success, response.ReturnStatus);
        Assert.NotNull(customer);

        AssertCustomerData(customer);

        Assert.NotNull(customer.Orders);
        Assert.Equal(5, customer.Orders.Count);

        var orders = customer.Orders.ToList();
        Assert.Null(orders[0].OrderDetails);
        Assert.Null(orders[1].OrderDetails);
        Assert.Null(orders[2].OrderDetails);
        Assert.Null(orders[3].OrderDetails);
        Assert.Null(orders[4].OrderDetails);
    }

    [Fact, Priority(1)]
    public async Task GetCustomerAsync_ShouldReturn_NoOrders_ForGivenCustomer()
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();

        // Action
        var response = await repository.GetCustomerAsync(1);
        var customer = response.Data as CustomerEntity;

        // Assert
        Assert.Equal(RepositoryReturnStatusType.Success, response.ReturnStatus);
        Assert.NotNull(customer);

        AssertCustomerData(customer);

        Assert.Null(customer.Orders);
    }

    [Fact, Priority(1)]
    public async Task GetAllAsync_ShouldReturn_CorrectNumberOfItemsData()
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();

        // Action
        var response = await repository.GetAllAsync();
        var customers = response.Data as List<CustomerEntity>;
        var customer = customers?.FirstOrDefault(x => x.Id == 2);

        // Assert 
        Assert.Equal(RepositoryReturnStatusType.Success, response.ReturnStatus);
        Assert.NotNull(customer);
        Assert.NotNull(customers);
        Assert.Equal(5, customers.Count);

        AssertCustomerData(customer, 2);

        AssertCustomerData(customers[0]);
        AssertCustomerData(customers[1], 2);
        AssertCustomerData(customers[2], 3, 3, 5);
        AssertCustomerData(customers[3], 4, 5, 10, 2);
        AssertCustomerData(customers[4], 5, 6, 10, 2);
    }
    
    [Fact, Priority(1)]
    public async Task QueryAsync_ShouldReturn_CorrectRecord()
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();

        // Action
        var response = await repository.QueryAsync(x => x.FirstName == "First 1");
        var customers = response.Data as List<CustomerEntity>;

        // Assert
        Assert.Equal(RepositoryReturnStatusType.Success, response.ReturnStatus);
        Assert.NotNull(customers);
        Assert.Single(customers);

        AssertCustomerData(customers[0]);
    }

    [Fact, Priority(1)]
    public async Task QueryCustomerAsync_ShouldReturn_NoOrders_ForQueriedCustomers()
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();

        // Action
        var response = await repository.QueryCustomersAsync(x => x.FirstName == "First 1" ||
                                                                 x.FirstName == "First 2");
        var customers = response.Data as List<CustomerEntity>;

        // Assert
        Assert.Equal(RepositoryReturnStatusType.Success, response.ReturnStatus);
        Assert.NotNull(customers);
        Assert.Equal(2, customers.Count);

        AssertCustomerData(customers[0]);
        AssertCustomerData(customers[1], 2);

        Assert.Null(customers[0].Orders);
        Assert.Null(customers[1].Orders);
    }

    [Fact, Priority(1)]
    public async Task QueryCustomerAsync_ShouldReturn_ShippedOrdersWithNoDetails_ForQueriedCustomers()
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();

        // Action
        var response = await repository.QueryCustomersAsync(x => x.FirstName == "First 1" ||
                                                                 x.FirstName == "First 2", 
                                                            OrderShippedStatusType.Shipped);
        var customers = response.Data as List<CustomerEntity>;

        // Assert
        Assert.Equal(RepositoryReturnStatusType.Success, response.ReturnStatus);
        Assert.NotNull(customers);
        Assert.Equal(2, customers.Count);

        AssertCustomerData(customers[0]); 
        AssertCustomerData(customers[1], 2);

        Assert.NotNull(customers[0].Orders);
        Assert.NotNull(customers[1].Orders);

        var orders = customers[0]
            .Orders
            .ToList();

        Assert.Null(orders[0].OrderDetails);
        Assert.Null(orders[1].OrderDetails);
        Assert.Null(orders[2].OrderDetails);

        orders = customers[1]
            .Orders
            .ToList();

        Assert.Null(orders[0].OrderDetails);
        Assert.Null(orders[1].OrderDetails);
        Assert.Null(orders[2].OrderDetails);

    }

    // NOTE:    The following tests need to take place after get tests because
    //          they will add/delete records that will change the counts of the gets
    [Fact, Priority(20)]
    public async Task AddAsync_ShouldAddAndReturn_CorrectRecord()
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();
        var addCustomer = CreateCustomerRecords(1)[0];
        
        // Action
        var response = await repository.AddAsync(addCustomer);
        var responseQuery = await repository.QueryAsync(x => x.FirstName == "First 100");

        var addedCustomer = response.Data as CustomerEntity;
        var queryCustomers = responseQuery.Data as List<CustomerEntity>;

        // Assert
        Assert.Equal(RepositoryReturnStatusType.Success, response.ReturnStatus);
        Assert.Equal(RepositoryReturnStatusType.Success, responseQuery.ReturnStatus);

        Assert.NotNull(addCustomer);
        Assert.NotNull(addedCustomer);
        Assert.NotNull(queryCustomers);
        Assert.Single(queryCustomers);

        Assert.Equal(addCustomer.LastName, addedCustomer.LastName);
        Assert.Equal(addCustomer.FirstName, addedCustomer.FirstName);
        Assert.Equal(addCustomer.BillingAddress, addedCustomer.BillingAddress);
        Assert.Equal(addCustomer.ShippingAddress, addedCustomer.ShippingAddress);

        Assert.Equal(addCustomer.Created, addedCustomer.Created);
        Assert.Equal(addCustomer.CreatedBy, addedCustomer.CreatedBy);

        Assert.Equal(addCustomer.LastUpdated, addedCustomer.LastUpdated);
        Assert.Equal(addCustomer.LastUpdatedBy, addedCustomer.LastUpdatedBy);
    }

    [Fact, Priority(30)]
    public async Task DeleteAsync_ShouldDelete_CorrectRecord()
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();
        var responseQuery = await repository.QueryAsync(x => x.FirstName == "First 100");
        var queryCustomers = responseQuery.Data as List<CustomerEntity>;
        var customer = queryCustomers?[0];

        // Action

        Assert.NotNull(customer);
        var deleteResponse = await repository.DeleteAsync(customer);
        var queryDeletedResponse = await repository.QueryAsync(x => x.FirstName == "First 100");

        // Assert
        Assert.Equal(RepositoryReturnStatusType.Success, deleteResponse.ReturnStatus);
        Assert.Equal(RepositoryReturnStatusType.Success, responseQuery.ReturnStatus);
        Assert.Equal(RepositoryReturnStatusType.NotFound, queryDeletedResponse.ReturnStatus);
    }

    [Fact, Priority(40)]
    public async Task AddRangeAsync_ShouldAddCustomer_QueryAndReturnCorrectRecords()
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();
        var addCustomers = CreateCustomerRecords(3);
        
        // Action
        var response = await repository.AddRangeAsync(addCustomers);
        var responseQuery = await repository.QueryAsync(x => x.FirstName.StartsWith("First 10"));

        var addedCustomers = response.Data as List<CustomerEntity>;
        var queryCustomers = responseQuery.Data as List<CustomerEntity>;

        // Assert
        Assert.Equal(RepositoryReturnStatusType.Success, response.ReturnStatus);
        Assert.Equal(RepositoryReturnStatusType.Success, responseQuery.ReturnStatus);

        Assert.NotNull(addedCustomers);
        Assert.NotNull(queryCustomers);

        Assert.Equal(addCustomers.Count, addedCustomers.Count);
        Assert.Equal(addCustomers.Count, queryCustomers.Count);

        for (int i = 0; i < addedCustomers.Count; i++)
        {
            var addCustomer = addCustomers[i];
            var queryCustomer = queryCustomers[i];
            var addedCustomer = addedCustomers[i];

            AssertAddCustomerData(addCustomer, addedCustomer);
            AssertAddCustomerData(addCustomer, queryCustomer);
        }
    }

    [Fact, Priority(50)]
    public async Task DeleteRangeAsync_ShouldQueryAndDelete_CorrectRecords()
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();
        var responseQuery = await repository.QueryAsync(x => x.FirstName.StartsWith("First 10"));
        var queryCustomers = responseQuery.Data as List<CustomerEntity>;

        // Action
        Assert.NotNull(queryCustomers);
        var deleteResponse = await repository.DeleteRangeAsync(queryCustomers);
        var queryDeletedResponse = await repository.QueryAsync(x => x.FirstName.StartsWith("First 10"));

        // Assert
        Assert.Equal(RepositoryReturnStatusType.Success, deleteResponse.ReturnStatus);
        Assert.Equal(RepositoryReturnStatusType.Success, responseQuery.ReturnStatus);
        Assert.Equal(RepositoryReturnStatusType.NotFound, queryDeletedResponse.ReturnStatus);
    }

    [Fact, Priority(60)]
    public async Task AddRangeAsync_ShouldAddCustomersWithOrderAndDetail_ReturnAndQueryCorrectRecords()
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();
        var addCustomers = CreateCustomerRecords(3, true);
        
        // Action
        var response = await repository.AddRangeAsync(addCustomers);
        var responseQuery = await repository.QueryCustomersAsync(x => x.FirstName.StartsWith("First 10"));

        var addedCustomers = (response.Data as List<CustomerEntity>)?
            .OrderBy(x => x.Id)
            .ToList();

        var queryCustomers = (responseQuery.Data as List<CustomerEntity>)?
            .OrderBy(x => x.Id)
            .ToList();

        // Assert
        Assert.Equal(RepositoryReturnStatusType.Success, response.ReturnStatus);
        Assert.Equal(RepositoryReturnStatusType.Success, responseQuery.ReturnStatus);

        Assert.NotNull(addedCustomers);
        Assert.NotNull(queryCustomers);

        Assert.Equal(addCustomers.Count, addedCustomers.Count);
        Assert.Equal(addCustomers.Count, queryCustomers.Count);

        for (int i = 0; i < addedCustomers.Count; i++)
        {
            var addCustomer = addCustomers[i];
            var queryCustomer = queryCustomers[i];
            var addedCustomer = addedCustomers[i];

            var queryOrders = AssertAddCustomerData(addCustomer, queryCustomer);
            var addedOrders = AssertAddCustomerData(addCustomer, addedCustomer);

            var addOrders = addCustomer
                .Orders
                .ToList();

            for (int j = 0; j < addOrders.Count; j++)
            {
                var addOrder = addOrders[j];
                var queryOrder = queryOrders[j];
                var addedOrder = addedOrders[j];

                var queryOrderDetails = AssertAddOrderData(addOrder, queryOrder);
                var addedOrderDetails = AssertAddOrderData(addOrder, addedOrder);

                var addOrderDetails = addOrder
                    .OrderDetails
                    .ToList();

                for (int k = 0; k < addedOrderDetails.Count; k++)
                {
                    var addOrderDetail = addOrderDetails[k];
                    var queryOrderDetail = queryOrderDetails[k];
                    var addedOrderDetail = addedOrderDetails[k];

                    AssertAddOrderDetailData(addOrderDetail, queryOrderDetail);
                    AssertAddOrderDetailData(addOrderDetail, addedOrderDetail);
                }
            }
        }
    }

    [Fact, Priority(99)]
    public async Task DeleteRangeAsync_ShouldQueryAndDeleteCustomersWithOrderAndDetail_Records()
    {
        // Arrange
        var repository = ServiceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();

        // Action
        var responseQuery = await repository.QueryCustomersAsync(x => x.FirstName.StartsWith("First 10"));
        var queryCustomers = responseQuery.Data as List<CustomerEntity>;

        Assert.NotNull(queryCustomers);
        var customerIds = queryCustomers.Select(x => x.Id).ToList();
        var deleteResponse = await repository.DeleteRangeAsync(queryCustomers);
        var queryDeletedResponse = await repository.QueryCustomersAsync(x => x.FirstName.StartsWith("First 10"));

        // Assert
        Assert.Equal(RepositoryReturnStatusType.Success, deleteResponse.ReturnStatus);
        Assert.Equal(RepositoryReturnStatusType.Success, responseQuery.ReturnStatus);
        Assert.Equal(RepositoryReturnStatusType.NotFound, queryDeletedResponse.ReturnStatus);
    }
    
    #endregion Public Tests

    #region Private Support Functions
    private List<CustomerEntity> CreateCustomerRecords(int count, bool addOrders = false)
    {
        var customers = new List<CustomerEntity>();
        var date = new DateTime(2025, 1, 1);

        count += 100;
        for (int i = 100; i < count; i++)
        {
            var customer = new CustomerEntity()
            {
                FirstName = $"First {i}",
                LastName = $"Last {i}",
                BillingAddress = $"Billing Address {i}",
                ShippingAddress = $"Shipping Address {i}",
                Created = date,
                CreatedBy = "User1",
                LastUpdated = date,
                LastUpdatedBy = "User1"
            };

            RepositoryInitializer.SeedTestCustomer(customer);
            customers.Add(customer);
        }

        return customers;
    }

    private void AssertCustomerData(CustomerEntity customer, int id = 1, int createdDays = 0, int lastUpdatedDays = 0, int userId = 1)
    {
        var date = new DateTime(2025, 1, 1);
        var created = date.AddDays(createdDays);
        var lastUpdated = date.AddDays(lastUpdatedDays);

        Assert.NotNull(customer);

        Assert.Equal(id, customer.Id);
        Assert.Equal($"Last {id}", customer.LastName);
        Assert.Equal($"First {id}", customer.FirstName);
        Assert.Equal($"Billing Address {id}", customer.BillingAddress);
        Assert.Equal($"Shipping Address {id}", customer.ShippingAddress);

        Assert.Equal(created, customer.Created);
        Assert.Equal(lastUpdated, customer.LastUpdated);
        Assert.Equal($"User{userId}", customer.LastUpdatedBy);
    }

    private List<OrderEntity> AssertAddCustomerData(CustomerEntity addCustomer, CustomerEntity customer)
    {
        Assert.Equal(addCustomer.LastName, customer.LastName);
        Assert.Equal(addCustomer.FirstName, customer.FirstName);
        Assert.Equal(addCustomer.BillingAddress, customer.BillingAddress);
        Assert.Equal(addCustomer.ShippingAddress, customer.ShippingAddress);

        Assert.Equal(addCustomer.Created, customer.Created);
        Assert.Equal(addCustomer.CreatedBy, customer.CreatedBy);

        Assert.Equal(addCustomer.LastUpdated, customer.LastUpdated);
        Assert.Equal(addCustomer.LastUpdatedBy, customer.LastUpdatedBy);

        Assert.NotNull(customer.Orders); 
        Assert.NotNull(addCustomer.Orders);

        Assert.Equal(addCustomer.Orders.Count, customer.Orders.Count);

        return customer.Orders.ToList();
    }

    private List<OrderDetailEntity> AssertAddOrderData(OrderEntity addOrder, OrderEntity order)
    {
        Assert.Equal(addOrder.CustomerId, order.CustomerId);
        Assert.Equal(addOrder.OrderDate, order.OrderDate);
        Assert.Equal(addOrder.ShippedDate, order.ShippedDate);

        Assert.Equal(addOrder.Created, order.Created);
        Assert.Equal(addOrder.CreatedBy, order.CreatedBy);
        Assert.Equal(addOrder.LastUpdated, order.LastUpdated);
        Assert.Equal(addOrder.LastUpdatedBy, order.LastUpdatedBy);

        Assert.NotNull(order);
        Assert.NotNull(addOrder);

        Assert.Equal(addOrder.OrderDetails.Count, order.OrderDetails.Count);

        return order.OrderDetails.ToList();
    }

    private void AssertAddOrderDetailData(OrderDetailEntity addOrderDetail, OrderDetailEntity orderDetail)
    {
        Assert.Equal(addOrderDetail.Quantity, orderDetail.Quantity);
        Assert.Equal(addOrderDetail.OrderId, orderDetail.OrderId);
        Assert.Equal(addOrderDetail.ProductId, orderDetail.ProductId);
        Assert.Equal(addOrderDetail.OrderDetailStatus, orderDetail.OrderDetailStatus);

        Assert.Equal(addOrderDetail.Created, orderDetail.Created);
        Assert.Equal(addOrderDetail.CreatedBy, orderDetail.CreatedBy);
        Assert.Equal(addOrderDetail.LastUpdated, orderDetail.LastUpdated);
        Assert.Equal(addOrderDetail.LastUpdatedBy, orderDetail.LastUpdatedBy);
    }

    #endregion Private Support Functions
}