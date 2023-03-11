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

        public User GetUserByEmailAsync(string emailNo)
        {
            var sql = "select * from TblUsers where emailNo=@emailNo";
            //return _connection.QuerySingleOrDefault<User>(sql,new {@id=id});
            return _connection.Query<User>(sql, new { @emailNo = emailNo }).SingleOrDefault();           
        }

        public User GetUserByPhoneAsync(string phoneNo)
        {
            var sql = "select * from TblUsers where phoneNo=@phoneNo";
            //return _connection.QuerySingleOrDefault<User>(sql,new {@id=id});
            return _connection.Query<User>(sql, new { @phoneNo = phoneNo }).SingleOrDefault();
        }

        public List<User> GetAll()
        {
            var sql = "select * from TblUsers";
            return _connection.Query<User>(sql).ToList();
        }
        public async Task AddUserAsync(User user)
        {       
            var parameters = new DynamicParameters();
            parameters.Add("@gender", user.gender);
            parameters.Add("@fName", user.fName);
            parameters.Add("@lName", user.lName);
            parameters.Add("@phoneNo", user.phoneNo);
            parameters.Add("@emailNo", user.emailNo);
            parameters.Add("@userCity", user.userCity);
            parameters.Add("@userImg", user.userImg);
            parameters.Add("@userCV", user.userCV);
            parameters.Add("@password", user.password);
            parameters.Add("@dob", user.dob,DbType.DateTime);
            await _connection.ExecuteAsync("InsertTblUsersRecordByGender", parameters, commandType: CommandType.StoredProcedure);
        }

        //public void AddUser(User user)
        //{
        //    //CONVERT(datetime, @dob)
        //    var sql =
        //        "insert into TblUsers(fName,lName,phoneNo,emailNo,userCity,userImg,userCV,password,dob) values(@fName,@lName,@phoneNo,@emailNo,@userCity,@userImg,@userCV,@password,@dob)"
        //        + "select cast(SCOPE_IDENTITY() as int)";
        //    var id = _connection.Query<int>(sql, new
        //    {
        //        user.fName,
        //        user.lName,
        //        user.phoneNo,
        //        user.emailNo,
        //        user.userCity,
        //        user.userImg,
        //        user.userCV,
        //        user.password,
        //        user.dob

        //    }).Single();
        //    //user.userId = id;
        //    //return user;
        //}

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
