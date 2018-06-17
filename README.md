# Dapper Testing tool

The tool is inspired by the F# type providers. Unfortunately, C# doesn't support them. Due to this fact the tool requires some metadata and manual running as a unit test.  

## Motivation
Raw SQL queries are generally considered as more brittle and less maintainable than LINQ.  
There are 2 types of issues in queries (Raw SQL/ LINQ):

### Syntactic issues

In case of LINQ, if you misspell a column name, or provide an incorrect type of a parameter the C# compiler will fail the build and you will know about the problem in the design time.  
Raw SQL normally is just a string literal and can not be verified in the design time.  

### Logical issues

Both querying approaches can have logical issues. If you selecting the incorrect column or using MAX instead of MIN the compiler can not help you. THe only one way to detect such issues is unit/integration tests.  

### Maintainability
During the evolution of a project some columns might be renamed, or added to the tables. All queries that use those tables must be reviewed and updated. In case or LINQ it's much easier because of syntax highlight, intelly-sense and type safety.  
On the other hand, complex queries are much more intuitive and readable in raw SQL.  
To equalize the maintainability we must find a way to verify the raw SQL queries in the design time without writing integration tests.

## The solution
1. The MSTest framework can run the same test with different parameters. 
2. The tool contains a custom data source for the MSTest framework (`DapperDataSource`).
3. The data source searches for the Dapper queries in the specified assemblies.
4. In the _class initialize_ the tool disables the actual execution of the queries, but keeps SQL server parsing them and checking their correctness (including parameter names).
5. The MSTest framework calls the custom data-set and gets all queries to be injected into the test.
6. The MSTest framework executes the test for each query and the Helper checks the query correctness respectively.
7. In the _class cleanup_ the tool enables the actual execution of the queries again.

Therefore, you have a "unit" test per each Dapper query.

## How to use
1. Decorate the class that contains a SQL query with the `DapperQuery` attribute
2. Create a static object that contains dummy parameters for the queries
3. Specify the _Query name_ and the _Parameters object name_ in the `DapperQuery` attribute
4. If the query contains placeholders (e.g. {0}) you can specify the _names_ of the fields to be injected in the placeholders.
5. Create a unit test that accepts the `QueryContext` object.
6. Decorate this test with the `DataTestMethod` attribute and the `DapperDataSource` attribute.
7. In the `DapperDataSource` specify the assemblies that must be scanned for queries using the types that are located in these assemblies.
8. Call the `await DryRun.ExecuteQuery(ctx)` in the unit test and pass the context injected as a parameter.
9. Call the `await DryRun.EnableSafeMode(ConnectionString)` in the class initialize and pass the connection string.
10. Call the `await DryRun.DisableSafeMode()` in the class cleanup.

## The query metadata
The query contains 2 placeholders, one of them can have 4 values.  
![query_metadata](readme_images/1.png)

## The unit test
This is all you need!  
![2](readme_images/2.png)

## The Resharper test runner
One unit test is generated for each `DapperQuery` attribute.  
You can see the class name, query name, and placeholder values  
![3](readme_images/3.png)

## The Visual Studio test runner
The same is in the Visual Studio.  
![4](readme_images/4.png)
