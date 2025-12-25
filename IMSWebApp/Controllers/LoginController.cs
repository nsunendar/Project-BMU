using Dapper;
using IMSWebApp.Function;
using IMSWebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;

namespace IMSWebApp.Controllers
{
    public class LoginController : Controller
    {

        //public bool needLogin = true;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private LoginResponse loginResponse;

        public LoginController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            Debug.WriteLine("--------------------------checkLogged--------------------------");

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("", "Home");
            }
            else
            {
                ViewData["Version"] = _configuration["Version"];
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> LoginChecking(string userN, string userP)
        {
            string loginResult = "index";

            loginResult = await CheckDB(userN, userP);

            if (loginResult == "Ok")
            {
                var claims = new List<Claim>
                {
                    new Claim("user", CF.EncryptString(_configuration["EKey"], userN)),
                    new Claim("role", "Member"),
                    new Claim("userName", CF.EncryptString(_configuration["EKey"], userN))
                };
                var claimsIdentity = new ClaimsIdentity(claims, "IMSWebApp");
                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };
                await HttpContext.SignInAsync("IMSWebApp", new ClaimsPrincipal(claimsIdentity), authProperties);
                Response.Cookies.Append("IMSWebAppCurrBranch", "BMU1");
                Response.Cookies.Append("IMSWebAppCurrCompany", "BMU");
            }
            return loginResult;
        }

        private async Task<string> CheckDB(string userName, string userPass)
        {
            Debug.WriteLine("--------------------------Checking DB--------------------------");
            string returnData = "";
            if (string.IsNullOrWhiteSpace(userName))
            {
                returnData = "Mohon isi semua kolom";
            }
            else
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["LoginEndpoint"];
                var loginModel = new LoginModel
                {
                    Username = userName,
                    Password = userPass
                };
                try
                {
                    var json = JsonConvert.SerializeObject(loginModel);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        loginResponse = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());
                        returnData = "Ok";
                    }
                    else
                    {
                        returnData = await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception ex)
                {
                    returnData = "Error accessing API" + ex.Message;
                }
            }
            return returnData;
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<string> LoginChecking(string userN, string userP)
        //{
        //    string loginResult = "index";
        //    if (string.IsNullOrWhiteSpace(userN))
        //    {
        //        loginResult = "Mohon isi semua kolom";
        //    }
        //    else
        //    {
        //        // Use HttpClientFactory to create HttpClient instance
        //        var client = _httpClientFactory.CreateClient();
        //        string apiKey = _configuration["ApiKey"];
        //        string apiUrl = _configuration["ApiEndpoint"] + _configuration["LoginEndpoint"];
        //        var loginModel = new LoginModel
        //        {
        //            Username = userN,
        //            Password = userP
        //        };

        //        try
        //        {
        //            var json = JsonConvert.SerializeObject(loginModel);
        //            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        //            client.DefaultRequestHeaders.Add("ApiKey", apiKey);
        //            HttpResponseMessage response = await client.PostAsync(apiUrl, content);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var claims = new List<Claim>
        //                {
        //                    new Claim("user", CF.EncryptString(_configuration["EKey"], userN)),
        //                    new Claim("role", "Member"),
        //                    new Claim("userName", CF.EncryptString(_configuration["EKey"], userN))
        //                };
        //                var claimsIdentity = new ClaimsIdentity(claims, "IMSWebApp");
        //                var authProperties = new AuthenticationProperties
        //                {
        //                };
        //                await HttpContext.SignInAsync("IMSWebApp", new ClaimsPrincipal(claimsIdentity), authProperties);
        //                Response.Cookies.Append("IMSWebAppCurrBranch", "");
        //                Response.Cookies.Append("IMSWebAppCurrCompany", "");
        //            }
        //            else
        //            {
        //                loginResult = await response.Content.ReadAsStringAsync();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            //returnData = "Error accessing API: " + ex.Message;
        //            loginResult = "Error accessing API";
        //        }
        //    }
        //    return loginResult;
        //}



    }
}
