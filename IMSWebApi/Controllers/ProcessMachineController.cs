using IMSWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IMSWebApi.Controllers
{
    public class ProcessMachineController : Controller
    {
        private readonly SqlConnection? _connection;

        public ProcessMachineController(SqlConnection connection)
        {
            _connection = connection;
        }

        [Route("/[controller]/getDataProcess")]
        [HttpPost]
        public async Task<IActionResult> GetDataProcess([FromBody] SPParameters parUsername)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM TMProcess WHERE BussCode=@BussCode AND PlantCode=@PlantCode", _connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@BussCode", parUsername.ACTIVEBRANCH);
                    command.Parameters.AddWithValue("@PlantCode", parUsername.DATA);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<TMProcess>();
                        var properties = typeof(TMProcess).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            TMProcess item = new();
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

        [Route("/[controller]/getDataProcessById")]
        [HttpPost]
        public async Task<IActionResult> GetProcessById([FromBody] int id)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM TMProcess WHERE Id=@Id", _connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<TMProcess>();
                        var properties = typeof(TMProcess).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            TMProcess item = new();
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
                return StatusCode(500, "Can't Load GetCustomerById" + ex.Message);
            }
        }

        [Route("/[controller]/getNoMachine")]
        [HttpGet]
        public async Task<IActionResult> GetNoMachine()
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("SELECT InvType as Value, InvTypeDesc as Text FROM TMMachine", _connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<SelectedBoxValuesApi>();
                        var properties = typeof(SelectedBoxValuesApi).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            SelectedBoxValuesApi item = new();
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
                return StatusCode(500, "Can't Load GetInvTypes" + ex.Message);
            }
        }

    }
}
