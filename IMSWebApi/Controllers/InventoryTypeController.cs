using IMSWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IMSWebApi.Controllers
{
    [ApiController]
    public class InventoryTypeController : ControllerBase
    {

        private readonly SqlConnection? _connection;

        public InventoryTypeController(SqlConnection connection)
        {
            _connection = connection;
        }

        [Route("/[controller]/getDataInventoryType")]
        [HttpGet]
        public async Task<IActionResult> GetDataInventoryType()
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM TMInvType ORDER BY InvType", _connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<TMInvType>();
                        var properties = typeof(TMInvType).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            TMInvType item = new();
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
                return StatusCode(500, "Can't Load GetDataInventoryType. " + ex.Message);
            }
        }

        [Route("/[controller]/getDataInvTypeByID")]
        [HttpPost]
        public async Task<IActionResult> GetDataInvTypeByID([FromBody] string code)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("SELECT InvType, InvTypeDesc, InsertUser FROM TMInvType WHERE InvType=@code", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@code", code);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<TMInvType>();
                        var properties = typeof(TMInvType).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            TMInvType item = new();
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
                return StatusCode(500, "Can't Load GetDataInvTypeByID. " + ex);
            }
        }

        [Route("/[controller]/getDataInvSubTypeByID")]
        [HttpPost]
        public async Task<IActionResult> GetDataInvSubTypeByID([FromBody] string code)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("SELECT InvType, InvSubType, InvSubTypeDesc, InsertUser FROM TMInvSubType WHERE InvType=@code", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@code", code);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<TMInvSubType>();
                        var properties = typeof(TMInvSubType).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            TMInvSubType item = new();
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
                return StatusCode(500, "Can't Load GetDataInvSubTypeByID. " + ex);
            }
        }

        [Route("/[controller]/getDataInventorySubType/{invtype}")]
        [HttpGet]
        public async Task<IActionResult> GetDataInventorySubType(string invtype)
        {
            try
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("SELECT InvType, InvSubType, InvSubTypeDesc, InsertUser FROM TMInvSubType WHERE InvType=@invtype  ", _connection))
                {                   
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@invtype", invtype);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<TMInvSubType>();
                        var properties = typeof(TMInvSubType).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            TMInvSubType item = new();
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
                return StatusCode(500, "Can't Load GetDataInventory " + ex.Message);
            }
        }

        [Route("/[controller]/insertType")]
        [HttpPost]
        public async Task<IActionResult> InsertInventoryType([FromBody] TMInvType invtype)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmInsertInventoryType", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InvType", invtype.InvType);
                command.Parameters.AddWithValue("@InvTypeDesc", invtype.InvTypeDesc);
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
                    return StatusCode(500, "Error updating InsertInventoryType: " + ex.Message);
                }
            }
        }

        [Route("/[controller]/insertSubtype")]
        [HttpPost]
        public async Task<IActionResult> InsertInventorySubType([FromBody] TMInvSubType invsubtype)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmInsertInventorySubType", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InvType", invsubtype.InvType);
                command.Parameters.AddWithValue("@InvSubType", invsubtype.InvSubType);
                command.Parameters.AddWithValue("@InvSubTypeDesc", invsubtype.InvSubTypeDesc);
                command.Parameters.AddWithValue("@InsertUser", invsubtype.InsertUser);
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
                    return StatusCode(500, "Error updating InsertInventorySubType: " + ex.Message);
                }
            }
        }

        [Route("/[controller]/updateType")]
        [HttpPut]
        public async Task<IActionResult> UpdateInventoryType([FromBody] TMInvType invtype)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmUpdateInventoryType", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InvType", invtype.InvType);
                command.Parameters.AddWithValue("@InvTypeDesc", invtype.InvTypeDesc);
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
                    return StatusCode(500, "Error updating InsertInventoryType: " + ex.Message);
                }
            }
        }

        [Route("/[controller]/updateSubtype")]
        [HttpPut]
        public async Task<IActionResult> UpdateInventorySubType([FromBody] TMInvSubType invsubtype)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmUpdateInventorySubType", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InvType", invsubtype.InvType);
                command.Parameters.AddWithValue("@InvSubType", invsubtype.InvSubType);
                command.Parameters.AddWithValue("@InvSubTypeDesc", invsubtype.InvSubTypeDesc);
                command.Parameters.AddWithValue("@InsertUser", invsubtype.InsertUser);
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
                    return StatusCode(500, "Error updating InsertInventorySubType: " + ex.Message);
                }
            }
        }
    }
}
