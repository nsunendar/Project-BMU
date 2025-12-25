using Dapper;
using IMSWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IMSWebApi.Controllers
{
    [ApiController]
    public class BOMController : ControllerBase
    {
        private readonly SqlConnection? _connection;

        public BOMController(SqlConnection connection)
        {
            _connection = connection;
        }

        [Route("/[controller]/getDataBOMInventory")]
        [HttpPost]
        public async Task<IActionResult> GetDataBOMInventory([FromBody] SPParameters parUsername)
        {
            try
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("spmGetBOMInventory", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BussCode", parUsername.ACTIVEBRANCH);
                    command.Parameters.AddWithValue("@paraSP", parUsername.DATA);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<BOMHItem>();
                        var properties = typeof(BOMHItem).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            BOMHItem item = new();
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

        [Route("/[controller]/getDataBOMInventoryById")]
        [HttpPost]
        public async Task<IActionResult> GetDataBOMInventoryById([FromBody] int id)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spmGetBOMInventoryByID", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<BOMItem>();
                        var properties = typeof(BOMItem).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            BOMItem item = new();
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

        [Route("/[controller]/getDataBOMMaterialById")]
        [HttpPost]
        public async Task<IActionResult> GetDataBOMMaterialById([FromBody] SPParameters parUsername)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spmGetBOMMaterialByInv", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BussCode", parUsername.ACTIVEBRANCH);
                    command.Parameters.AddWithValue("@paraSP", parUsername.DATA);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<BOMMaterial>();
                        var properties = typeof(BOMMaterial).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            BOMMaterial item = new();
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
                return StatusCode(500, "Can't Load GetDataBOMMaterialById" + ex.Message);
            }
        }

        [Route("/[controller]/getDataBOMMaterialByCode")]
        [HttpPost]
        public async Task<IActionResult> GetDataBOMMaterialByCode([FromBody] SPParameters parUsername)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("spmGetBOMMaterialByCode", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BussCode", parUsername.ACTIVEBRANCH);
                    command.Parameters.AddWithValue("@paraSP", parUsername.DATA);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<BOMMaterial>();
                        var properties = typeof(BOMMaterial).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            BOMMaterial item = new();
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
                return StatusCode(500, "Can't Load GetDataBOMMaterialByCode" + ex.Message);
            }
        }

        [Route("/[controller]/getBOMLevelFG")]
        [HttpGet]
        public async Task<IActionResult> GetBOMLevelFG()
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("select Id, BussCode,PlantCode,ItemCode,ItemName,Status,QtyUsage,Satuan,LevelSeqn,ParentId,ParentItem from TRBOMItems where ParentItem is null", _connection))
                {
                    command.CommandType = CommandType.Text;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<BOMItem>();
                        var properties = typeof(BOMItem).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            BOMItem item = new();
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
                return StatusCode(500, "Can't Load GetBOMLevelFG " + ex.Message);
            }
        }

        [Route("/[controller]/insertLevelFG")]
        [HttpPost]
        public async Task<IActionResult> InsertLevelFG([FromBody] BOMItem bomds)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmInsertBOM", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", bomds.Id);
                command.Parameters.AddWithValue("@BussCode", bomds.BussCode);
                command.Parameters.AddWithValue("@PlantCode", bomds.PlantCode);
                command.Parameters.AddWithValue("@ItemCode", bomds.ItemCode);
                command.Parameters.AddWithValue("@ItemName", (object?)bomds.ItemName ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", bomds.Status);
                command.Parameters.AddWithValue("@QtyUsage", (object?)bomds.QtyUsage ?? DBNull.Value);
                command.Parameters.AddWithValue("@Satuan", (object?)bomds.Satuan ?? DBNull.Value);
                command.Parameters.AddWithValue("@LevelSeqn", bomds.LevelSeqn);
                command.Parameters.AddWithValue("@ParentId", (object?)bomds.ParentId ?? DBNull.Value);
                command.Parameters.AddWithValue("@ParentItem", (object?)bomds.ParentItem ?? DBNull.Value);
                //command.Parameters.AddWithValue("@InsertUser", HttpContext.User.Identity?.Name ?? "system");
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

        [Route("/[controller]/insertBOMHeader")]
        [HttpPost]
        public async Task<IActionResult>InsertBOMHeader([FromBody] BOMItem bomds)
        {

            if (bomds == null) return BadRequest("Payload kosong.");
            if (string.IsNullOrWhiteSpace(bomds.ItemCode)) return BadRequest("ItemCode wajib diisi.");

            try
            {
                using var command = new SqlCommand("sprInsertBOMHeader", _connection);
                command.CommandType = CommandType.StoredProcedure;

                // JANGAN kirim @Id untuk insert identity
                command.Parameters.AddWithValue("@BussCode", (object?)bomds.BussCode ?? DBNull.Value);
                command.Parameters.AddWithValue("@PlantCode", (object?)bomds.PlantCode ?? DBNull.Value);
                command.Parameters.AddWithValue("@ItemCode", bomds.ItemCode);
                command.Parameters.AddWithValue("@ItemName", (object?)bomds.ItemName ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", bomds.Status);
                command.Parameters.AddWithValue("@QtyUsage", (object?)bomds.QtyUsage ?? DBNull.Value);
                command.Parameters.AddWithValue("@Satuan", (object?)bomds.Satuan ?? DBNull.Value);
                command.Parameters.AddWithValue("@LevelSeqn", bomds.LevelSeqn);
                command.Parameters.AddWithValue("@ParentId", (object?)bomds.ParentId ?? DBNull.Value);
                command.Parameters.AddWithValue("@ParentItem", (object?)bomds.ParentItem ?? DBNull.Value);

                var outParam = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(outParam);

                if (_connection.State != ConnectionState.Open)
                    await _connection.OpenAsync();

                await command.ExecuteNonQueryAsync();
                var newId = (int)outParam.Value;

                return Ok(new { id = newId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error inserting BOM Header: " + ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }
        }

        [Route("/[controller]/updateLevelFG")]
        [HttpPut]
        public async Task<IActionResult> UpdateLevelFG([FromBody] BOMItem bomds)
        {
            string resultMsg = string.Empty;
            int resultNum = 0;
            using (var command = new SqlCommand("spmUpdateBOM", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", bomds.Id);
                command.Parameters.AddWithValue("@BussCode", bomds.BussCode);
                command.Parameters.AddWithValue("@PlantCode", bomds.PlantCode);
                command.Parameters.AddWithValue("@ItemCode", bomds.ItemCode);
                command.Parameters.AddWithValue("@ItemName", (object?)bomds.ItemName ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", bomds.Status);
                command.Parameters.AddWithValue("@QtyUsage", (object?)bomds.QtyUsage ?? DBNull.Value);
                command.Parameters.AddWithValue("@Satuan", (object?)bomds.Satuan ?? DBNull.Value);
                command.Parameters.AddWithValue("@LevelSeqn", bomds.LevelSeqn);
                command.Parameters.AddWithValue("@ParentId", (object?)bomds.ParentId ?? DBNull.Value);
                command.Parameters.AddWithValue("@ParentItem", (object?)bomds.ParentItem ?? DBNull.Value);
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

        [Route("/[controller]/getBOMLevelRaw/{parentItem}")]
        [HttpGet]
        public async Task<IActionResult> GetBOMLevelRaw(string parentItem)
        {
            try
            {
                await _connection.OpenAsync();

                using (var command = new SqlCommand("select Id,BussCode,PlantCode,ItemCode,ItemName,Status,QtyUsage,Satuan,LevelSeqn,ParentId,ParentItem from TRBOMItems where ParentItem=@para", _connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@para", parentItem);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var itemList = new List<BOMItem>();
                        var properties = typeof(BOMItem).GetProperties();
                        while (await reader.ReadAsync())
                        {
                            BOMItem item = new();
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
                return StatusCode(500, "Can't Load GetBOMLevelRaw " + ex.Message);
            }
        }

        [Route("/[controller]/SearchInventoryFG/{searchitem}")]
        [HttpGet]
        public async Task<IActionResult> SearchInventoryFG([FromRoute] string searchitem)
        {
            try
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("select InvCode as Value, InvName as Text from TMInventory where InvType='01' and isnull(InvName,'')+isnull(InvCode,'') like @para", _connection))
                {
                    command.CommandType = CommandType.Text;
                    var par = string.Concat("%", searchitem, "%");
                    command.Parameters.AddWithValue("@para", par);
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
                return StatusCode(500, "Can't Load GetBOMLevelRaw " + ex.Message);
            }
        }

        [Route("/[controller]/SearchInventoryRM/{searchitem}")]
        [HttpGet]
        public async Task<IActionResult> SearchInventoryRM([FromRoute] string searchitem)
        {
            try
            {
                await _connection.OpenAsync();
                using (var command = new SqlCommand("select InvCode as Value, InvName as Text from TMInventory where InvType<>'01' and isnull(InvName,'')+isnull(InvCode,'') like @para", _connection))
                {
                    command.CommandType = CommandType.Text;
                    var par = string.Concat("%", searchitem, "%");
                    command.Parameters.AddWithValue("@para", par);
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
                return StatusCode(500, "Can't Load GetBOMLevelRaw " + ex.Message);
            }
        }

        //[Route("/[controller]/getBOMLevelCP")]
        //[HttpGet]
        //public async Task<IActionResult> GetBOMLevelCP([FromQuery] string parentItem)
        //{
        //    try
        //    {
        //        await _connection.OpenAsync();

        //        using (var command = new SqlCommand("select * from TRBOMItems where ParentItem=@para", _connection))
        //        {
        //            command.CommandType = CommandType.Text;
        //            command.Parameters.AddWithValue("@para", parentItem);
        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                var itemList = new List<BOMMaterial>();
        //                var properties = typeof(BOMMaterial).GetProperties();
        //                while (await reader.ReadAsync())
        //                {
        //                    BOMMaterial item = new();
        //                    foreach (var property in properties)
        //                    {
        //                        if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
        //                        {
        //                            var value = reader[property.Name];

        //                            if (value == DBNull.Value && Nullable.GetUnderlyingType(property.PropertyType) != null)
        //                            {
        //                                property.SetValue(item, null);
        //                            }
        //                            else
        //                            {
        //                                property.SetValue(item, value);
        //                            }
        //                        }
        //                    }
        //                    itemList.Add(item);
        //                }
        //                await _connection.CloseAsync();
        //                return Ok(itemList);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Can't Load GetBOMLevelRaw" + ex.Message);
        //    }
        //}
    }
}
