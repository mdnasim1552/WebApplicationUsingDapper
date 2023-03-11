using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(IConfiguration config, IWebHostEnvironment webHostEnvironment)
        {
            _userRepository = new UserRepository(config);
            _countryRepository = new CountryRepository(config);
            _cityRepository = new CityRepository(config);
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var users = _userRepository.GetAll();
            return View(users);
        }
        
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.GetAll().Where(u => u.emailNo == model.emailNo).SingleOrDefault();
                if (user != null)
                {
                    bool passwordMatches = BCrypt.Net.BCrypt.Verify(model.password, user.password);
                    if (passwordMatches)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.userId),
                            new Claim(ClaimTypes.Name, $"{user.fName} {user.lName}")
                        };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = model.IsRemember
                        };
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        return RedirectToAction("Index", "Account");
                    }
                    else
                    {
                        TempData["ErrorPassword"] = "Invalid password!";
                        return View(model);
                    }
                }
                else
                {
                    TempData["ErrorUsername"] = "Username not found";
                    return View(model);
                }
            }
            else
            {
                return View();
            }   
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
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
                if (_userRepository.GetUserByPhoneAsync(model.phoneNo) != null)
                {
                    ModelState.AddModelError("phoneNo", "Phone number already exists");
                    model.Countries = _countryRepository.GetAll();
                    return View(model);
                }
                if (_userRepository.GetUserByEmailAsync(model.emailNo) != null)
                {
                    ModelState.AddModelError("emailNo", "Email already exists");
                    model.Countries = _countryRepository.GetAll();
                    return View(model);
                }
                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.password, salt);

                // Create a new User object with the byte arrays for userImg and userCV
                var data = new User() 
                { 
                    gender=model.gender,
                    fName = model.fName,
                    lName = model.lName,
                    phoneNo = model.phoneNo,
                    emailNo = model.emailNo,
                    userCity = model.userCity,
                    userImg = UploadUserImage(model.userImg),
                    userCV = UploadUserCV(model.userCV),
                    password = hashedPassword,
                    dob = model.dob
                };  
                _userRepository.AddUserAsync(data);
        
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
        private string UploadUserImage(IFormFile userImg)
        {
            string uniqueFileName=string.Empty;
            if (userImg != null)
            {
                var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var imageExtension = Path.GetExtension(userImg.FileName);
                if (!allowedImageExtensions.Contains(imageExtension.ToLower()))
                {
                    throw new ArgumentException("Invalid image type.");
                }
                var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Content/UserImage");
                uniqueFileName=Guid.NewGuid().ToString()+"_"+userImg.FileName;
                var filePath=Path.Combine(uploadFolder, uniqueFileName);
                using(var fileStream=new FileStream(filePath,FileMode.Create))
                {
                    userImg.CopyTo(fileStream);
                }

            }
            return uniqueFileName;
        }
        private string UploadUserCV(IFormFile userCV)
        {
            string uniqueFileName = string.Empty;
            if (userCV != null)
            {
                var allowedFileExtensions = new[] { ".pdf", ".doc", ".docx" };
                var extension = Path.GetExtension(userCV.FileName);
                if (!allowedFileExtensions.Contains(extension.ToLower()))
                {
                    throw new ArgumentException("Invalid file type.");
                }

                var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Content/UserCV");
                uniqueFileName = Guid.NewGuid().ToString() +"_"+ userCV.FileName;
                var filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    userCV.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
