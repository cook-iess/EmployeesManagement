using Microsoft.AspNetCore.Mvc;

namespace EmployeesManagement.Controllers
{
    public class ConfigController : Controller
    {
        private readonly IConfiguration _configuration;
        public ConfigController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            var connString = _configuration.GetConnectionString("DefaultConnection");
            return Content($"Connection String: {connString}");
        }
    }
}