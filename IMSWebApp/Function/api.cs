using IMSWebApp.Models;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IMSWebApp.Function
{
    public static class api
    {
        public static async Task<UserDetail> GetUserDetail(string apiUrl, string apiKey, HttpClient client, StringContent data)
        {
            UserDetail? userDetail = null;
            try
            {
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                HttpResponseMessage response = await client.PostAsync(apiUrl, data);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    userDetail = JsonConvert.DeserializeObject<UserDetail>(responseContent);
                }
            }
            catch (Exception ex)
            {
                
            }
            return userDetail;
        }

        public static async Task<List<MenuList>> GetSideMenu(string apiUrl, string apiKey, HttpClient client, StringContent data)
        {
            List<MenuList>? MenuList = null;
            try
            {
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                HttpResponseMessage response = await client.PostAsync(apiUrl, data);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MenuList = JsonConvert.DeserializeObject<List<MenuList>>(responseContent);
                }
            }
            catch (Exception ex)
            {

            }
            return MenuList;
        }

        public static async Task<List<UserBranch>> GetUserBranch(IHttpClientFactory httpClientFactory, HttpContext httpContext)
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

            string apiKey = configuration["ApiKey"];
            string apiUrl = configuration["ApiEndpoint"] + configuration["UserBranchEndpoint"];

            List<UserBranch>? BranchList = null;

            var param = new SPParameters
            {
                ACTIVEBRANCH = "BMU",
                USERNAME = CF.DecryptString(configuration["EKey"], httpContext.User.FindFirstValue("userName"))
            };
            try
            {
                var client = httpClientFactory.CreateClient();
                var json = JsonConvert.SerializeObject(param);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    BranchList = JsonConvert.DeserializeObject<List<UserBranch>>(responseContent);
                }
            }
            catch (Exception ex)
            {
                //returnData = "Error accessing API: " + ex.Message;
                //returnData = "Error accessing API";
            }
            return BranchList;
        }
    }
}