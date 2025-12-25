using DevExtreme.AspNet.Data;
using IMSWebApp.Function;
using IMSWebApp.Models;
using IMSWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace IMSWebApp.Controllers.MasterData
{
    public class InventoryController : Controller
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public InventoryController(ILogger<InventoryController> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        [Authorize]
        [HttpGet]
        [Route("/MasterData/Inventory")]
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
        [Route("MasterData/Inventory/GetData")]
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
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["InventoryGetListEndpoint"];

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
                    List<InventoryList> dataList = JsonConvert.DeserializeObject<List<InventoryList>>(responseContent);
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
        [Route("MasterData/Inventory/DetailById")]
        public async Task<IActionResult> Details(int id, DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["InventoryByIDEndpoint"];

                var json = JsonConvert.SerializeObject(id);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var inventoryList = JsonConvert.DeserializeObject<List<InventoryList>>(responseContent);
                    return PartialView("_InventoryDetail", inventoryList);
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
        [Route("MasterData/Inventory/Edit")]
        public async Task<IActionResult> EditProduct(int id, DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["InventoryByIDEndpoint"];

                var json = JsonConvert.SerializeObject(id);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var inventoryList = JsonConvert.DeserializeObject<List<InventoryList>>(responseContent);
                    var product = inventoryList.FirstOrDefault(i => i.Id == id);
                    return PartialView("_InventoryEdit", product);
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
        [Route("MasterData/Inventory/UpdInventory")]
        public async Task<IActionResult> UpdInventory()
        {
            try
            {
                var form = await HttpContext.Request.ReadFormAsync();
                if (form == null)
                {
                    return BadRequest("No form data received");
                }

                var sId = Convert.ToInt32(form["Id"]);
                var sBussCode = "";
                var sPlantCode = form["PlantCode"];
                var sInvCode = form["InvCode"];
                var sInvName = form["InvName"];
                var sRelasiCode = form["RelasiCode"];
                var sCSINVName = form["CSINVName"];
                var sInvStatus = form["InvStatus"] == "on" ? true : false;
                var sDiscontinue = form["Discontinue"] == "on" ? true : false;
                var sVendorCode = form["VendorCode"];
                var sBarcode = form["Barcode"];
                var sInvType = form["InvType"];
                var sInvSubType = form["InvSubType"];
                var sBrand = form["Brand"];
                var sSmallUnit = form["SmallUnit"];
                var sLargeUnit = form["LargeUnit"];
                var sCrt = Convert.ToDecimal(form["Crt"]);
                var sFra = Convert.ToDecimal(form["Fra"]);
                var sNorm = Convert.ToDecimal(form["Norm"]);
                var sProcess = 0; //Convert.ToInt32(form["Process"]);
                var sNoMachine = 0; //Convert.ToDecimal(string.IsNullOrEmpty(form["sNoMachine"]) ? 0 : form["sNoMachine"]);
                var sPeople = 0; //Convert.ToDecimal(form["People"]);
                var sSalesPrice = Convert.ToDecimal(form["SalesPrice"]);
                var sBuyPrice = Convert.ToDecimal(form["BuyPrice"]);
                var sInsertUser = CF.DecryptString(_configuration["EKey"], HttpContext.User.FindFirstValue("userName"));


                var inventory = new InventoryList
                {
                    Id = sId,
                    BussCode = sBussCode, 
                    PlantCode = sPlantCode,  
                    InvCode = sInvCode, 
                    InvName = sInvName,  
                    RelasiCode = sRelasiCode, 
                    CSINVName  = sCSINVName,
                    InvStatus = sInvStatus,  
                    Discontinue = sDiscontinue, 
                    VendorCode = sVendorCode,
                    VendorName = "",
                    Barcode = sBarcode,
                    InvType = sInvType,
                    InvTypeDesc = "",
                    InvSubType = sInvSubType,  //InvSubType
                    InvSubTypeDesc = "",
                    Brand = sBrand,
                    BrandDesc = "",
                    SmallUnit = sSmallUnit,
                    LargeUnit= sLargeUnit,
                    Crt = sCrt,
                    Fra = sFra,
                    Norm = sNorm,
                    Process = sProcess,
                    NoMachine = sNoMachine,
                    People = sPeople,
                    CodeBOM = 0,
                    SalesPrice = sSalesPrice,
                    BuyPrice   = sBuyPrice,
                    InsertUser = sInsertUser
                };

                var json = JsonConvert.SerializeObject(inventory);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["InventoryUpdateEndpoint"];

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
        [Route("Masterdata/Inventory/AddInventory")]
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
                var sBussCode = Request.Cookies["IMSWebAppCurrCompany"]?.ToString(); 
                var sPlantCode = Request.Cookies["IMSWebAppCurrBranch"]?.ToString();
                var sInvCode = form["InvCode"];
                var sInvName = form["InvName"];
                var sRelasiCode = form["RelasiCode"];
                var sCSINVName = form["CSINVName"];
                var sInvStatus = true; //form["InvStatus"] == "on" ? true : false;
                var sDiscontinue = false; // form["Discontinue"] == "on" ? true : false;
                var sVendorCode = form["VendorCode"];
                var sBarcode = form["Barcode"];
                var sInvType = form["InvType"];
                var sInvSubType = form["InvSubType"];
                var sBrand = form["Brand"];
                var sSmallUnit = form["SmallUnit"];
                var sLargeUnit = form["LargeUnit"];
                var sCrt = Convert.ToDecimal(form["Crt"]);
                var sFra = Convert.ToDecimal(form["Fra"]);
                var sNorm = Convert.ToDecimal(form["Norm"]);
                var sProcess =  0; //Convert.ToInt32(form["Process"] == "" ? 0 : form["Process"]);
                var sNoMachine = 0; //Convert.ToDecimal(string.IsNullOrEmpty(form["sNoMachine"]) ? 0 : form["sNoMachine"]);
                var sPeople = 0; //Convert.ToDecimal(form["People"] == "" ? 0 : form["People"]);
                var sSalesPrice = Convert.ToDecimal(form["SalesPrice"] == "" ? 0 : form["SalesPrice"]);
                var sBuyPrice = Convert.ToDecimal(form["BuyPrice"] == "" ? 0 : form["BuyPrice"]);
                var sInsertUser = CF.DecryptString(_configuration["EKey"], HttpContext.User.FindFirstValue("userName"));


                var inventory = new InventoryList
                {
                    Id = sId,
                    BussCode = sBussCode,
                    PlantCode = sPlantCode,
                    InvCode = sInvCode,
                    InvName = sInvName,
                    RelasiCode = sRelasiCode,
                    CSINVName = sCSINVName,
                    InvStatus = sInvStatus,
                    Discontinue = sDiscontinue,
                    VendorCode = sVendorCode,
                    VendorName = "",
                    Barcode = sBarcode,
                    InvType = sInvType,
                    InvTypeDesc = "",
                    InvSubType = sInvSubType, 
                    InvSubTypeDesc = "",
                    Brand = sBrand,
                    BrandDesc = "",
                    SmallUnit = sSmallUnit,
                    LargeUnit = sLargeUnit,
                    Crt = sCrt,
                    Fra = sFra,
                    Norm = sNorm,
                    Process = sProcess,
                    NoMachine = sNoMachine,
                    People = sPeople,
                    CodeBOM = 0,
                    SalesPrice = sSalesPrice,
                    BuyPrice = sBuyPrice,
                    InsertUser = sInsertUser
                };

                var json = JsonConvert.SerializeObject(inventory);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["InventoryInsertEndpoint"];

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { message = "Inventory added successfully.!" });
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
        [Route("MasterData/Inventory/Add")]
        public async Task<IActionResult> AddProduct(int id, DataSourceLoadOptions loadOptions)
        {
            try
            {
                return PartialView("_InventoryAdd");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("MasterData/Inventory/DelInventory")]
        public async Task<IActionResult> DelInventory(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["InventoryDeleteEndpoint"];

                HttpResponseMessage response = await client.DeleteAsync(apiUrl + "/" + Convert.ToString(id));

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { message = "Inventory added successfully.!" });
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


        #region "Combo"

        [HttpGet]
        [Route("MasterData/Inventory/GetInvTypes")]
        public async Task<IActionResult> GetInvTypes(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["GetInvTypesEndpoint"];
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
        [Route("MasterData/Inventory/GetInvSubTypes")]
        public async Task<IActionResult> GetInvSubTypes(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["GetInvSubTypesEndpoint"];
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
        [Route("MasterData/Inventory/GetBrand")]
        public async Task<IActionResult> GetBrand(DataSourceLoadOptions loadOptions)
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

            //try
            //{
            //    var client = _httpClientFactory.CreateClient();
            //    string apiKey = _configuration["ApiKey"];
            //    client.DefaultRequestHeaders.Add("ApiKey", apiKey);
            //    string apiUrl = _configuration["ApiEndpoint"] + _configuration["GetBrandsEndpoint"];
            //    HttpResponseMessage response = await client.GetAsync(apiUrl);


            //    if (response.IsSuccessStatusCode)
            //    {
            //        string responseContent = await response.Content.ReadAsStringAsync();
            //        List<SelectedBoxValues> dataList = JsonConvert.DeserializeObject<List<SelectedBoxValues>>(responseContent);
            //        return Ok(DataSourceLoader.Load(dataList, loadOptions));
            //    }
            //    else
            //    {
            //        var errorResponse = await response.Content.ReadAsStringAsync();
            //        return StatusCode((int)response.StatusCode, errorResponse);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            //}
        }

        [HttpGet]
        [Route("MasterData/Inventory/GetVendor")]
        public async Task<IActionResult> GetVendor(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["GetVendorEndpoint"];
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
        [Route("MasterData/Inventory/GetUnit")]
        public async Task<IActionResult> GetUnit(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["GetUnitInvEndpoint"];
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

        #endregion "Combo"

    }
}
