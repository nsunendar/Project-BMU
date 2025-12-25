using IMSWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IMSWebApi.Controllers
{
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly SqlConnection? _connection;

        public InventoryController(SqlConnection connection)
        {
            _connection = connection;
        }

        [Route("/[controller]/getDataInventory")]
        [HttpPost]
        public async Task<IActionResult> GetDataInventory([FromBody] SPParameters parUsername)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spmGetInventory", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BussCode", parUsername.ACTIVEBRANCH);
                    command.Parameters.AddWithValue("@paraSP", parUsername.DATA);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<Inventory>();
                        var properties = typeof(Inventory).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            Inventory item = new();
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

        [Route("/[controller]/getDataInventoryById")]
        [HttpPost]
        public async Task<IActionResult> GetInventoryById([FromBody] int id)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spmGetInventoryByID", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<Inventory>();
                        var properties = typeof(Inventory).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            Inventory item = new();
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

        [Route("/[controller]/updateInventory")]
        [HttpPut]
        public async Task<IActionResult> UpdateInventory([FromBody] Inventory inventory)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;

            // Menyiapkan SQL command untuk stored procedure
            using (var command = new SqlCommand("spmUpdateInventory", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Menambahkan parameter untuk stored procedure
                command.Parameters.AddWithValue("@Id", inventory.Id);
                command.Parameters.AddWithValue("@BussCode", inventory.BussCode);
                command.Parameters.AddWithValue("@PlantCode", inventory.PlantCode);
                command.Parameters.AddWithValue("@InvCode", inventory.InvCode == null ? "" : inventory.InvCode);
                command.Parameters.AddWithValue("@InvName", inventory.InvName == null ? "" : inventory.InvName);
                command.Parameters.AddWithValue("@RelasiCode", inventory.RelasiCode == null ? "" : inventory.RelasiCode);
                command.Parameters.AddWithValue("@CSINVName", inventory.CSINVName == null ? "" : inventory.CSINVName);
                command.Parameters.AddWithValue("@InvStatus", inventory.InvStatus == null ? false : inventory.InvStatus);
                command.Parameters.AddWithValue("@Discontinue", inventory.Discontinue == null ? false : inventory.Discontinue);
                command.Parameters.AddWithValue("@VendorCode", inventory.VendorCode == null ? "" : inventory.VendorCode);
                command.Parameters.AddWithValue("@Barcode", inventory.Barcode == null ? "" : inventory.Barcode);
                command.Parameters.AddWithValue("@InvType", inventory.InvType == null ? "" : inventory.InvType);
                command.Parameters.AddWithValue("@InvSubType", inventory.InvSubType == null ? "" : inventory.InvSubType);
                command.Parameters.AddWithValue("@Brand", inventory.Brand == null ? "" : inventory.Brand);
                command.Parameters.AddWithValue("@LargeUnit", inventory.LargeUnit == null ? "" : inventory.LargeUnit);
                command.Parameters.AddWithValue("@SmallUnit", inventory.SmallUnit == null ? "" : inventory.SmallUnit);
                command.Parameters.Add(new SqlParameter("@Crt", SqlDbType.Decimal)
                {
                    Value = (object)inventory.Crt ?? DBNull.Value,
                    Precision = 4,
                    Scale = 0
                });
                command.Parameters.Add(new SqlParameter("@Fra", SqlDbType.Decimal)
                {
                    Value = (object)inventory.Fra ?? DBNull.Value,
                    Precision = 6,
                    Scale = 0
                });
                command.Parameters.Add(new SqlParameter("@Norm", SqlDbType.Decimal)
                {
                    Value = (object)inventory.Norm ?? DBNull.Value,
                    Precision = 6,
                    Scale = 0
                });
                command.Parameters.Add(new SqlParameter("@Process", SqlDbType.Int)
                {
                    Value = inventory.Process
                });
                command.Parameters.Add(new SqlParameter("@NoMachine", SqlDbType.Decimal)
                {
                    Value = (object)inventory.NoMachine ?? DBNull.Value,
                    Precision = 4,
                    Scale = 0
                });
                command.Parameters.Add(new SqlParameter("@People", SqlDbType.Decimal)
                {
                    Value = (object)inventory.People ?? DBNull.Value,
                    Precision = 4,
                    Scale = 0
                });
                command.Parameters.Add(new SqlParameter("@SalesPrice", SqlDbType.Decimal)
                {
                    Value = (object)inventory.SalesPrice ?? DBNull.Value,
                    Precision = 12,
                    Scale = 0
                });
                command.Parameters.Add(new SqlParameter("@BuyPrice", SqlDbType.Decimal)
                {
                    Value = (object)inventory.BuyPrice ?? DBNull.Value,
                    Precision = 12,
                    Scale = 0
                });
                command.Parameters.AddWithValue("@InsertUser", inventory.InsertUser);

                // Menambahkan parameter output untuk hasil
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
                    // Membuka koneksi ke database dan menjalankan stored procedure
                    await _connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    // Mengambil nilai output dari stored procedure
                    resultMsg = resultMsgParam.Value.ToString();
                    resultNum = (int)resultNumParam.Value;

                    // Menutup koneksi setelah eksekusi
                    await _connection.CloseAsync();

                    // Mengembalikan hasil sesuai dengan status code
                    if (resultNum == 200)
                    {
                        return Ok(resultMsg); // Berhasil update
                    }
                    else
                    {
                        return StatusCode(resultNum, resultMsg); // Gagal dengan error code
                    }
                }
                catch (Exception ex)
                {
                    // Jika terjadi error, kembalikan status 500 dengan pesan error
                    return StatusCode(500, "Error updating product: " + ex.Message);
                }
            }
        }

        [Route("/[controller]/insertInventory")]
        [HttpPost]
        public async Task<IActionResult> InsertInventory([FromBody] Inventory inventory)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;

            // Menyiapkan SQL command untuk stored procedure
            using (var command = new SqlCommand("spmInsertInventory", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Menambahkan parameter untuk stored procedure
                command.Parameters.AddWithValue("@Id", inventory.Id);
                command.Parameters.AddWithValue("@BussCode", inventory.BussCode);
                command.Parameters.AddWithValue("@PlantCode", inventory.PlantCode);
                command.Parameters.AddWithValue("@InvCode", inventory.InvCode);
                command.Parameters.AddWithValue("@InvName", inventory.InvName);
                command.Parameters.AddWithValue("@RelasiCode", inventory.RelasiCode);
                command.Parameters.AddWithValue("@CSINVName", inventory.CSINVName);
                command.Parameters.AddWithValue("@InvStatus", inventory.InvStatus);
                command.Parameters.AddWithValue("@Discontinue", inventory.Discontinue);
                command.Parameters.AddWithValue("@VendorCode", inventory.VendorCode);
                command.Parameters.AddWithValue("@Barcode", inventory.Barcode);
                command.Parameters.AddWithValue("@InvType", inventory.InvType);
                command.Parameters.AddWithValue("@InvSubType", inventory.InvSubType);
                command.Parameters.AddWithValue("@Brand", inventory.Brand);
                command.Parameters.AddWithValue("@LargeUnit", inventory.LargeUnit);
                command.Parameters.AddWithValue("@SmallUnit", inventory.SmallUnit);
                command.Parameters.Add(new SqlParameter("@Crt", SqlDbType.Decimal)
                {
                    Value = (object)inventory.Crt ?? DBNull.Value,
                    Precision = 4,
                    Scale = 0
                });
                command.Parameters.Add(new SqlParameter("@Fra", SqlDbType.Decimal)
                {
                    Value = (object)inventory.Fra ?? DBNull.Value,
                    Precision = 6,
                    Scale = 0
                });
                command.Parameters.Add(new SqlParameter("@Norm", SqlDbType.Decimal)
                {
                    Value = (object)inventory.Norm ?? DBNull.Value,
                    Precision = 6,
                    Scale = 0
                });
                command.Parameters.Add(new SqlParameter("@Process", SqlDbType.Int)
                {
                    Value = inventory.Process
                });
                command.Parameters.Add(new SqlParameter("@NoMachine", SqlDbType.Decimal)
                {
                    Value = (object)inventory.NoMachine ?? DBNull.Value,
                    Precision = 4,
                    Scale = 0
                });
                command.Parameters.Add(new SqlParameter("@People", SqlDbType.Decimal)
                {
                    Value = (object)inventory.People ?? DBNull.Value,
                    Precision = 4,
                    Scale = 0
                });
                command.Parameters.Add(new SqlParameter("@SalesPrice", SqlDbType.Decimal)
                {
                    Value = (object)inventory.SalesPrice ?? DBNull.Value,
                    Precision = 12,
                    Scale = 0
                });
                command.Parameters.Add(new SqlParameter("@BuyPrice", SqlDbType.Decimal)
                {
                    Value = (object)inventory.BuyPrice ?? DBNull.Value,
                    Precision = 12,
                    Scale = 0
                });
                command.Parameters.AddWithValue("@InsertUser", inventory.InsertUser);

                // Menambahkan parameter output untuk hasil
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
                    // Membuka koneksi ke database dan menjalankan stored procedure
                    await _connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    // Mengambil nilai output dari stored procedure
                    resultMsg = resultMsgParam.Value.ToString();
                    resultNum = (int)resultNumParam.Value;

                    // Menutup koneksi setelah eksekusi
                    await _connection.CloseAsync();

                    // Mengembalikan hasil sesuai dengan status code
                    if (resultNum == 200)
                    {
                        return Ok(resultMsg); // Berhasil update
                    }
                    else
                    {
                        return StatusCode(resultNum, resultMsg); // Gagal dengan error code
                    }
                }
                catch (Exception ex)
                {
                    // Jika terjadi error, kembalikan status 500 dengan pesan error
                    return StatusCode(500, "Error updating product: " + ex.Message);
                }
            }
        }

        //"GetInvTypesEndpoint": "Inventory/getInvTypes",
        [Route("/[controller]/getInvTypes")]
        [HttpGet]
        public async Task<IActionResult> GetInvTypes()
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("SELECT InvType as Value, InvTypeDesc as Text FROM TMInvType", _connection))
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

        //"GetInvSubTypesEndpoint": "Inventory/getInvSubTypes",
        [Route("/[controller]/getInvSubTypes")]
        [HttpGet]
        public async Task<IActionResult> GetInvSubTypes(string invtype = "")
        {
            string dataVal = HttpContext.Request.Headers["Options"];

            try
            {
                // Cek jika invtype kosong atau null
                var intypes = string.IsNullOrEmpty(invtype) ? "" : invtype;
                await _connection.OpenAsync();
                using (var command = new SqlCommand("SELECT InvSubType as Value, InvSubTypeDesc as Text FROM TMInvSubType WHERE InvType LIKE @intype", _connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@intype", "%" + dataVal + "%"); 
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
                // Logging optional error message, change "Can't Load GetInvSubTypes" if necessary
                return StatusCode(500, "Can't Load GetInvSubTypes" + ex.Message);
            }
        }

        //"GetVendorEndpoint": "Inventory/getVendor",
        [Route("/[controller]/getVendor")]
        [HttpGet]
        public async Task<IActionResult> GetVendor()
        {
            try
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("SELECT VendorCode as Value, VendorName as Text FROM TMVendor", _connection))
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
                return StatusCode(500, "Can't Load GetVendor" + ex.Message);
            }
        }

        //"GetBrandsEndpoint": "Inventory/getBrands",
        [Route("/[controller]/getBrands")]
        [HttpGet]
        public async Task<IActionResult> GetBrands()
        {
            try
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("SELECT BrandCode as Value, BrandDesc as Text FROM TMBrand", _connection))
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
                return StatusCode(500, "Can't Load GetBrands" + ex.Message);
            }
        }

        //"GetPeopleEndpoint": "Inventory/getPeople"
        [Route("/[controller]/getPeople")]
        [HttpGet]
        public async Task<IActionResult> GetPeople()
        {
            try
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("SELECT  Id as Value, PeopleCode+' '+PeopleName as Text FROM TMPeople", _connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<SelectedBoxValuesId>();
                        var properties = typeof(SelectedBoxValuesId).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            SelectedBoxValuesId item = new();
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
                return StatusCode(500, "Can't Load GetPeople" + ex.Message);
            }
        }

        //"GetNoMachineEndpoint": "Inventory/getNoMachine",
        [Route("/[controller]/getNoMachine")]
        [HttpGet]
        public async Task<IActionResult> GetNoMachine()
        {
            try
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("SELECT  Id as Value, MachineCode+' '+MachineName as Text FROM TMMachine", _connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<SelectedBoxValuesId>();
                        var properties = typeof(SelectedBoxValuesId).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            SelectedBoxValuesId item = new();
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
                return StatusCode(500, "Can't Load Machine" + ex.Message);
            }
        }

        //"GetProcessEndpoint": "Inventory/getProcess",
        [Route("/[controller]/getProcess")]
        [HttpGet]
        public async Task<IActionResult> GetProcess()
        {
            try
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("select Id as Value, 'Code:'+ProcCode+'|Seqn:'+convert(varchar,[NoProcess])+'|Mch:'+[CodeMachine]+'|Qty:'+convert(varchar,[QtyUsage])+'|TrgtDay:'+convert(varchar,[TarProDay]) as Text from [TMProcess] order by [ProcCode],[NoProcess]", _connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<SelectedBoxValuesId>();
                        var properties = typeof(SelectedBoxValuesId).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            SelectedBoxValuesId item = new();
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
                return StatusCode(500, "Can't Load GetProcess" + ex.Message);
            }
        }

        //"GetSatuanKecilEndpoint": "Inventory/getUnitInv",
        [Route("/[controller]/getUnitInv")]
        [HttpGet]
        public async Task<IActionResult> GetUnitInv()
        {
            try
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("SELECT UnitCode as Value, UnitDesc as Text FROM [TMInvUnit]", _connection))
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
                return StatusCode(500, "Can't Load GetPeople" + ex.Message);
            }
        }

        [Route("/[controller]/getInventoryFG")]
        [HttpGet]
        public async Task<IActionResult> GetInventoryFG()
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("SELECT Invcode as Value, InvName as Text FROM TMInventory WHERE InvType='01'", _connection))
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
                return StatusCode(500, "Can't Load GetCustomerById. " + ex.Message);
            }
        }
    }
}
