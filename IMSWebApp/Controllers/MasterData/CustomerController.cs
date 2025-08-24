using DevExtreme.AspNet.Data;
using IMSWebApp.Function;
using IMSWebApp.Models;
using IMSWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;   
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IMSWebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerController(ILogger<CustomerController> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        [Authorize]
        [HttpGet]
        [Route("/MasterData/Customer")]
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
        [Route("MasterData/Customer/GetData")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetData(DataSourceLoadOptions loadOptions)
        {
            try
            {
                string dataVal = HttpContext.Request.Headers["Options"];

                if (string.IsNullOrEmpty(dataVal))
                {
                    return StatusCode(200, "Cannot get Mutation Options is NULL");
                }

                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["CustomerGetListEndpoint"];

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
                    List<CustomerList> dataList = JsonConvert.DeserializeObject<List<CustomerList>>(responseContent);
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
        [Route("MasterData/Customer/DetailById")]
        public async Task<IActionResult> Details(int id, DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["CustomerByIDEndpoint"];

                var json = JsonConvert.SerializeObject(id);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var customerList = JsonConvert.DeserializeObject<List<CustomerList>>(responseContent);
                    var custList = customerList.FirstOrDefault(i => i.Id == id);
                    return PartialView("_CustomerDetails", custList);
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
        [Route("Masterdata/Customer/Add")]
        public IActionResult AddCustomer()
        {
            return PartialView("_CustomerAdd");
        }

        [HttpPost]
        [Route("MasterData/Customer/UpdCustomer")]
        public async Task<IActionResult> UpdCustomer()
        {
            try
            {
                var form = await HttpContext.Request.ReadFormAsync();
                if (form == null)
                {
                    return BadRequest("No form data received");
                }

                var sId = Convert.ToInt64(form["Id"]);
                var sBussCode = form["BussCode"];
                var sPlantCode = form["PlantCode"];
                var sCustCode = form["CustCode"];
                var sCustName = form["CustName"];
                var sCustAddress = form["CustAddress"];
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
                var sCustType = form["CustType"];
                var sCustSubType = form["CustSubType"];
                var sArea = form["Area"];
                var sSalesman = form["Salesman"];
                var sTerm = form["Term"] != "" ? Convert.ToInt64(form["Term"]) : 0;
                var sPriceCode = string.Empty;
                var sJoinDate = string.IsNullOrEmpty(form["JoinDate"]) ? (DateTime?)null : DateTime.Parse(form["JoinDate"]);
                var sInsertUser = CF.DecryptString(_configuration["EKey"], HttpContext.User.FindFirstValue("userName"));


                var customer = new CustomerList
                {
                    Id = sId,
                    BussCode = sBussCode,
                    PlantCode = sPlantCode,
                    CustCode = sCustCode,
                    CustName = sCustName,
                    CustAddress = sCustAddress,
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
                    CustType = sCustType,
                    CustSubType = sCustSubType,
                    Area = sArea,
                    Salesman = sSalesman,
                    Term = sTerm,
                    PriceCode = sPriceCode ,
                    JoinDate = sJoinDate,
                    InsertUser = sInsertUser
                };

                var json = JsonConvert.SerializeObject(customer);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["CustomerUpdateEndpoint"];

                HttpResponseMessage response = await client.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { message = "Inventory updated successfully.!" });
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
        [Route("MasterData/Customer/InsCustomer")]
        public async Task<IActionResult> InsertCustomer()
        {
            try
            {
                var form = await HttpContext.Request.ReadFormAsync();
                if (form == null)
                {
                    return BadRequest("No form data received");
                }

                var sId = Convert.ToInt64(form["Id"]);
                var sBussCode = Request.Cookies["IMSWebAppCurrCompany"]?.ToString();
                var sPlantCode = Request.Cookies["IMSWebAppCurrBranch"]?.ToString();
                var sCustCode = form["CustCode"];
                var sCustName = form["CustName"];
                var sCustAddress = form["CustAddress"];
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
                var sCustType = form["CustType"];
                var sCustSubType = form["CustSubType"];
                var sArea = form["Area"];
                var sSalesman = form["Salesman"];
                var sTerm = form["Term"] != "" ? Convert.ToInt64(form["Term"]) : 0;
                var sPriceCode = string.Empty;
                var sJoinDate = string.IsNullOrEmpty(form["JoinDate"]) ? (DateTime?)null : DateTime.Parse(form["JoinDate"]);
                var sInsertUser = CF.DecryptString(_configuration["EKey"], HttpContext.User.FindFirstValue("userName"));


                var customer = new CustomerList
                {
                    Id = sId,
                    BussCode = sBussCode,
                    PlantCode = sPlantCode,
                    CustCode = sCustCode,
                    CustName = sCustName,
                    CustAddress = sCustAddress,
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
                    CustType = sCustType,
                    CustSubType = sCustSubType,
                    Area = sArea,
                    Salesman = sSalesman,
                    Term = sTerm,
                    PriceCode = sPriceCode,
                    JoinDate = sJoinDate,
                    InsertUser = sInsertUser
                };

                var json = JsonConvert.SerializeObject(customer);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["CustomerInsertEndpoint"];

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { message = "Inventory updated successfully.!" });
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

        [HttpGet]
        [Route("MasterData/Customer/Edit")]
        public async Task<IActionResult> EditCustomer(int id, DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["CustomerByIDEndpoint"];

                var json = JsonConvert.SerializeObject(id);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var custList = JsonConvert.DeserializeObject<List<CustomerList>>(responseContent);
                    var cust = custList.FirstOrDefault(i => i.Id == id);
                    return PartialView("_CustomerEdit", cust);
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
        [Route("MasterData/Customer/GetCustType")]
        public async Task<IActionResult> GetCustType(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["GetCustTypeEndpoint"];
                HttpResponseMessage response = await client.GetAsync(apiUrl);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var invTypeList = JsonConvert.DeserializeObject<List<SelectedBoxValues>>(responseContent);
                    return Ok(invTypeList);
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
        [Route("MasterData/Customer/GetCustSubType")]
        public async Task<IActionResult> GetCustSubType(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["GetCustSubTypeEndpoint"];
                HttpResponseMessage response = await client.GetAsync(apiUrl);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var invTypeList = JsonConvert.DeserializeObject<List<SelectedBoxValues>>(responseContent);
                    return Ok(invTypeList);
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
        [Route("MasterData/Customer/GetCsArea")]
        public async Task<IActionResult> GetCsArea(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["GetCsAreaEndpoint"];
                HttpResponseMessage response = await client.GetAsync(apiUrl);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var invTypeList = JsonConvert.DeserializeObject<List<SelectedBoxValues>>(responseContent);
                    return Ok(invTypeList);
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
        [Route("MasterData/Customer/GetCsSalesman")]
        public async Task<IActionResult> GetCsSalesman(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["GetCsSalesmanEndpoint"];
                HttpResponseMessage response = await client.GetAsync(apiUrl);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var invTypeList = JsonConvert.DeserializeObject<List<SelectedBoxValues>>(responseContent);
                    return Ok(invTypeList);
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
    }
}
