using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApplicationUsingDapper.Models;

namespace WebApplicationUsingDapper.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly IDbConnection _connection;
        public CityRepository(IConfiguration config)
        {
            // Retrieve the connection string
            string connectionString = config.GetConnectionString("DefaultConnection");

            // Create a new SqlConnection using the connection string
            _connection = new SqlConnection(connectionString);
        }
        public List<City> GetAllCityByCountryId(int countryId)
        {
            string sql = "select * from TbleCity where countryId=@countryId";
            return _connection.Query<City>(sql, new { @countryId= countryId }).ToList();
        }
    }
}
