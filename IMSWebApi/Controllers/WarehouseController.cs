using IMSWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IMSWebApi.Controllers
{
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly SqlConnection? _connection;
        public WarehouseController(SqlConnection connection)
        {
            _connection = connection;
        }

        [Route("/[controller]/getDataWarehouse")]
        [HttpPost]
        public async Task<IActionResult> GetDataWarehouse([FromBody] SPParameters parUsername)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spmGetWarehouse", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BussCode", parUsername.ACTIVEBRANCH);
                    command.Parameters.AddWithValue("@paraSP", parUsername.DATA);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<TMWarehouse>();
                        var properties = typeof(TMWarehouse).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            TMWarehouse item = new();
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

        [Route("/[controller]/getDataWarehouseByID")]
        [HttpPost]
        public async Task<IActionResult> GetDataWarehouseByID([FromBody] int idwarehouse)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spmGetWarehouseByID", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", (object)idwarehouse);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<TMWarehouse>();
                        var properties = typeof(TMWarehouse).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            TMWarehouse item = new();
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
                return StatusCode(500, "Can't Load GetDataWarehouseByID. " + ex.Message);
            }
        }

        [Route("/[controller]/insertWarehouse")]
        [HttpPost]
        public async Task<IActionResult> InsertWarehouse([FromBody] TMWarehouse salesman)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmInsertWarehouse", _connection))
            {

                command.Parameters.AddWithValue("@BussCode", (object)salesman.BussCode ?? DBNull.Value);
                command.Parameters.AddWithValue("@PlantCode", (object)salesman.PlantCode ?? DBNull.Value);
                command.Parameters.AddWithValue("@WhCode", (object)salesman.WhCode ?? DBNull.Value);
                command.Parameters.AddWithValue("@WhName", (object)salesman.WhName ?? DBNull.Value);
                command.Parameters.AddWithValue("@WhAddress", (object)salesman.WhAddress ?? DBNull.Value);
                command.Parameters.AddWithValue("@City", (object)salesman.City ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", (object)salesman.Status ?? DBNull.Value);
                command.Parameters.AddWithValue("@M3Size", (object)salesman.M3Size ?? DBNull.Value);
                command.Parameters.AddWithValue("@CRTSize", (object)salesman.CRTSize ?? DBNull.Value);
                command.Parameters.AddWithValue("@Stock", (object)salesman.Stock ?? DBNull.Value);
                command.Parameters.AddWithValue("@InsertUser", (object)salesman.InsertUser ?? DBNull.Value);
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

        [Route("/[controller]/updateWarehouse")]
        [HttpPut]
        public async Task<IActionResult> UpdateWarehouse([FromBody] TMWarehouse salesman)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmUpdateWarehouse", _connection))
            {
                command.Parameters.AddWithValue("@Id", salesman.Id);
                command.Parameters.AddWithValue("@BussCode", (object)salesman.BussCode ?? DBNull.Value);
                command.Parameters.AddWithValue("@PlantCode", (object)salesman.PlantCode ?? DBNull.Value);
                command.Parameters.AddWithValue("@WhCode", (object)salesman.WhCode ?? DBNull.Value);
                command.Parameters.AddWithValue("@WhName", (object)salesman.WhName ?? DBNull.Value);
                command.Parameters.AddWithValue("@WhAddress", (object)salesman.WhAddress ?? DBNull.Value);
                command.Parameters.AddWithValue("@City", (object)salesman.City ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", (object)salesman.Status ?? DBNull.Value);
                command.Parameters.AddWithValue("@M3Size", (object)salesman.M3Size ?? DBNull.Value);
                command.Parameters.AddWithValue("@CRTSize", (object)salesman.CRTSize ?? DBNull.Value);
                command.Parameters.AddWithValue("@Stock", (object)salesman.Stock ?? DBNull.Value);
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
                    return StatusCode(500, "Error updating Warehouse: " + ex.Message);
                }
            }
        }

        [Route("/[controller]/deleteWarehouse")]
        [HttpDelete]
        public async Task<IActionResult> DeleteWarehouse([FromQuery] int idwarehouse)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmDeleteWarehouse", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", idwarehouse);
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
                    return StatusCode(500, "Error Deleting  Warehouse: " + ex.Message);
                }
            }
        }
    }
}
