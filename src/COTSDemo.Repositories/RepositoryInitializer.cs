using COTSDemo.Abstractions.Entities;
using COTSDemo.Abstractions.Enums;
using Microsoft.EntityFrameworkCore;

namespace COTSDemo.Repositories;

public static class RepositoryInitializer
{
    #region Public Functions

    public static void SeedEfData(ModelBuilder modelBuilder)
    {
        if (modelBuilder == null!)
        {
            throw new ArgumentNullException("Model build is null");
        }

        var products = CreateProductEntities(true);
        modelBuilder?
            .Entity<ProductEntity>()
            .HasData(products);

        var customers = CreateCustomerEntities(true);
        modelBuilder?
            .Entity<CustomerEntity>()
            .HasData(customers);

        var orders = CreateOrderEntities(true);
        modelBuilder?
            .Entity<OrderEntity>()
            .HasData(orders);

        var orderDetails = CreateOrderDetailEntities(true);
        modelBuilder?
            .Entity<OrderDetailEntity>()
                .HasData(orderDetails);
    }

    public static void SeedEfData(COTSDemoDbContext dbContext)
     {
        try
        {
            dbContext.Database.EnsureCreated();
            if (dbContext.Customers.Any())
            {
                return;
            }

            var products = CreateProductEntities(false);
            var customers = CreateCustomerEntities(false);

            var orders = CreateOrderEntities(false);
            var orderDetails = CreateOrderDetailEntities(false);

            dbContext.Products.AddRange(products);
            dbContext.Customers.AddRange(customers);

            dbContext.Orders.AddRange(orders);
            dbContext.OrderDetails.AddRange(orderDetails);

            dbContext.SaveChanges();
        }
        catch 
        {
        }
     }

    public static void SeedTestCustomer(CustomerEntity customer)
    {
        var orders = CreateOrderEntities(false);
        customer.Orders = new List<OrderEntity>();

        foreach (var order in orders)
        {
            order.OrderDetails = new List<OrderDetailEntity>();
            var orderDetails = CreateOrderDetailEntities(false);
            foreach (var orderDetail in orderDetails)
            {
                order.OrderDetails.Add(orderDetail);
            }

            customer.Orders.Add(order);
        }
    }

    #endregion Public Functions

    #region Private Support Functions

    // About the test data:
    // A minimal amount of test data is needed to ensure that the repository is working correctly.
    // For repository testing we are more concerned about accurate retrieval and insert/update of data
    // than business logic.
    //
    // Testing of service/business logic will include adding/updating orders and details
    // to ensure those things are working correctly
    //
    // NOTE: As business logic changes the number of products, orders, etc. and their values
    //       may need to change as ensure all business functionaly can be tested
    // 
    // This data contains:
    //  5 Products with different names, prices quantity on hand, date/user created and updated
    //  5 Customers with different names, billing address, shipping address, date/user created and updated
    //  5 Orders all for customer #1 with different values to ensure queries return correct items
    //  A varied number of order details for each order with different values to ensure queries 

    private static IEnumerable<ProductEntity> CreateProductEntities(bool setId)
    {
        var dateCreated = new DateTime(2025, 1, 1);
        var dateCreated2 = new DateTime(2025, 2, 1);
        var products = new List<ProductEntity>
        {
            new() {Id = setId ? 1 : 0, Name = "Product 1", Price = 1.00M, QuantityOnHand = 10, Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User1"},
            new() {Id = setId ? 2 : 0, Name = "Product 2", Price = 2.21M, QuantityOnHand = 10, Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User1"},
            new() {Id = setId ? 3 : 0, Name = "Product 3", Price = 3.32M, QuantityOnHand = 25, Created = dateCreated.AddDays(3), CreatedBy = "User1", LastUpdated = dateCreated.AddDays(5), LastUpdatedBy = "User1"},
            new() {Id = setId ? 4 : 0, Name = "Product 4", Price = 4.50M, QuantityOnHand = 1,  Created = dateCreated.AddDays(5), CreatedBy = "User1", LastUpdated = dateCreated.AddDays(10), LastUpdatedBy = "User2"},
            new() {Id = setId ? 5 : 0, Name = "Product 5", Price = 4.75M, QuantityOnHand = 1,  Created = dateCreated.AddDays(6), CreatedBy = "User1", LastUpdated = dateCreated.AddDays(10), LastUpdatedBy = "User2"},
        };

        return products;
    }

    private static IEnumerable<CustomerEntity> CreateCustomerEntities(bool setId)
    {
        var dateCreated = new DateTime(2025, 1, 1);
        var customers = new List<CustomerEntity>
        {
            new() {Id = setId ? 1 : 0, FirstName = "First 1", LastName = "Last 1", BillingAddress = "Billing Address 1", ShippingAddress = "Shipping Address 1", Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User1"},
            new() {Id = setId ? 2 : 0, FirstName = "First 2", LastName = "Last 2", BillingAddress = "Billing Address 2", ShippingAddress = "Shipping Address 2", Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User1"},
            new() {Id = setId ? 3 : 0, FirstName = "First 3", LastName = "Last 3", BillingAddress = "Billing Address 3", ShippingAddress = "Shipping Address 3", Created = dateCreated.AddDays(3), CreatedBy = "User1", LastUpdated = dateCreated.AddDays(5), LastUpdatedBy = "User1"},
            new() {Id = setId ? 4 : 0, FirstName = "First 4", LastName = "Last 4", BillingAddress = "Billing Address 4", ShippingAddress = "Shipping Address 4", Created = dateCreated.AddDays(5), CreatedBy = "User1", LastUpdated = dateCreated.AddDays(10), LastUpdatedBy = "User2"},
            new() {Id = setId ? 5 : 0, FirstName = "First 5", LastName = "Last 5", BillingAddress = "Billing Address 5", ShippingAddress = "Shipping Address 5", Created = dateCreated.AddDays(6), CreatedBy = "User1", LastUpdated = dateCreated.AddDays(10), LastUpdatedBy = "User2"},
        };

        return customers;
    }

    private static IEnumerable<OrderEntity> CreateOrderEntities(bool setId)
    {
        var dateCreated = new DateTime(2025, 1, 1);
        var dateCreated2 = new DateTime(2025, 2, 1);

        var orders = new List<OrderEntity>
        {
            new() {Id = setId ? 1 : 0, CustomerId = 1, OrderDate = dateCreated, ShippedDate = null, Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User1"},
            new() {Id = setId ? 2 : 0, CustomerId = 1, OrderDate = dateCreated, ShippedDate = dateCreated.AddDays(5), Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User1"},
            new() {Id = setId ? 3 : 0, CustomerId = 1, OrderDate = dateCreated.AddDays(3), ShippedDate = dateCreated.AddDays(5), Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated.AddDays(3), LastUpdatedBy = "User1"},
            new() {Id = setId ? 4 : 0, CustomerId = 1, OrderDate = dateCreated2.AddDays(5), ShippedDate = null, Created = dateCreated2, CreatedBy = "User1", LastUpdated = dateCreated2.AddDays(6), LastUpdatedBy = "User2"},
            new() {Id = setId ? 5 : 0, CustomerId = 1, OrderDate = dateCreated2.AddDays(6), ShippedDate = dateCreated2.AddDays(7), Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated2.AddDays(7), LastUpdatedBy = "User2"},

            new() {Id = setId ? 6 : 0, CustomerId = 2, OrderDate = dateCreated, ShippedDate = null, Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User1"},
            new() {Id = setId ? 7 : 0, CustomerId = 2, OrderDate = dateCreated, ShippedDate = dateCreated.AddDays(5), Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User1"},
            new() {Id = setId ? 8 : 0, CustomerId = 2, OrderDate = dateCreated.AddDays(3), ShippedDate = dateCreated.AddDays(5), Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated.AddDays(3), LastUpdatedBy = "User1"},
            new() {Id = setId ? 9 : 0, CustomerId = 2, OrderDate = dateCreated2.AddDays(5), ShippedDate = null, Created = dateCreated2, CreatedBy = "User1", LastUpdated = dateCreated2.AddDays(6), LastUpdatedBy = "User2"},
            new() {Id = setId ? 10: 0, CustomerId = 2, OrderDate = dateCreated2.AddDays(6), ShippedDate = dateCreated2.AddDays(7), Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated2.AddDays(7), LastUpdatedBy = "User2"},

        };

        return orders;
    }

    private static IEnumerable<OrderDetailEntity> CreateOrderDetailEntities(bool setId)
    {
        var dateCreated = new DateTime(2025, 1, 1);
        var orderDetails = new List<OrderDetailEntity>
        {
            // customer 1
            new()  {Id = setId ? 1 : 0, Quantity = 5, OrderId = 1, ProductId = 1, OrderDetailStatus = OrderDetailStatusType.InProgress, Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User1" },
            new()  {Id = setId ? 2 : 0, Quantity = 1, OrderId = 1, ProductId = 2, OrderDetailStatus = OrderDetailStatusType.Fulfilled,  Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User1" },
            new()  {Id = setId ? 3 : 0, Quantity = 100, OrderId = 1, ProductId = 3, OrderDetailStatus = OrderDetailStatusType.Fulfilled, Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User2" },

            new()  {Id = setId ? 4 : 0, Quantity = 1, OrderId = 2, ProductId = 1, OrderDetailStatus = OrderDetailStatusType.Fulfilled, Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User1" },
            new()  {Id = setId ? 5 : 0, Quantity = 8, OrderId = 2, ProductId = 3, OrderDetailStatus = OrderDetailStatusType.Fulfilled, Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User1" },

            new()  {Id = setId ? 6 : 0, Quantity = 1, OrderId = 5, ProductId = 1, OrderDetailStatus = OrderDetailStatusType.Backorder, Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User1" },

            // customer 2
            new()  {Id = setId ? 7 : 0, Quantity = 5, OrderId = 6, ProductId = 1, OrderDetailStatus = OrderDetailStatusType.InProgress, Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User1" },
            new()  {Id = setId ? 8 : 0, Quantity = 1, OrderId = 6, ProductId = 2, OrderDetailStatus = OrderDetailStatusType.Fulfilled,  Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User1" },
            new()  {Id = setId ? 9 : 0, Quantity = 100, OrderId = 6, ProductId = 3, OrderDetailStatus = OrderDetailStatusType.Fulfilled, Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User2" },

            new()  {Id = setId ? 10: 0, Quantity = 1, OrderId = 7, ProductId = 1, OrderDetailStatus = OrderDetailStatusType.Fulfilled, Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User1" },
            new()  {Id = setId ? 11: 0, Quantity = 8, OrderId = 7, ProductId = 3, OrderDetailStatus = OrderDetailStatusType.Fulfilled, Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User1" },

            new()  {Id = setId ? 12: 0, Quantity = 1, OrderId = 10, ProductId = 1, OrderDetailStatus = OrderDetailStatusType.Backorder, Created = dateCreated, CreatedBy = "User1", LastUpdated = dateCreated, LastUpdatedBy = "User1" },

        };

        return orderDetails;
    }

    #endregion Private Support Functions
}