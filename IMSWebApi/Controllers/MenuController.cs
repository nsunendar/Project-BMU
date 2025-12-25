using IMSWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace IMSWebApi.Controllers
{
    //[Route("/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly SqlConnection? _connection;

        public MenuController(SqlConnection connection)
        {
            _connection = connection;
        }

        [Route("/[controller]/getModul")]
        [HttpPost]
        public async Task<IActionResult> GetMenuModul([FromBody] SPParameters parUsername)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spzGetMenuModulByUser", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Userid", parUsername.USERNAME);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<MenuModul>();
                        var properties = typeof(MenuModul).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            MenuModul item = new();
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
                return StatusCode(500, "Can't Load GetMenuModul" + ex.Message);
            }
        }

        [Route("/[controller]/getProcess")]
        [HttpPost]
        public async Task<IActionResult> GetMenuProces([FromBody] SPParameters parUsername)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spzGetMenuProcesByUser", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Userid", parUsername.USERNAME);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<MenuProces>();
                        var properties = typeof(MenuProces).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            MenuProces item = new();
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
                return StatusCode(500, "Can't Load GetMenuProces" + ex.Message);
            }
        }

        [Route("/[controller]/getProgram")]
        [HttpPost]
        public async Task<IActionResult> GetMenuProgram([FromBody] SPParameters parUsername)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spzGetMenuProgramByUser", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Userid", parUsername.USERNAME);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<MenuProgram>();
                        var properties = typeof(MenuProgram).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            MenuProgram item = new();
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
                return StatusCode(500, "Can't Load GetMenuProgram" + ex.Message);
            }
        }

        [Route("/[controller]/getMenuItem")]
        [HttpPost]
        public async Task<IActionResult> GetMenuItem([FromBody] SPParameters parUsername)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spzGetMenuItemByUserModul", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BussCode", "BMU");
                    command.Parameters.AddWithValue("@ModulCode", parUsername.DATA);
                    command.Parameters.AddWithValue("@Userid", parUsername.USERNAME);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<MenuItem>();
                        var properties = typeof(MenuItem).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            MenuItem item = new();
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
                return StatusCode(500, "Can't Load GetMenuProgram" + ex.Message);
            }
        }

        [Route("/[controller]/getMenuItemModul")]
        [HttpPost]
        public async Task<IActionResult> GetMenuItemModul([FromBody] SPParameters parUsername)
        {
            try
            {
                await _connection.OpenAsync();

                var procesList = new List<MenuList>();  // List untuk menampung semua MenuList
                var ProgramList = new List<MenuProgram>();  // (ProgramList tampaknya tidak digunakan dalam kode ini)

                using (var command = new SqlCommand("spzGetMenuItemUserModul", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BussCode", "BMU");
                    command.Parameters.AddWithValue("@ModulCode", parUsername.DATA);
                    command.Parameters.AddWithValue("@Userid", parUsername.USERNAME);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        MenuList currentProces = null;

                        // Membaca data baris per baris dari query
                        while (await reader.ReadAsync())
                        {
                            var modulCode = reader.GetString(reader.GetOrdinal("ModulCode"));
                            var procesCode = reader.GetString(reader.GetOrdinal("ProcCode"));
                            var urlProc = reader.GetString(reader.GetOrdinal("UrlProc"));
                            var procesName = reader.GetString(reader.GetOrdinal("ProcName"));
                            var progCode = reader.GetString(reader.GetOrdinal("ProgCode"));
                            var progName = reader.GetString(reader.GetOrdinal("ProgName"));
                            var urlLink = reader.GetString(reader.GetOrdinal("Url"));

                            // Memeriksa apakah proses baru ditemukan
                            if (currentProces == null || currentProces.ProcesCode != procesCode)
                            {
                                // Jika ada proses sebelumnya, tambahkan ke list
                                if (currentProces != null)
                                {
                                    procesList.Add(currentProces);
                                }

                                // Membuat objek MenuList untuk proses baru
                                currentProces = new MenuList
                                {
                                    ProcesCode = procesCode,
                                    ProcesName = procesName,
                                    ModulCode = modulCode,
                                    LinkProc = urlProc,
                                    MenuPrograms = new List<MenuListProgram>()  // List untuk menu program
                                };
                            }

                            // Jika kode program valid, tambahkan ke daftar menu program
                            if (!string.IsNullOrEmpty(progCode))
                            {
                                currentProces.MenuPrograms.Add(new MenuListProgram
                                {
                                    ProcesCode = procesCode,
                                    ProgCode = progCode,
                                    ProgName = progName,
                                    LinkUrl = urlLink,
                                });
                            }
                        }

                        // Menambahkan proses terakhir yang ditemukan ke dalam list
                        if (currentProces != null)
                        {
                            procesList.Add(currentProces);
                        }

                        await _connection.CloseAsync();

                        // Mengembalikan seluruh list dari proses yang ditemukan
                        return Ok(procesList);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Can't Load GetMenuProgram" + ex.Message);
            }
        }


        //[Route("/[controller]/getMenuItemModul")]
        //[HttpPost]
        //public async Task<IActionResult> GetMenuItemModul([FromBody] SPParameters parUsername)
        //{
        //    try
        //    {
        //        await _connection.OpenAsync();

        //        var procesList = new List<MenuList>();
        //        var ProgramList = new List<MenuProgram>();

        //        using (var command = new SqlCommand("spzGetMenuItemUserModul", _connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@BussCode", "BMU");
        //            command.Parameters.AddWithValue("@ModulCode", parUsername.DATA);
        //            command.Parameters.AddWithValue("@Userid", parUsername.USERNAME);

        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                MenuList currentProces = null;

        //                while (await reader.ReadAsync())
        //                {
        //                    var modulCode = reader.GetString(reader.GetOrdinal("ModulCode"));
        //                    var procesCode = reader.GetString(reader.GetOrdinal("ProcCode"));
        //                    var urlProc = reader.GetString(reader.GetOrdinal("UrlProc"));
        //                    var procesName = reader.GetString(reader.GetOrdinal("ProcName"));
        //                    var progCode = reader.GetString(reader.GetOrdinal("ProgCode"));
        //                    var progName = reader.GetString(reader.GetOrdinal("ProgName"));
        //                    var urlLink = reader.GetString(reader.GetOrdinal("Url"));

        //                    if (currentProces == null || currentProces.ProcesCode != procesCode)
        //                    {
        //                        if (currentProces != null)
        //                        {
        //                            procesList.Add(currentProces);
        //                        }
        //                        currentProces = new MenuList
        //                        {
        //                            ProcesCode = procesCode,
        //                            ProcesName = procesName,
        //                            ModulCode = modulCode,
        //                            LinkProc = urlProc,
        //                            MenuPrograms = new List<MenuListProgram>()
        //                        };
        //                    }

        //                    if (progCode != "")
        //                    {
        //                        currentProces.MenuPrograms.Add(new MenuListProgram
        //                        {
        //                            ProcesCode = procesCode,
        //                            ProgCode = progCode,
        //                            ProgName = progName,
        //                            LinkUrl = urlLink,
        //                        });
        //                    }
        //                }
        //                await _connection.CloseAsync();
        //                return Ok(currentProces);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Can't Load GetMenuProgram");
        //    }
        //}
    }
}
