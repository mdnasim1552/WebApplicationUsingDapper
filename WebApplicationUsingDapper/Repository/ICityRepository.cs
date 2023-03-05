using WebApplicationUsingDapper.Models;

namespace WebApplicationUsingDapper.Repository
{
    public interface ICityRepository
    {
        List<City> GetAllCityByCountryId(int countryId);
    }
}
