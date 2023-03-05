using WebApplicationUsingDapper.Models.Account;

namespace WebApplicationUsingDapper.Repository
{
    public interface IUserRepository
    {
        User FindById(int id);
        List<User> GetAll();
        void AddUser(User user);
        User UpdateUser(User user);
        void RemoveUser(int id);
    }
}
