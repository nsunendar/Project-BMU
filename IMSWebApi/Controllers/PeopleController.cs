using IMSWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IMSWebApi.Controllers
{
    [ApiController]
    public class PeopleController : ControllerBase
    {

        private readonly SqlConnection? _connection;
        public PeopleController(SqlConnection connection)
        {
            _connection = connection;
        }

        [Route("/[controller]/getDataPeople")]
        [HttpGet]
        public async Task<IActionResult> GetDataPeople()
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("SELECT Id, BussCode, PlantCode, PeopleCode, PeopleName, Status, Phone, Email, JoinDate, PeopleJob, PeopleGroup, InsertUser FROM TMPeople ORDER BY Id", _connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<TMPeople>();
                        var properties = typeof(TMPeople).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            TMPeople item = new();
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
                return StatusCode(500, "Can't Load GetDataPeople. " + ex.Message);
            }
        }

        [Route("/[controller]/getDataPeopleByID")]
        [HttpPost]
        public async Task<IActionResult> GetDataInvBrandByID([FromBody] string code)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("SELECT BussCode, PlantCode, PeopleCode, PeopleName, Status, Phone, Email, JoinDate, PeopleJob, PeopleGroup, InsertUser FROM TMPeople WHERE PeopleCode=@code", _connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@code", code);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<TMPeople>();
                        var properties = typeof(TMPeople).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            TMPeople item = new();
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
                return StatusCode(500, "Can't Load GetDataInvBrandByID. " + ex.Message);
            }
        }

        [Route("/[controller]/insertPeople")]
        [HttpPost]
        public async Task<IActionResult> InsertPeople([FromBody] TMPeople people)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmInsertPeople", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", people.Id);
                command.Parameters.AddWithValue("@BussCode", people.BussCode);
                command.Parameters.AddWithValue("@PlantCode", people.PlantCode);
                command.Parameters.AddWithValue("@PeopleCode", people.PeopleCode);
                command.Parameters.AddWithValue("@PeopleName", people.PeopleName);
                command.Parameters.AddWithValue("@Status", people.Status);
                command.Parameters.AddWithValue("@Phone", people.Phone);
                command.Parameters.AddWithValue("@Email", people.Email);
                command.Parameters.AddWithValue("@JoinDate", people.JoinDate);
                command.Parameters.AddWithValue("@PeopleJob", people.PeopleJob);
                command.Parameters.AddWithValue("@PeopleGroup", people.PeopleGroup);
                command.Parameters.AddWithValue("@InsertUser", people.InsertUser);
                SqlParameter resultMsgParam = new SqlParameter("@ResultMsg", SqlDbType.VarChar, 800)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(resultMsgParam);

                SqlParameter resultNumParam = new SqlParameter("@ResultNum", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(resultNumParam);

                try
                {
                    await _connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    resultMsg = resultMsgParam.Value.ToString();
                    resultNum = (int)resultNumParam.Value;

                    await _connection.CloseAsync();
                    if (resultNum == 200)
                    {
                        return Ok(resultMsg);
                    }
                    else
                    {
                        return StatusCode(resultNum, resultMsg);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Error updating Insert Brand: " + ex.Message);
                }
            }
        }

        [Route("/[controller]/updatePeople")]
        [HttpPut]
        public async Task<IActionResult> UpdatePeople([FromBody] TMPeople people)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmUpdatePeople", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", people.Id);
                command.Parameters.AddWithValue("@BussCode", people.BussCode);
                command.Parameters.AddWithValue("@PlantCode", people.PlantCode);
                command.Parameters.AddWithValue("@PeopleCode", people.PeopleCode);
                command.Parameters.AddWithValue("@PeopleName", people.PeopleName);
                command.Parameters.AddWithValue("@Status", people.Status);
                command.Parameters.AddWithValue("@Phone", people.Phone);
                command.Parameters.AddWithValue("@Email", people.Email);
                command.Parameters.AddWithValue("@JoinDate", people.JoinDate);
                command.Parameters.AddWithValue("@PeopleJob", people.PeopleJob);
                command.Parameters.AddWithValue("@PeopleGroup", people.PeopleGroup);
                command.Parameters.AddWithValue("@InsertUser", people.InsertUser);
                SqlParameter resultMsgParam = new SqlParameter("@ResultMsg", SqlDbType.VarChar, 800)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(resultMsgParam);

                SqlParameter resultNumParam = new SqlParameter("@ResultNum", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(resultNumParam);

                try
                {
                    await _connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    resultMsg = resultMsgParam.Value.ToString();
                    resultNum = (int)resultNumParam.Value;

                    await _connection.CloseAsync();
                    if (resultNum == 200)
                    {
                        return Ok(resultMsg);
                    }
                    else
                    {
                        return StatusCode(resultNum, resultMsg);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Error updating Brand: " + ex.Message);
                }
            }
        }

        [Route("/[controller]/deletePeople")]
        [HttpDelete]
        public async Task<IActionResult> DeletePeople([FromQuery] string peoplecode)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmDeletePeople", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PeopleCode", peoplecode);
                SqlParameter resultMsgParam = new SqlParameter("@ResultMsg", SqlDbType.VarChar, 800)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(resultMsgParam);

                SqlParameter resultNumParam = new SqlParameter("@ResultNum", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(resultNumParam);

                try
                {
                    await _connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    resultMsg = resultMsgParam.Value.ToString();
                    resultNum = (int)resultNumParam.Value;

                    await _connection.CloseAsync();
                    if (resultNum == 200)
                    {
                        return Ok(resultMsg);
                    }
                    else
                    {
                        return StatusCode(resultNum, resultMsg);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Error Deleting  Brand: " + ex.Message);
                }
            }
        }
    }
}
