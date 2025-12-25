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
    public class MachineController : Controller
    {
        private readonly ILogger<MachineController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public MachineController(ILogger<MachineController> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        [Authorize]
        [HttpGet]
        [Route("/MasterData/Machine")]
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
        [Route("MasterData/Machine/GetData")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetData(DataSourceLoadOptions loadOptions)
        {
            try
            {
                string dataVal = HttpContext.Request.Headers["Options"];

                if (string.IsNullOrEmpty(dataVal))
                {
                    return StatusCode(200, "Cannot get Machine Options is NULL");
                }

                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["MachinesGetListEndpoint"];

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
                    List<TMMachine> dataList = JsonConvert.DeserializeObject<List<TMMachine>>(responseContent);
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
        [Route("MasterData/Machine/DetailById")]
        public async Task<IActionResult> Details(int id, DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["MachinesByIDEndpoint"];

                var json = JsonConvert.SerializeObject(id);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var machineList = JsonConvert.DeserializeObject<List<TMMachine>>(responseContent);
                    var machines = machineList.FirstOrDefault(i => i.Id == id);
                    return PartialView("_MachineDetail", machines);
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
        [Route("MasterData/Machine/Edit")]
        public async Task<IActionResult> EditMachnes(int id, DataSourceLoadOptions loadOptions)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["MachinesByIDEndpoint"];

                var json = JsonConvert.SerializeObject(id);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var machineList = JsonConvert.DeserializeObject<List<TMMachine>>(responseContent);
                    var machines = machineList.FirstOrDefault(i => i.Id == id);
                    return PartialView("_MachineEdit", machines);
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
        [Route("MasterData/Machine/Add")]
        public async Task<IActionResult> AddMachine(int id, DataSourceLoadOptions loadOptions)
        {
            try
            {
                return PartialView("_MachineAdd");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("MasterData/Machine/Update")]
        public async Task<IActionResult> UpdMachine()
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
                var sMachineCode = form["MachineCode"];
                var sMachineName = form["MachineName"];
                var sStatus = form["Status"] == "on" ? true : false;

                var sByDate = form["BuyDate"];
                DateTime? buyDate = null;
                if (!string.IsNullOrEmpty(sByDate))
                {
                    DateTime parsedDate;
                    if (DateTime.TryParse(sByDate, out parsedDate))
                    {
                        buyDate = parsedDate;
                    }
                }
                var sBuyDate = buyDate;

                var sMntDate = form["MaintDate"];
                DateTime? mntDate = null;
                if (!string.IsNullOrEmpty(sMntDate))
                {
                    DateTime parsedDate;
                    if (DateTime.TryParse(sMntDate, out parsedDate))
                    {
                        mntDate = parsedDate;
                    }
                }
                var sMaintDate = mntDate;

                var sUsage = Convert.ToDecimal(form["Usage"]);
                var sInsertUser = CF.DecryptString(_configuration["EKey"], HttpContext.User.FindFirstValue("userName"));


                var machine = new TMMachine
                {
                    Id = sId,
                    BussCode = sBussCode,
                    PlantCode = sPlantCode,
                    MachineCode = sMachineCode,
                    MachineName = sMachineName,
                    Status = sStatus,
                    BuyDate = sBuyDate,
                    MaintDate = sMaintDate,
                    Usage = sUsage,
                    InsertUser = sInsertUser,
                };

                var json = JsonConvert.SerializeObject(machine);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["MachinesUpdateEndpoint"];

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
        [Route("Masterdata/Machine/Insert")]
        public async Task<IActionResult> InsMachine()
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
                var sMachineCode = form["MachineCode"];
                var sMachineName = form["MachineName"];
                var sStatus = form["Status"] == "on" ? true : false;
                var sByDate = form["BuyDate"];
                DateTime? buyDate = null;
                if (!string.IsNullOrEmpty(sByDate))
                {
                    DateTime parsedDate;
                    if (DateTime.TryParse(sByDate, out parsedDate))
                    {
                        buyDate = parsedDate;
                    }
                }
                var sBuyDate = buyDate;
                var sMntDate = form["MaintDate"];
                DateTime? mntDate = null;
                if (!string.IsNullOrEmpty(sMntDate))
                {
                    DateTime parsedDate;
                    if (DateTime.TryParse(sMntDate, out parsedDate))
                    {
                        mntDate = parsedDate;
                    }
                }
                var sMaintDate = mntDate;
                var sUsage = form["Usage"] == "" ? 0 : Convert.ToDecimal(form["Usage"]); 
                var sInsertUser = CF.DecryptString(_configuration["EKey"], HttpContext.User.FindFirstValue("userName"));

                var machine = new TMMachine
                {
                    Id = sId,
                    BussCode = sBussCode,
                    PlantCode = sPlantCode,
                    MachineCode = sMachineCode,
                    MachineName = sMachineName,
                    Status = sStatus,
                    BuyDate = sBuyDate,
                    MaintDate = sMaintDate,
                    Usage = sUsage,
                    InsertUser = sInsertUser,
                };

                var json = JsonConvert.SerializeObject(machine);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient();
                string apiKey = _configuration["ApiKey"];
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                string apiUrl = _configuration["ApiEndpoint"] + _configuration["MachinesInsertEndpoint"];

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { message = "Machine added successfully.!" });
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
