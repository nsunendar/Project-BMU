using IMSWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.PortableExecutable;

namespace IMSWebApi.Controllers
{

    [ApiController]
    public class MachinesController : ControllerBase
    {
        private readonly SqlConnection? _connection;

        public MachinesController(SqlConnection connection)
        {
            _connection = connection;
        }

        [Route("/[controller]/getDataMachine")]
        [HttpPost]
        public async Task<IActionResult> GetDataMachine([FromBody] SPParameters parUsername)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spmGetMachine", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BussCode", parUsername.ACTIVEBRANCH);
                    command.Parameters.AddWithValue("@paraSP", parUsername.DATA);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<TMMachine>();
                        var properties = typeof(TMMachine).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            TMMachine item = new();
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
                return StatusCode(500, "Can't Load GetDataMachine " + ex.Message);
            }
        }

        [Route("/[controller]/getDataMachineById")]
        [HttpPost]
        public async Task<IActionResult> GetMachineById([FromBody] int id)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("SELECT Id,BussCode,PlantCode,MachineCode,MachineName,Status,BuyDate,MaintDate,Usage,InsertUser FROM TMMachine WHERE Id=@Id", _connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<TMMachine>();
                        var properties = typeof(TMMachine).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            TMMachine item = new();
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

        [Route("/[controller]/insertMachine")]
        [HttpPost]
        public async Task<IActionResult> InsertMachine([FromBody] TMMachine machine)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;

            // Menyiapkan SQL command untuk stored procedure
            using (var command = new SqlCommand("spmInsertMachine", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                // Menambahkan parameter untuk stored procedure
                command.Parameters.AddWithValue("@Id", machine.Id);
                command.Parameters.AddWithValue("@BussCode", machine.BussCode);
                command.Parameters.AddWithValue("@PlantCode", machine.PlantCode);
                command.Parameters.AddWithValue("@MachineCode", machine.MachineCode);
                command.Parameters.AddWithValue("@MachineName", machine.MachineName);
                command.Parameters.AddWithValue("@Status", machine.Status);
                command.Parameters.AddWithValue("@BuyDate", machine.BuyDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@MaintDate", machine.MaintDate ?? (object)DBNull.Value);
                command.Parameters.Add(new SqlParameter("@Usage", SqlDbType.Decimal)
                {
                    Value = (object)machine.Usage ?? DBNull.Value,
                    Precision = 12,
                    Scale = 0
                });
                command.Parameters.AddWithValue("@InsertUser", machine.InsertUser);

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

        [Route("/[controller]/updateMachine")]
        [HttpPut]
        public async Task<IActionResult> UpdateMachine([FromBody] TMMachine machine)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;

            // Menyiapkan SQL command untuk stored procedure
            using (var command = new SqlCommand("spmUpdateMachine", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Menambahkan parameter untuk stored procedure
                command.Parameters.AddWithValue("@Id", machine.Id);
                command.Parameters.AddWithValue("@BussCode", machine.BussCode);
                command.Parameters.AddWithValue("@PlantCode", machine.PlantCode);
                command.Parameters.AddWithValue("@MachineCode", machine.MachineCode);
                command.Parameters.AddWithValue("@MachineName", machine.MachineName);
                command.Parameters.AddWithValue("@Status", machine.Status);
                command.Parameters.AddWithValue("@BuyDate", machine.BuyDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@MaintDate", machine.MaintDate ?? (object)DBNull.Value);
                command.Parameters.Add(new SqlParameter("@Usage", SqlDbType.Decimal)
                {
                    Value = (object)machine.Usage ?? DBNull.Value,
                    Precision = 12,
                    Scale = 0
                });                
                command.Parameters.AddWithValue("@InsertUser", machine.InsertUser);

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
    }
}
