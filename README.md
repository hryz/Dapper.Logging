# Dapper Logging integration

The tool consists of simple decorators for the `DbConnection` and `DbCommand` which track the execution time and write messages to the `ILogger<T>`. The `ILogger<T>` can be handled by any logging framework (e.g. Serilog).
The result is similar to the default EF Core logging behavior.

The lib declares a helper method for registering the `IDbConnectionFactory` in the IoC container.
The connection factory is SQL Provider agnostic. That's why you have to specify the real factory method:
```
  services.AddDbConnectionFactory(prv => new SqlConnection(conStr)); 
```
After registration, the `IDbConnectionFactory` can be injected into classes that need a SQL connection.  
```
private readonly IDbConnectionFactory _connectionFactory;
public GetProductsHandler(IDbConnectionFactory connectionFactory)
{
    _connectionFactory = connectionFactory;
}
```
The `IDbConnectionFactory.CreateConnection` will return a decorated version that logs the activity.  
```
using (DbConnection db = _connectionFactory.CreateConnection())
{
    //...
}
```
The lifetime of the factory can be configured during the registration in DI.  
Also, you can alter the default messages using the `DbLoggingConfigurationBuilder` and fluent api.

## Samples

The code is simpler than this explanation 🙂. Please check the _Samples_.  
There you can find a comparison of EF Core and Dapper. It should show the similarities in logging behavior.