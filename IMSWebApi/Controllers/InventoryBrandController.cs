using IMSWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IMSWebApi.Controllers
{
    [ApiController]
    public class InventoryBrandController : ControllerBase
    {

        private readonly SqlConnection? _connection;

        public InventoryBrandController(SqlConnection connection)
        {
            _connection = connection;
        }

        [Route("/[controller]/getDataInventoryBrand")]
        [HttpGet]
        public async Task<IActionResult> GetDataInventoryBrand()
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("SELECT BrandCode, BrandDesc, convert(varchar(35),'') InsertUser FROM TMBrand ORDER BY BrandCode", _connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<TMBrand>();
                        var properties = typeof(TMBrand).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            TMBrand item = new();
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
                return StatusCode(500, "Can't Load GetDataInventoryBrand. " + ex.Message);
            }
        }

        [Route("/[controller]/getDataInvBrandByID")]
        [HttpPost]
        public async Task<IActionResult> GetDataInvBrandByID([FromBody] string code)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("SELECT BrandCode, BrandDesc, convert(varchar(35),'') InsertUser FROM TMBrand WHERE BrandCode=@code", _connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@code", code);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<TMBrand>();
                        var properties = typeof(TMBrand).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            TMBrand item = new();
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

        [Route("/[controller]/insertBrand")]
        [HttpPost]
        public async Task<IActionResult> InsertInventoryBrand([FromBody] TMBrand invtype)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmInsertBrand", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BrandCode", invtype.BrandCode);
                command.Parameters.AddWithValue("@BrandDesc", invtype.BrandDesc);
                command.Parameters.AddWithValue("@InsertUser", invtype.InsertUser);
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

        [Route("/[controller]/updateBrand")]
        [HttpPut]
        public async Task<IActionResult> UpdateInventoryBrand([FromBody] TMBrand invtype)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmUpdateBrand", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BrandCode", invtype.BrandCode);
                command.Parameters.AddWithValue("@BrandDesc", invtype.BrandDesc);
                command.Parameters.AddWithValue("@InsertUser", invtype.InsertUser);
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

        [Route("/[controller]/deleteBrand")]
        [HttpDelete]
        public async Task<IActionResult> DeleteInventoryBrand([FromQuery] string invtype)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmDeleteBrand", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BrandCode", invtype);
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
