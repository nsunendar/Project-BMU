using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IMSWebApi.Controllers
{
    [ApiController]
    public class ProductionController : ControllerBase
    {
        private readonly SqlConnection? _connection;

        public ProductionController(SqlConnection connection)
        {
            _connection = connection;
        }


    }
}
