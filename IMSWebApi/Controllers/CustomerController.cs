using IMSWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace IMSWebApi.Controllers
{
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly SqlConnection? _connection;

        public CustomerController(SqlConnection connection)
        {
            _connection = connection;
        }

        [Route("/[controller]/getDataCustomer")]
        [HttpPost]
        public async Task<IActionResult> GetDataCustomer([FromBody] SPParameters parUsername)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spmGetCustomer", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BussCode", parUsername.ACTIVEBRANCH);
                    command.Parameters.AddWithValue("@paraSP", parUsername.DATA);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<CustomerList>();
                        var properties = typeof(CustomerList).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            CustomerList item = new();
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
                return StatusCode(500, "Can't Load GetDataCustomer" + ex.Message);
            }
        }

        [Route("/[controller]/getCustomerById")]
        [HttpPost]
        public async Task<IActionResult> GetCustomerById([FromBody] int id)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spmGetCustomerByID", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<CustomerList>();
                        var properties = typeof(CustomerList).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            CustomerList item = new();
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

        [Route("/[controller]/insertCustomer")]
        [HttpPost]
        public async Task<IActionResult> InsertCustomer([FromBody] CustomerList customerList)
        {
            string? resultMsg = string.Empty;
            int resultNum = 0;

            using (var command = new SqlCommand("spmAddCustomer", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BussCode", customerList.BussCode);
                command.Parameters.AddWithValue("@PlantCode", customerList.PlantCode);
                command.Parameters.AddWithValue("@CustCode", customerList.CustCode);
                command.Parameters.AddWithValue("@CustName", customerList.CustName);
                command.Parameters.AddWithValue("@CustAddress", customerList.CustAddress);
                command.Parameters.AddWithValue("@City", customerList.City);
                command.Parameters.AddWithValue("@Status", customerList.Status);
                command.Parameters.AddWithValue("@Phone", customerList.Phone);
                command.Parameters.AddWithValue("@Email", customerList.Email);
                command.Parameters.AddWithValue("@OwnerName", customerList.OwnerName);
                command.Parameters.AddWithValue("@PKP", customerList.PKP);
                command.Parameters.AddWithValue("@TaxName", customerList.TaxName);
                command.Parameters.AddWithValue("@TaxAddress", customerList.TaxAddress);
                command.Parameters.AddWithValue("@TaxCity", customerList.TaxCity);
                command.Parameters.AddWithValue("@NPWP", customerList.NPWP);
                command.Parameters.AddWithValue("@CustType", customerList.CustType);
                command.Parameters.AddWithValue("@CustSubType", customerList.CustSubType);
                command.Parameters.AddWithValue("@Area", customerList.Area);
                command.Parameters.AddWithValue("@Salesman", customerList.Salesman);
                command.Parameters.AddWithValue("@Term", customerList.Term);
                command.Parameters.AddWithValue("@PriceCode", customerList.PriceCode);
                if (customerList.JoinDate.HasValue)
                {
                    command.Parameters.AddWithValue("@JoinDate", customerList.JoinDate.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@JoinDate", DBNull.Value);
                };
                command.Parameters.AddWithValue("@InsertUser", customerList.InsertUser);
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
                    command.ExecuteNonQuery();

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
                    return StatusCode(500, "Error Add Customer" + ex.Message);
                }
            }
        }

        [Route("/[controller]/updateCustomer")]
        [HttpPut]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerList cust)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;

            // Menyiapkan SQL command untuk stored procedure
            using (var command = new SqlCommand("spmUpdateCustomer", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Menambahkan parameter untuk stored procedure
                command.Parameters.AddWithValue("@Id", cust.Id);
                command.Parameters.AddWithValue("@BussCode", cust.BussCode);
                command.Parameters.AddWithValue("@PlantCode", cust.PlantCode);
                command.Parameters.AddWithValue("@CustCode", cust.CustCode);
                command.Parameters.AddWithValue("@CustName", cust.CustName);
                command.Parameters.AddWithValue("@CustAddress", cust.CustAddress);
                command.Parameters.AddWithValue("@City", cust.City);
                command.Parameters.AddWithValue("@Status", cust.Status);
                command.Parameters.AddWithValue("@Phone", cust.Phone);
                command.Parameters.AddWithValue("@Email", cust.Email);
                command.Parameters.AddWithValue("@OwnerName", cust.OwnerName);
                command.Parameters.AddWithValue("@PKP", cust.PKP);
                command.Parameters.AddWithValue("@TaxName", cust.TaxName);
                command.Parameters.AddWithValue("@TaxAddress", cust.TaxAddress);
                command.Parameters.AddWithValue("@TaxCity", cust.TaxCity);
                command.Parameters.AddWithValue("@NPWP", cust.NPWP);
                command.Parameters.AddWithValue("@CustType", cust.CustType);
                command.Parameters.AddWithValue("@CustSubType", cust.CustSubType);
                command.Parameters.AddWithValue("@Area", cust.Area);
                command.Parameters.AddWithValue("@Salesman", cust.Salesman);
                command.Parameters.AddWithValue("@Term", cust.Term);
                command.Parameters.AddWithValue("@PriceCode", cust.PriceCode);
                if (cust.JoinDate.HasValue)
                {
                    command.Parameters.AddWithValue("@JoinDate", cust.JoinDate.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@JoinDate", DBNull.Value);
                };
                command.Parameters.AddWithValue("@InsertUser", cust.InsertUser);

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


        
        //"GetCustTypeEndpoint": "Customer/getCustType",
        [Route("/[controller]/getCustTypes")]
        [HttpGet]
        public async Task<IActionResult> GetCustTypes()
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("SELECT CsType as Value, CsTypeDesc as Text FROM TMCustomerType", _connection))
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
                return StatusCode(500, "Can't Load getCustType" + ex.Message);
            }
        }

        //"GetCustSubTypeEndpoint": "Customer/getCustSubType",
        [Route("/[controller]/getCustSubTypes")]
        [HttpGet]
        public async Task<IActionResult> GetCustSubTypes(string cstype = "")
        {
            string dataVal = HttpContext.Request.Headers["Options"];

            try
            {
                // Cek jika invtype kosong atau null
                await _connection.OpenAsync();
                using (var command = new SqlCommand("SELECT CsSubType as Value, CsSubTypeDesc as Text FROM TMCustomerSubType WHERE CsType LIKE @intype", _connection))
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
                return StatusCode(500, "Can't Load getCustSubType" + ex.Message);
            }
        }

        //"GetCsAreaEndpoint": "Customer/getCsArea",
        [Route("/[controller]/getCsArea")]
        [HttpGet]
        public async Task<IActionResult> GetCsArea()
        {
            try
            {
                // Cek jika invtype kosong atau null
                await _connection.OpenAsync();
                using (var command = new SqlCommand("SELECT CsArea as Value, CsAreaDesc as Text FROM TMAreaCustomer", _connection))
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
                // Logging optional error message, change "Can't Load GetInvSubTypes" if necessary
                return StatusCode(500, "Can't Load getCsArea" + ex.Message);
            }
        }

        //"GetCsSalesmanEndpoint": "Customer/getCsSalesman",
        [Route("/[controller]/getCsSalesman")]
        [HttpGet]
        public async Task<IActionResult> GetCsSalesman()
        {
            try
            {
                // Cek jika invtype kosong atau null
                await _connection.OpenAsync();
                using (var command = new SqlCommand("SELECT SlsCode as Value, SlsName as Text FROM TMSalesman", _connection))
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
                // Logging optional error message, change "Can't Load GetInvSubTypes" if necessary
                return StatusCode(500, "Can't Load getCsSalesman" + ex.Message);
            }
        }

    }
}