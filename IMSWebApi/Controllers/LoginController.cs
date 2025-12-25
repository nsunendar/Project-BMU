using IMSWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;


namespace IMSWebApi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly SqlConnection? _connection;

        public LoginController(SqlConnection connection)
        {
            _connection = connection;
        }

        [Route("/login")]
        [HttpPost]
        public async Task<IActionResult>Login([FromBody] LoginModel LoginInfo)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spLogin", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", LoginInfo.Username);
                    command.Parameters.AddWithValue("@Password", LoginInfo.Password);
                    command.Parameters.Add("@Result", SqlDbType.Int).Direction = ParameterDirection.Output;
                    await command.ExecuteNonQueryAsync();
                    int result = (int)command.Parameters["@Result"].Value;

                    _connection.Close();

                    if (result == 1)
                    {
                        return Ok();
                    }
                    else
                    {
                        // Handle other cases if needed
                        return BadRequest("Login Failed, please check your username and password.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [Route("/getUserBranch")]
        [HttpPost]
        public async Task<IActionResult> GetUserBranch([FromBody] SPParameters parUsername)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spzUserBranch", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BussCode", parUsername.ACTIVEBRANCH);
                    command.Parameters.AddWithValue("@Userid", parUsername.USERNAME);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<UserBranch>();
                        var properties = typeof(UserBranch).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            UserBranch item = new();
                            foreach (var property in properties)
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                                {
                                    var value = reader[property.Name];

                                    if (value == DBNull.Value && Nullable.GetUnderlyingType(property.PropertyType) != null)
                                    {
                                        property.SetValue(item, null);
                                    }
                                    else
                                    {
                                        property.SetValue(item, value);
                                    }
                                }
                            }
                            itemList.Add(item);
                        }
                        await _connection.CloseAsync();
                        return Ok(itemList);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Can't Load GetMenuProces" + ex.Message);
            }
        }


    }
}
