using WebApplicationUsingDapper.Models.Account;

namespace WebApplicationUsingDapper.Repository
{
    public interface IUserRepository
    {
        User FindById(int id);
        List<User> GetAll();
        Task AddUserAsync(User user);
        User UpdateUser(User user);
        void RemoveUser(int id);
    }
}
