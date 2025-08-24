using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using IMSWebApp.Function;
using IMSWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace IMSWebApp.Controllers.MasterData
{
    public class InventoryBrandController : Controller
    {
        private readonly ILogger<InventoryBrandController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public InventoryBrandController(ILogger<InventoryBrandController> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        [Authorize]
        [HttpGet]
        [Route("/MasterData/InventoryBrand")]
        public async Task<IActionResult> Index()
        {
            var currentMenu = "MDM";
            var userName = CF.DecryptString(_configuration["EKey"], HttpContext.User.FindFirstValue("userName"));
            ViewData["userName"] = userName;
            ViewData["currentMenu"] = currentMenu;
            ViewData["currentMenuName"] = "Master Data Management";
            ViewData["MenuItems"] = await api.GetSideMenu(_configuration["ApiEndpoint"] + _configuration["MenuItemModulEndpoint"], _configuration["ApiKey"], _httpClientFactory.CreateClient(), new StringContent(JsonConvert.SerializeObject(new SPParameters { USERNAME = userName, DATA = currentMenu }), System.Text.Encoding.UTF8, "application/json"));
            return View();
        }

        [Authorize]
        [HttpGet]
        [Route("MasterData/InventoryBrand/GetData")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetDataBrand(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["InvBrandGetDataEndpoint"];
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    List<TMBrand> dataList = JsonConvert.DeserializeObject<List<TMBrand>>(responseContent);
                    return Ok(DataSourceLoader.Load(dataList, loadOptions));
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, errorResponse);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("MasterData/InventoryBrand/AddInvBrand")]
        public async Task<IActionResult> AddInvBrand(DataSourceLoadOptions loadOptions)
        {
            try
            {
                return PartialView("_InventoryBrandAdd");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("MasterData/InventoryBrand/EditInvBrand")]
        public async Task<IActionResult> EditInvBrand(string brandcode, DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["InvBrandGetDataByIdEndpoint"];

                var json = JsonConvert.SerializeObject(brandcode);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var invBrandList = JsonConvert.DeserializeObject<List<TMBrand>>(responseContent);
                    var brand = invBrandList.FirstOrDefault(i => i.BrandCode == brandcode);
                    return PartialView("_InventoryBrandEdit", brand);
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, errorResponse);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("MasterData/InventoryBrand/DeleteInvBrand")]
        public async Task<IActionResult> DeleteInvBrand(string brandcode, DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["InvBrandDeleteEndpoint"];
                HttpResponseMessage response = await client.DeleteAsync(apiUrl +"/"+ brandcode);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return Ok(responseContent);
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, errorResponse);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("MasterData/InventoryBrand/Update")]
        public async Task<IActionResult> UpdInventoryBrand()
        {
            try
            {
                var form = await HttpContext.Request.ReadFormAsync();
                if (form == null)
                {
                    return BadRequest("No form data received");
                }

                var sBrandCode = form["BrandCode"];
                var sBrandDesc = form["BrandDesc"];
                var sInsertUser = CF.DecryptString(_configuration["EKey"], HttpContext.User.FindFirstValue("userName"));

                var brand = new TMBrand
                {
                    BrandCode = sBrandCode,
                    BrandDesc = sBrandDesc,
                    InsertUser = sInsertUser
                };

                var json = JsonConvert.SerializeObject(brand);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["InvBrandUpdateEndpoint"];

                HttpResponseMessage response = await client.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { message = "Brand updated successfully.!" });
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, errorResponse);
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("Masterdata/InventoryBrand/Add")]
        public async Task<IActionResult> AddInventory()
        {
            try
            {
                var form = await HttpContext.Request.ReadFormAsync();
                if (form == null)
                {
                    return BadRequest("No form data received");
                }

                var sBrandCode = form["BrandCode"];
                var sBrandDesc = form["BrandDesc"];
                var sInsertUser = CF.DecryptString(_configuration["EKey"], HttpContext.User.FindFirstValue("userName"));

                var brand = new TMBrand
                {
                    BrandCode = sBrandCode,
                    BrandDesc = sBrandDesc,
                    InsertUser = sInsertUser
                };

                var json = JsonConvert.SerializeObject(brand);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["InvBrandInsertEndpoint"];

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { message = "Brand added successfully.!" });
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, errorResponse);
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("Masterdata/InventoryBrand/Delete")]
        public async Task<IActionResult> DelInventory(string brandcode)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["InvBrandDeleteEndpoint"];

                HttpResponseMessage response = await client.DeleteAsync(apiUrl + "/" + brandcode);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { message = "Brand Deleted successfully.!" });
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, errorResponse);
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
            }
        }

    }
}
