using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Core.CoreLib.Services.Database
{
    public partial class DapperContext
    {
        protected IConfiguration _configuration;
        
        public DapperContext(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_configuration.GetConnectionString("Azure"));
    }
}