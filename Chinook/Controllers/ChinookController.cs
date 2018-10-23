using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Chinook.DataAccess;

namespace Chinook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChinookController : ControllerBase
    {
        private readonly ChinookStorage _storage;

        public ChinookController()
        {
            _storage = new ChinookStorage();
        }

        [HttpGet]
        public IActionResult GetSales()
        {
            return Ok(_storage.GetSalesAgent());
        }
    }
}