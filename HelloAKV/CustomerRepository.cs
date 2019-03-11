using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace HelloAKV
{
    public class CustomerRepository
    {   
        private readonly string _connectionString;

        public CustomerRepository(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _connectionString = configuration["AdventureWorksDb"];
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ArgumentNullException("The connection string is empty or null");
            }
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            const string query = "SELECT * FROM [SalesLT].[Customer]";

            using (var connection = new SqlConnection(_connectionString))
            {
                var customers = await connection.QueryAsync<Customer>(query).ConfigureAwait(false);
                return customers?.ToList();
            }
        }

    }
}
