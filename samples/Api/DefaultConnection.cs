using Data.Abstract;
using Microsoft.Extensions.Configuration;

namespace Api
{
    public class DefaultConnection : IConnectionString
    {
        private readonly IConfiguration _configuration;

        public DefaultConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Value => _configuration.GetConnectionString("DefaultConnection");
    }
}
