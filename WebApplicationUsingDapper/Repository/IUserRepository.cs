using WebApplicationUsingDapper.Models.Account;

namespace WebApplicationUsingDapper.Repository
{
    public interface IUserRepository
    {
        User FindById(int id);
        List<User> GetAll();
        Task AddUserAsync(User user);
        User GetUserByEmailAsync(string emailNo);
        User GetUserByPhoneAsync(string phoneNo);

        User UpdateUser(User user);
        void RemoveUser(int id);
    }
}
