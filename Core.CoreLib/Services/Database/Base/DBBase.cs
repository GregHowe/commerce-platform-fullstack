
using Dapper;

namespace Core.CoreLib.Services.Database.Base
{
    public class DBBase
    {
        private DapperContext _context;

        public DBBase(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> ExecuteQuery<T>(string query)
        {
            using (var connection = _context.CreateConnection())
                return await connection.QueryAsync<T>(query);
        }

        public async Task<IEnumerable<T>> ExecuteQuery<T>(string query, object parameters)
        {
            using (var connection = _context.CreateConnection())
                return await connection.QueryAsync<T>(query, parameters);
        }

        public async Task<int> ExecuteQuery(string query)
        {
            using (var connection = _context.CreateConnection())
                return await connection.ExecuteAsync(query);
        }

        public async Task<int> ExecuteQuery(string query, object parameters)
        {
            using (var connection = _context.CreateConnection())
                return await connection.ExecuteAsync(query, parameters);
        }

        public async Task<string> ExecuteScalarQuery(string query)
        {
            using (var connection = _context.CreateConnection())
                return await connection.ExecuteScalarAsync<string>(query);
        }

        public async Task<string> ExecuteScalarQuery(string query, object parameters)
        {
            using (var connection = _context.CreateConnection())
                return await connection.ExecuteScalarAsync<string>(query, parameters);
        }
    }
}