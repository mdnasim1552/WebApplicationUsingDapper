using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplicationUsingDapper.Models;
using WebApplicationUsingDapper.Models.Account;
using WebApplicationUsingDapper.Models.ViewModel;
using WebApplicationUsingDapper.Repository;

namespace WebApplicationUsingDapper.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICityRepository _cityRepository;

        public AccountController(IConfiguration config)
        {
            _userRepository = new UserRepository(config);
            _countryRepository = new CountryRepository(config);
            _cityRepository = new CityRepository(config);
        }
        public IActionResult Index()
        {
            var users = _userRepository.GetAll();
            return View(users);
        }
        [HttpPost]
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Login(LoginViewModel model)
        {
            return View();
        }
        public IActionResult SignUp()
        {
            var viewModel = new SignUpViewModel
            {
                Countries = _countryRepository.GetAll(),
                Cities = new List<City>()
            };

            return View(viewModel);      
        }
        [HttpPost]
        public IActionResult SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
        
                // Create a new User object with the byte arrays for userImg and userCV
                var data = new User() 
                { 
                    fName = model.fName,
                    lName = model.lName,
                    phoneNo = model.phoneNo,
                    emailNo = model.emailNo,
                    userCity = model.userCity,
                    userImg = model.userImg,
                    userCV = model.userCV,
                    password = model.password,
                    dob = model.dob
                };
        
                // Add the new User object to the database
                _userRepository.AddUser(data);
        
                return RedirectToAction("Login");
            }
            else
            {
                model.Countries= _countryRepository.GetAll();
                model.Cities = new List<City>();
                TempData["ErrorMessage"] = "Empty form cannot be submitted!";
                return View(model);
            }
        }
        [HttpPost]
        public JsonResult LoadCities(int countryId)
        {
            var cities = _cityRepository.GetAllCityByCountryId(countryId);
            var cityItems = cities.Select(c => new SelectListItem
            {
                Text = c.cityName,
                Value = c.cityId.ToString()
            }).ToList();

            return Json(cityItems);
        }
    }
}
