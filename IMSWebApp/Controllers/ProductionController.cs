using IMSWebApp.Function;
using IMSWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IMSWebApp.Controllers
{
    public class ProductionController : Controller
    {
        private readonly ILogger<ProductionController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductionController(ILogger<ProductionController> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        [Authorize]
        [HttpGet]
        [Route("/Production")]
        public async Task<IActionResult> Index()
        {
            var currentMenu = "PRD";
            var userName = CF.DecryptString(_configuration["EKey"], HttpContext.User.FindFirstValue("userName"));
            ViewData["userName"] = userName;
            ViewData["currentMenu"] = currentMenu;
            ViewData["currentMenuName"] = "Production Management";
            ViewData["MenuItems"] = await api.GetSideMenu(_configuration["ApiEndpoint"] + _configuration["MenuItemModulEndpoint"], _configuration["ApiKey"], _httpClientFactory.CreateClient(), new StringContent(JsonConvert.SerializeObject(new SPParameters { USERNAME = userName, DATA = currentMenu }), System.Text.Encoding.UTF8, "application/json"));
            return View();
        }
    }
}
