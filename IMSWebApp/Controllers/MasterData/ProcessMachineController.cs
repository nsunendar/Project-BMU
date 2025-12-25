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
    public class ProcessMachineController : Controller
    {
        private readonly ILogger<ProcessMachineController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProcessMachineController(ILogger<ProcessMachineController> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        [Authorize]
        [HttpGet]
        [Route("/MasterData/ProcessMachine")]
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
        [Route("MasterData/ProcessMachine/GetData")]
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
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["ProcessGetEndpoint"];

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
                    List<TMProcess> dataList = JsonConvert.DeserializeObject<List<TMProcess>>(responseContent);
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
        [Route("MasterData/ProcessMachine/Edit")]
        public async Task<IActionResult> EditProcess(int id, DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["ProcessByIdEndpoint"];

                var json = JsonConvert.SerializeObject(id);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var inventoryList = JsonConvert.DeserializeObject<List<TMProcess>>(responseContent);
                    var procMchn = inventoryList.FirstOrDefault(i => i.Id == id);
                    return PartialView("_ProcessMachineEdit", procMchn);
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
        [Route("MasterData/ProcessMachine/Add")]
        public async Task<IActionResult> AddProcess(DataSourceLoadOptions loadOptions)
        {
            try
            {
                return PartialView("_ProcessMachineAdd");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //[HttpPost]
        //[Route("Masterdata/Inventory/UpdateInventory")]
        //public async Task<IActionResult> UpdateInventory()
        //{
        //    try
        //    {
        //        var form = await HttpContext.Request.ReadFormAsync();
        //        if (form == null)
        //        {
        //            return BadRequest("No form data received");
        //        }

        //        var sId = Convert.ToInt32(form["Id"]);
        //        var sBussCode = "";
        //        var sPlantCode = form["PlantCode"];
        //        var sInvCode = form["InvCode"];
        //        var sInvName = form["InvName"];
        //        var sRelasiCode = form["RelasiCode"];
        //        var sCSINVName = form["CSINVName"];
        //        var sInvStatus = form["InvStatus"] == "on" ? true : false;
        //        var sDiscontinue = form["Discontinue"] == "on" ? true : false;
        //        var sVendorCode = form["VendorCode"];
        //        var sBarcode = form["Barcode"];
        //        var sInvType = form["InvType"];
        //        var sInvSubType = form["InvSubType"];
        //        var sBrand = form["Brand"];
        //        var sSmallUnit = form["SmallUnit"];
        //        var sLargeUnit = form["LargeUnit"];
        //        var sCrt = Convert.ToDecimal(form["Crt"]);
        //        var sFra = Convert.ToDecimal(form["Fra"]);
        //        var sNorm = Convert.ToDecimal(form["Norm"]);
        //        var sProcess = Convert.ToInt32(form["Process"]);
        //        var sNoMachine = Convert.ToDecimal(string.IsNullOrEmpty(form["sNoMachine"]) ? 0 : form["sNoMachine"]);
        //        var sPeople = Convert.ToDecimal(form["People"]);
        //        var sSalesPrice = Convert.ToDecimal(form["SalesPrice"]);
        //        var sBuyPrice = Convert.ToDecimal(form["BuyPrice"]);
        //        var sInsertUser = CF.DecryptString(_configuration["EKey"], HttpContext.User.FindFirstValue("userName"));


        //        var inventory = new InventoryList
        //        {
        //            Id = sId,
        //            BussCode = sBussCode,
        //            PlantCode = sPlantCode,
        //            InvCode = sInvCode,
        //            InvName = sInvName,
        //            RelasiCode = sRelasiCode,
        //            CSINVName = sCSINVName,
        //            InvStatus = sInvStatus,
        //            Discontinue = sDiscontinue,
        //            VendorCode = sVendorCode,
        //            VendorName = "",
        //            Barcode = sBarcode,
        //            InvType = sInvType,
        //            InvTypeDesc = "",
        //            InvSubType = sInvSubType,  //InvSubType
        //            InvSubTypeDesc = "",
        //            Brand = sBrand,
        //            BrandDesc = "",
        //            SmallUnit = sSmallUnit,
        //            LargeUnit = sLargeUnit,
        //            Crt = sCrt,
        //            Fra = sFra,
        //            Norm = sNorm,
        //            Process = sProcess,
        //            NoMachine = sNoMachine,
        //            People = sPeople,
        //            CodeBOM = 0,
        //            SalesPrice = sSalesPrice,
        //            BuyPrice = sBuyPrice,
        //            InsertUser = sInsertUser
        //        };

        //        var json = JsonConvert.SerializeObject(inventory);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //        var client = _httpClientFactory.CreateClient();
        //        string apiKey = _configuration["ApiKey"];
        //        client.DefaultRequestHeaders.Add("ApiKey", apiKey);
        //        string apiUrl = _configuration["ApiEndpoint"] + _configuration["InventoryUpdateEndpoint"];

        //        HttpResponseMessage response = await client.PutAsync(apiUrl, content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return Ok(new { message = "Inventory added successfully.!" });
        //        }
        //        else
        //        {
        //            var errorResponse = await response.Content.ReadAsStringAsync();
        //            return StatusCode((int)response.StatusCode, errorResponse);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception if needed
        //        return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
        //    }
        //}

        //[HttpPost]
        //[Route("Masterdata/Inventory/AddInventory")]
        //public async Task<IActionResult> AddInventory()
        //{
        //    try
        //    {
        //        var form = await HttpContext.Request.ReadFormAsync();
        //        if (form == null)
        //        {
        //            return BadRequest("No form data received");
        //        }

        //        var sId = Convert.ToInt32(form["Id"]);
        //        var sBussCode = Request.Cookies["IMSWebAppCurrCompany"]?.ToString();
        //        var sPlantCode = Request.Cookies["IMSWebAppCurrBranch"]?.ToString();
        //        var sInvCode = form["InvCode"];
        //        var sInvName = form["InvName"];
        //        var sRelasiCode = form["RelasiCode"];
        //        var sCSINVName = form["CSINVName"];
        //        var sInvStatus = form["InvStatus"] == "on" ? true : false;
        //        var sDiscontinue = form["Discontinue"] == "on" ? true : false;
        //        var sVendorCode = form["VendorCode"];
        //        var sBarcode = form["Barcode"];
        //        var sInvType = form["InvType"];
        //        var sInvSubType = form["InvSubType"];
        //        var sBrand = form["Brand"];
        //        var sSmallUnit = form["SmallUnit"];
        //        var sLargeUnit = form["LargeUnit"];
        //        var sCrt = Convert.ToDecimal(form["Crt"]);
        //        var sFra = Convert.ToDecimal(form["Fra"]);
        //        var sNorm = Convert.ToDecimal(form["Norm"]);
        //        var sProcess = Convert.ToInt32(form["Process"] == "" ? 0 : form["Process"]);
        //        var sNoMachine = Convert.ToDecimal(string.IsNullOrEmpty(form["sNoMachine"]) ? 0 : form["sNoMachine"]);
        //        var sPeople = Convert.ToDecimal(form["People"] == "" ? 0 : form["People"]);
        //        var sSalesPrice = Convert.ToDecimal(form["SalesPrice"]);
        //        var sBuyPrice = Convert.ToDecimal(form["BuyPrice"]);
        //        var sInsertUser = CF.DecryptString(_configuration["EKey"], HttpContext.User.FindFirstValue("userName"));


        //        var inventory = new InventoryList
        //        {
        //            Id = sId,
        //            BussCode = sBussCode,
        //            PlantCode = sPlantCode,
        //            InvCode = sInvCode,
        //            InvName = sInvName,
        //            RelasiCode = sRelasiCode,
        //            CSINVName = sCSINVName,
        //            InvStatus = sInvStatus,
        //            Discontinue = sDiscontinue,
        //            VendorCode = sVendorCode,
        //            VendorName = "",
        //            Barcode = sBarcode,
        //            InvType = sInvType,
        //            InvTypeDesc = "",
        //            InvSubType = sInvSubType,
        //            InvSubTypeDesc = "",
        //            Brand = sBrand,
        //            BrandDesc = "",
        //            SmallUnit = sSmallUnit,
        //            LargeUnit = sLargeUnit,
        //            Crt = sCrt,
        //            Fra = sFra,
        //            Norm = sNorm,
        //            Process = sProcess,
        //            NoMachine = sNoMachine,
        //            People = sPeople,
        //            CodeBOM = 0,
        //            SalesPrice = sSalesPrice,
        //            BuyPrice = sBuyPrice,
        //            InsertUser = sInsertUser
        //        };

        //        var json = JsonConvert.SerializeObject(inventory);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //        var client = _httpClientFactory.CreateClient();
        //        string apiKey = _configuration["ApiKey"];
        //        client.DefaultRequestHeaders.Add("ApiKey", apiKey);
        //        string apiUrl = _configuration["ApiEndpoint"] + _configuration["InventoryInsertEndpoint"];

        //        HttpResponseMessage response = await client.PutAsync(apiUrl, content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return Ok(new { message = "Inventory added successfully.!" });
        //        }
        //        else
        //        {
        //            var errorResponse = await response.Content.ReadAsStringAsync();
        //            return StatusCode((int)response.StatusCode, errorResponse);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception if needed
        //        return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
        //    }
        //}

    }
}
