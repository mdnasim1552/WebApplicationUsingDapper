using System.Data;
using Dapper;
using WebApplicationUsingDapper.Models.Account;
using Microsoft.Data.SqlClient;

namespace WebApplicationUsingDapper.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IConfiguration config)
        {
            // Retrieve the connection string
            string connectionString = config.GetConnectionString("DefaultConnection");

            // Create a new SqlConnection using the connection string
            _connection = new SqlConnection(connectionString);
        }

        public User FindById(int id)
        {
            var sql = "select * from TblUsers where userId=@id";
            //return _connection.QuerySingleOrDefault<User>(sql,new {@id=id});
            return _connection.Query<User>(sql, new { @id = id }).Single();

        }

        public List<User> GetAll()
        {
            var sql = "select * from TblUsers";
            return _connection.Query<User>(sql).ToList();
        }

        public void AddUser(User user)
        {
            var sql =
                "insert into TblUsers(fName,lName,phoneNo,emailNo,userCity,userImg,userCV,password,dob) values(@fName,@lName,@phoneNo,@emailNo,@userCity,@userImg,@userCV,@password,CONVERT(datetime, @dob))"
                + "select cast(SCOPE_IDENTITY() as int)";
            var id = _connection.Query<int>(sql, new
            {
                user.fName,
                user.lName,
                user.phoneNo,
                user.emailNo,
                user.userCity,
                user.userImg,
                user.userCV,
                user.password,
                user.dob

            }).Single();
            //user.userId = id;
            //return user;
        }

        public User UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public void RemoveUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}
