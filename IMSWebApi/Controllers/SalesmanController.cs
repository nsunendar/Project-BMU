using IMSWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace IMSWebApi.Controllers
{
    [ApiController]
    public class SalesmanController : ControllerBase
    {

        private readonly SqlConnection? _connection;
        public SalesmanController(SqlConnection connection)
        {
            _connection = connection;
        }

        [Route("/[controller]/getDataSalesman")]
        [HttpPost]
        public async Task<IActionResult> GetDataSalesman([FromBody] SPParameters parUsername)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spmGetSalesman", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BussCode", parUsername.ACTIVEBRANCH);
                    command.Parameters.AddWithValue("@paraSP", parUsername.DATA);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<TMSalesman>();
                        var properties = typeof(TMSalesman).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            TMSalesman item = new();
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
                return StatusCode(500, "Can't Load GetDataSalesman. " + ex.Message);
            }
        }

        [Route("/[controller]/getDataSalesmanByID")]
        [HttpPost]
        public async Task<IActionResult> GetDataSalesmanByID([FromBody] string idsalesman)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spmGetSalesmanByID", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", idsalesman);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<TMSalesman>();
                        var properties = typeof(TMSalesman).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            TMSalesman item = new();
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

        [Route("/[controller]/insertSalesman")]
        [HttpPost]
        public async Task<IActionResult> InsertPeople([FromBody] TMSalesman salesman)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmInsertSalesman", _connection))
            {
                command.Parameters.AddWithValue("@BussCode", salesman.BussCode);
                command.Parameters.AddWithValue("@PlantCode", salesman.PlantCode);
                command.Parameters.AddWithValue("@SlsCode", salesman.SlsCode);
                command.Parameters.AddWithValue("@SlsName", salesman.SlsName);
                command.Parameters.AddWithValue("@Status", salesman.Status);
                command.Parameters.AddWithValue("@Phone", salesman.Phone);
                command.Parameters.AddWithValue("@Email", salesman.Email);
                command.Parameters.AddWithValue("@Supervisi", salesman.Supervisi);
                command.Parameters.AddWithValue("@SubType", salesman.SubType);
                command.Parameters.AddWithValue("@Area", salesman.Area);
                command.Parameters.AddWithValue("@InsertUser", salesman.InsertUser);
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
                    return StatusCode(500, "Error updating Insert Salesman: " + ex.Message);
                }
            }
        }

        [Route("/[controller]/updateSalesman")]
        [HttpPut]
        public async Task<IActionResult> UpdatePeople([FromBody] TMSalesman salesman)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmUpdateSalesman", _connection))
            {
                command.Parameters.AddWithValue("@Id", salesman.Id);
                command.Parameters.AddWithValue("@BussCode", salesman.BussCode);
                command.Parameters.AddWithValue("@PlantCode", salesman.PlantCode);
                command.Parameters.AddWithValue("@SlsCode", salesman.SlsCode);
                command.Parameters.AddWithValue("@SlsName", salesman.SlsName);
                command.Parameters.AddWithValue("@Status", salesman.Status);
                command.Parameters.AddWithValue("@Phone", salesman.Phone);
                command.Parameters.AddWithValue("@Email", salesman.Email);
                command.Parameters.AddWithValue("@Supervisi", salesman.Supervisi);
                command.Parameters.AddWithValue("@SubType", salesman.SubType);
                command.Parameters.AddWithValue("@Area", salesman.Area);
                command.Parameters.AddWithValue("@InsertUser", salesman.InsertUser);
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

        [Route("/[controller]/deleteSalesman")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSalesman([FromQuery] int idsalesman)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmDeleteWarehouse", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", idsalesman);
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
