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
    public class VendorController : Controller
    {
        private readonly ILogger<VendorController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public VendorController(ILogger<VendorController> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        [Authorize]
        [HttpGet]
        [Route("/MasterData/Vendor")]
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
        [Route("MasterData/Vendor/GetData")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetDataVendor(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["VendorGetDataEndpoint"];

                string dataVal = HttpContext.Request.Headers["Options"];
                if (string.IsNullOrEmpty(dataVal))
                {
                    return StatusCode(200, "Cannot get  Options is NULL");
                }
                var param = new SPParameters
                {
                    USERNAME = CF.DecryptString(_configuration["EKey"], HttpContext.User.FindFirstValue("userName")),
                    ACTIVEBRANCH = "BMU",
                    DATA = dataVal,
                };
                var json = JsonConvert.SerializeObject(param);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    List<TMVendor> dataList = JsonConvert.DeserializeObject<List<TMVendor>>(responseContent);
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
        [Route("MasterData/Vendor/AddVendor")]
        public async Task<IActionResult> AddVendor(DataSourceLoadOptions loadOptions)
        {
            try
            {
                return PartialView("_VendorAdd");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("MasterData/Vendor/EditVendor")]
        public async Task<IActionResult> EditVendor(string vendorcode, DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["VendorGetByIdEndpoint"];

                var json = JsonConvert.SerializeObject(vendorcode);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var vendorList = JsonConvert.DeserializeObject<List<TMVendor>>(responseContent);
                    var vendor = vendorList.FirstOrDefault(i => i.VendorCode == vendorcode);
                    return PartialView("_VendorEdit", vendor);
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
        [Route("MasterData/Vendor/DeleteVendor")]
        public async Task<IActionResult> DeleteVendor(string vendorcode, DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["InvBrandDeleteEndpoint"];
                HttpResponseMessage response = await client.DeleteAsync(apiUrl + "/" + vendorcode);

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
        [Route("MasterData/Vendor/Update")]
        public async Task<IActionResult> UpdInventoryBrand()
        {
            try
            {
                var form = await HttpContext.Request.ReadFormAsync();
                if (form == null)
                {
                    return BadRequest("No form data received");
                }

                var sId = Convert.ToInt32(form["Id"]);
                var sBussCode = form["BussCode"];
                var sPlantCode = form["PlantCode"];
                var sVendorCode = form["VendorCode"];
                var sVendorName = form["VendorName"];
                var sVendorAddress = form["VendorAddress"];
                var sCity = form["City"];
                var sStatus = form["Status"] == "on" ? true : false;
                var sPhone = form["Phone"];
                var sEmail = form["Email"];
                var sOwnerName = form["OwnerName"];
                var sPKP = form["PKP"] == "on" ? true : false;
                var sTaxName = form["TaxName"];
                var sTaxAddress = form["TaxAddress"];
                var sTaxCity = form["TaxCity"];
                var sNPWP = form["NPWP"];
                var sVendorType = form["VendorType"];
                var sTerm = Convert.ToInt32(form["Term"]);
                var sLeadTime = Convert.ToInt32(form["LeadTime"]);
                var sPriceCode = form["PriceCode"];
                var sInsertUser = CF.DecryptString(_configuration["EKey"], HttpContext.User.FindFirstValue("userName"));

                var vendor = new TMVendor
                {
                    Id = sId,
                    BussCode = sBussCode,
                    PlantCode = sPlantCode,
                    VendorCode = sVendorCode,
                    VendorName = sVendorName,
                    VendorAddress = sVendorAddress,
                    City = sCity,
                    Status = sStatus,
                    Phone = sPhone,
                    Email = sEmail,
                    OwnerName = sOwnerName,
                    PKP = sPKP,
                    TaxName = sTaxName,
                    TaxAddress = sTaxAddress,
                    TaxCity = sTaxCity,
                    NPWP = sNPWP,
                    VendorType = sVendorType,
                    Term = sTerm,
                    LeadTime = sLeadTime,
                    PriceCode = sPriceCode,
                    InsertUser = sInsertUser
                };

                var json = JsonConvert.SerializeObject(vendor);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["VendorUpdateEndpoint"];

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
        [Route("Masterdata/Vendor/Insert")]
        public async Task<IActionResult> AddInventory()
        {
            try
            {
                var form = await HttpContext.Request.ReadFormAsync();
                if (form == null)
                {
                    return BadRequest("No form data received");
                }

                var sId = Convert.ToInt32(form["Id"]);
                var sBussCode = form["BussCode"];
                var sPlantCode = form["PlantCode"];
                var sVendorCode = form["VendorCode"];
                var sVendorName = form["VendorName"];
                var sVendorAddress = form["VendorAddress"];
                var sCity = form["City"];
                var sStatus = form["Status"] == "on" ? true : false;
                var sPhone = form["Phone"];
                var sEmail = form["Email"];
                var sOwnerName = form["OwnerName"];
                var sPKP = form["PKP"] == "on" ? true : false;
                var sTaxName = form["TaxName"];
                var sTaxAddress = form["TaxAddress"];
                var sTaxCity = form["TaxCity"];
                var sNPWP = form["NPWP"];
                var sVendorType = form["VendorType"];
                var sTerm = Convert.ToInt32(form["Term"]);
                var sLeadTime = Convert.ToInt32(form["LeadTime"]);
                var sPriceCode = form["PriceCode"];
                var sInsertUser = CF.DecryptString(_configuration["EKey"], HttpContext.User.FindFirstValue("userName"));

                var vendor = new TMVendor
                {
                    Id = sId,
                    BussCode = sBussCode,
                    PlantCode = sPlantCode,
                    VendorCode = sVendorCode,
                    VendorName = sVendorName,
                    VendorAddress = sVendorAddress,
                    City = sCity,
                    Status = sStatus,
                    Phone = sPhone,
                    Email = sEmail,
                    OwnerName = sOwnerName,
                    PKP = sPKP,
                    TaxName = sTaxName,
                    TaxAddress = sTaxAddress,
                    TaxCity = sTaxCity,
                    NPWP = sNPWP,
                    VendorType = sVendorType,
                    Term = sTerm,
                    LeadTime = sLeadTime,
                    PriceCode = sPriceCode,
                    InsertUser = sInsertUser
                };

                var json = JsonConvert.SerializeObject(vendor);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["VendorInsertEndpoint"];

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
        [Route("Masterdata/Vendor/Delete")]
        public async Task<IActionResult> DelInventory(string vendorcode)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["VendorDeleteEndpoint"];

                HttpResponseMessage response = await client.DeleteAsync(apiUrl + "/" + vendorcode);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { message = "Vendor Deleted successfully.!" });
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
