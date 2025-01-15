# COTSDemo - Customer Order and Tracking System

This project was written to help me better understand different technologies and to demonstrate the coding standards, styles, methodologies, SOLID principles and different design patterns that I use.

This project is designed around the idea of a simple customer ordering and tracking system.  It is not intended to have deep business logic but more to demonstrate the basic concepts of the technologies chosen to be used in the project.

## Current Status

This demo project is a work in progress.  It is not complete and some parts are not fully working.  The long term goal is to create a web application to provide and end-to-end demo of presenting data to a user, allowing inserts, updates, searching, etc. of customer order data.

At this time the following things have been implemented:

- Abstraction layer containing interfaces, models, entities, enumerators, etc.
- Repository layer using a repository pattern with EF Core for the database CRUD operations
- Services layer to contain any business logic or data retrieval/updates to be called from the API layer
- Unit testing for the repository and customer services layers (or layers added in the future)

This readme will be updated as things are fixed or new features are added.

## Overall design

The design of the COTSDemo project was to create multiple layers based on some of the DDD (Domain Driven Design) and clean architecture patterns.  The design does not fully embrace those patterns but utilizes some of the features and concepts from each at its core

### Current

As mentioned the above things have been implemented:

- Abstraction layer containing interfaces, models, entities, enumerators, etc.
- Repository layer using a repository pattern with EF Core for the database CRUD operations
- Services layer to contain any business logic or data retrieval/updates to be called from the API layer
- Unit testing for the repository and customer services layers

### Future

Future plans are to include the follow additional layers to provide an end-to-end COTSDemo

- API layer containing REST API calls to perform operations such as presenting view models to the UI layer, inserts, updates, searching, etc. of customer order data.
- UI layer allow users to view, modify and remove customer order items
- Integration and business logic testing for layers where it makes sense to that kind of testing
- Add a different repository layer showing how you can easily switch out different types of repositories such as moving from MSSQL to PostgresSQL or even a no-SQL database

### Models and Entities

Each model class contains a ToEntity and ToModel function.  For each model those functions will create or copy properties specific to that model.  The BaseModel class contains functions to copy properties that are common across all models

### Response Objects

Repository and service functions return an object that contains data and information from the calls.  These objects (IRepositoryResponse and IServiceResponse) contain the data that is returned from the call, error/exception data if the called function had some kind of error and additional information when applicable.

The idea is to allow the calling function to take what ever action is necessary depending on what is returned.  For example, if the called function captured and returned an exception then they calling functions can examine the exception details along with any additional information and take what ever actions are necessary.

## Testing Notes

The current repository for data access is using EF Core 9.  The unit testing code was designed is a way that it can use an “In Memory” EF core database or MSSQL database.  In the unit test project you can modify the DatabaseType option to be “InMemory” or “MSSQL” depending if you want to test in memory or directly to the SQL database.

The unit tests are using a nuget package to order unit tests because some tests will add and remove items to the database which will cause different counts depending on the timing of when some tests run.  This will force unit tests to run in order so those items that add/remove items will not interfere with other tests

When testing with the “In Memory” database type the unit test code uses the IClassFixture class for the DbContext to ensure that the in memory database stays in memory until all tests are complete so that all tests will use the same instance of the “In Memory” database.  This is to help with performance issues when running unit tests and help ensure data persists between tests