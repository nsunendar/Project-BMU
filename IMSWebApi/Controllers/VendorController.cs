using IMSWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Numerics;

namespace IMSWebApi.Controllers
{
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly SqlConnection? _connection;
        public VendorController(SqlConnection connection)
        {
            _connection = connection;
        }

        [Route("/[controller]/getDataVendor")]
        [HttpPost]
        public async Task<IActionResult> GetDataVendor([FromBody] SPParameters parUsername)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spmGetVendor", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BussCode", parUsername.ACTIVEBRANCH);
                    command.Parameters.AddWithValue("@paraSP", parUsername.DATA);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<TMVendor>();
                        var properties = typeof(TMVendor).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            TMVendor item = new();
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
                return StatusCode(500, "Can't Load GetDataVendor " + ex.Message);
            }
        }

        [Route("/[controller]/getDataVendorById")]
        [HttpPost]
        public async Task<IActionResult> GetVendorById([FromBody] int id)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spmGetVendorByID", _connection))
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
                return StatusCode(500, "Can't Load GetVendorById" + ex.Message);
            }
        }

        [Route("/[controller]/updateVendor")]
        [HttpPut]
        public async Task<IActionResult> UpdateVendor([FromBody] TMVendor vendor)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;

            // Menyiapkan SQL command untuk stored procedure
            using (var command = new SqlCommand("spmUpdateVendor", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", vendor.Id);
                command.Parameters.AddWithValue("@BussCode", vendor.BussCode ?? string.Empty);
                command.Parameters.AddWithValue("@PlantCode", vendor.PlantCode ?? string.Empty);
                command.Parameters.AddWithValue("@VendorCode", vendor.VendorCode ?? string.Empty);
                command.Parameters.AddWithValue("@VendorName", vendor.VendorName ?? string.Empty);
                command.Parameters.AddWithValue("@VendorAddress", vendor.VendorAddress ?? string.Empty);
                command.Parameters.AddWithValue("@City", vendor.City ?? string.Empty);
                command.Parameters.AddWithValue("@Status", vendor.Status ?? false);
                command.Parameters.AddWithValue("@Phone", vendor.Phone ?? string.Empty);
                command.Parameters.AddWithValue("@Email", vendor.Email ?? string.Empty);
                command.Parameters.AddWithValue("@OwnerName", vendor.OwnerName ?? string.Empty);
                command.Parameters.AddWithValue("@PKP", vendor.PKP ?? false);
                command.Parameters.AddWithValue("@TaxName", vendor.TaxName ?? string.Empty);
                command.Parameters.AddWithValue("@TaxAddress", vendor.TaxAddress ?? string.Empty);
                command.Parameters.AddWithValue("@TaxCity", vendor.TaxCity ?? string.Empty);
                command.Parameters.AddWithValue("@NPWP", vendor.NPWP ?? string.Empty);
                command.Parameters.AddWithValue("@VendorType", vendor.VendorType ?? string.Empty);
                command.Parameters.Add(new SqlParameter("@Term", SqlDbType.Decimal)
                {
                    Value = (object)vendor.Term ?? DBNull.Value,
                    Precision = 4,
                    Scale = 0
                });
                command.Parameters.Add(new SqlParameter("@LeadTime", SqlDbType.Decimal)
                {
                    Value = (object)vendor.LeadTime ?? DBNull.Value,
                    Precision = 6,
                    Scale = 0
                });
                command.Parameters.Add(new SqlParameter("@PriceCode", SqlDbType.Decimal)
                {
                    Value = (object)vendor.PriceCode ?? DBNull.Value,
                    Precision = 6,
                    Scale = 0
                });

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
                command.Parameters.AddWithValue("@InsertUser", vendor.InsertUser);

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
        public async Task<IActionResult> InsertInventory([FromBody] TMVendor vendor)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;

            // Menyiapkan SQL command untuk stored procedure
            using (var command = new SqlCommand("spmInsertVendor", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", vendor.Id);
                command.Parameters.AddWithValue("@BussCode", vendor.BussCode ?? string.Empty);
                command.Parameters.AddWithValue("@PlantCode", vendor.PlantCode ?? string.Empty);
                command.Parameters.AddWithValue("@VendorCode", vendor.VendorCode ?? string.Empty);
                command.Parameters.AddWithValue("@VendorName", vendor.VendorName ?? string.Empty);
                command.Parameters.AddWithValue("@VendorAddress", vendor.VendorAddress ?? string.Empty);
                command.Parameters.AddWithValue("@City", vendor.City ?? string.Empty);
                command.Parameters.AddWithValue("@Status", vendor.Status ?? false);
                command.Parameters.AddWithValue("@Phone", vendor.Phone ?? string.Empty);
                command.Parameters.AddWithValue("@Email", vendor.Email ?? string.Empty);
                command.Parameters.AddWithValue("@OwnerName", vendor.OwnerName ?? string.Empty);
                command.Parameters.AddWithValue("@PKP", vendor.PKP ?? false);
                command.Parameters.AddWithValue("@TaxName", vendor.TaxName ?? string.Empty);
                command.Parameters.AddWithValue("@TaxAddress", vendor.TaxAddress ?? string.Empty);
                command.Parameters.AddWithValue("@TaxCity", vendor.TaxCity ?? string.Empty);
                command.Parameters.AddWithValue("@NPWP", vendor.NPWP ?? string.Empty);
                command.Parameters.AddWithValue("@VendorType", vendor.VendorType ?? string.Empty);
                command.Parameters.Add(new SqlParameter("@Term", SqlDbType.Decimal)
                {
                    Value = (object)vendor.Term ?? DBNull.Value,
                    Precision = 4,
                    Scale = 0
                });
                command.Parameters.Add(new SqlParameter("@LeadTime", SqlDbType.Decimal)
                {
                    Value = (object)vendor.LeadTime ?? DBNull.Value,
                    Precision = 6,
                    Scale = 0
                });
                command.Parameters.Add(new SqlParameter("@PriceCode", SqlDbType.Decimal)
                {
                    Value = (object)vendor.PriceCode ?? DBNull.Value,
                    Precision = 6,
                    Scale = 0
                });         
                command.Parameters.AddWithValue("@InsertUser", vendor.InsertUser);

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

        [Route("/[controller]/getVendorTypes")]
        [HttpGet]
        public async Task<IActionResult> GetVendorTypes()
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("SELECT VndTypeCode Value,  VndTypeDesc Text FROM TMVendorType ORDER BY VndTypeCode", _connection))
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

        [Route("/[controller]/deleteVendor")]
        [HttpDelete]
        public async Task<IActionResult> DeleteVendor([FromQuery] string vendorcode)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmDeleteBrand", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BrandCode", vendorcode);
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
                    return StatusCode(500, "Error Deleting : " + ex.Message);
                }
            }
        }
    }
}
