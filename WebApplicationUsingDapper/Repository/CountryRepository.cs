using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApplicationUsingDapper.Models;

namespace WebApplicationUsingDapper.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IDbConnection _connection;
        public CountryRepository(IConfiguration config)
        {
            // Retrieve the connection string
            string connectionString = config.GetConnectionString("DefaultConnection");

            // Create a new SqlConnection using the connection string
            _connection = new SqlConnection(connectionString);

        }
        public List<Country> GetAll()
        {
            string sql = "select * from TblCountry";
            return _connection.Query<Country>(sql).ToList();
        }
    }
}
