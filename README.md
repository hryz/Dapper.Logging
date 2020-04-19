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

### How to start

Run `docker-compose build` , and then `docker-compose up`  
This will spin up a `PostgreSQL`, `ASP.Net Core API`, `Elastic`, `Kibana`  
Of course, you can start services selectively (e.g. only DB `docker-compose up db`)  
If you'd like to play with **ELK**, you should do the following:
1. start everything in docker compose
2. wait approximately 1 min for it to start 
    (you can check elastic by `http:localhost:9200` and Kibana by `http://localhost:5601`)
3. run a few queries for API to fill the elastic index:
    (`http://localhost:5000/swagger`)
4. open Kibana and create an index pattern:  
    4.1 Management (gear icon) -> Index Patterns  
    4.2 Create a pattern `logstash-`  
    4.3 Select the timestamp field `@timestamp` from the dropdown menu  
    4.4 Click **Create index**  
5. enjoy playing with Kibana and structured logging