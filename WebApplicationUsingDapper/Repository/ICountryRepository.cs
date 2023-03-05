using WebApplicationUsingDapper.Models;

namespace WebApplicationUsingDapper.Repository
{
    public interface ICountryRepository
    {
        List<Country> GetAll();
    }
}
