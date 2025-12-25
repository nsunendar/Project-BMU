using IMSWebApp.Function;
using IMSWebApp.Models;
using IMSWebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IMSWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGlobalService _globalService;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory, IGlobalService globalService)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _globalService = globalService;
        }

        [Authorize]
        [HttpGet]
        [Route("/Home")]
        public async Task<IActionResult> Index()
        {
            ViewData["isSidebarVisible"] = false;
            ViewData["userName"] = CF.DecryptString(_configuration["EKey"], HttpContext.User.FindFirstValue("userName"));
            ViewData["ModulList"] = await GetMainMenu();
            return View();
        }

        private async Task<List<MenuModul>> GetMainMenu()
        {
            string apiKey = _configuration["ApiKey"];
            string apiUrl = _configuration["ApiEndpoint"] + _configuration["MenuModulEndpoint"];
            List<MenuModul>? MenuList = null;

            var user = new LoginModel
            {
                Username = CF.DecryptString(_configuration["EKey"], HttpContext.User.FindFirstValue("userName"))
            };

            try
            {
                var client = _httpClientFactory.CreateClient();
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // Add the API key to the request headers
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);

                // Send a POST request to the API endpoint
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MenuList = JsonConvert.DeserializeObject<List<MenuModul>>(responseContent);
                }
            }
            catch (Exception ex)
            {
                //returnData = "Error accessing API: " + ex.Message;
                //returnData = "Error accessing API";
            }

            return MenuList;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> SetActiveBranch(string branchCode)
        {
            Response.Cookies.Append("IMSWebAppCurrBranch", branchCode);
            Response.Cookies.Append("IMSWebAppCurrCompany", "BMU");
            return "Ok";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> GetActiveBranch()
        {
            return Request.Cookies["IMSWebAppCurrBranch"].ToString();
        }


        //[Authorize]
        public async Task<IActionResult>Logout()
        {
            await HttpContext.SignOutAsync("IMSWebApp");
            Response.Cookies.Delete("IMSWebAppCurrBranch");
            Response.Cookies.Delete("IMSWebAppCurrCompany");
            return RedirectToAction("Index", "Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View();
        }
    }
}
