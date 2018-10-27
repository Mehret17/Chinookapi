using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Chinook.DataAccess;
using Chinook.Models;

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
        //Provide an endpoint that shows the invoices associated with each sales agent. The result should include the Sales Agent's full name.
        [HttpGet]
        public IActionResult GetSales()
        {
            return Ok(_storage.GetSalesAgent());
        }
        //Provide an endpoint that shows the Invoice Total, Customer name, Country and Sale Agent name for all invoices.
        [HttpGet("invoiceLine")]
        public IActionResult GetInvoice()
        {
            return Ok(_storage.GetInvoiceLine());
        }
        //Looking at the InvoiceLine table, provide an endpoint that COUNTs the number of line items for an Invoice with a parameterized Id from user input
        [HttpGet("{id}")]
        public IActionResult GetId(int id)
        {
            return Ok(_storage.GetCount(id));
        }

        [HttpPut("invoice")]
        public IActionResult UpdateInvoice(Invoice invoice)
        {
            return Ok(_storage.AddInvoice(invoice));
        }

        [HttpPut("employee")]
        public IActionResult UpdateEmployee(Employee employee)
        {
            return Ok(_storage.UpdateEmployee(employee.FirstName, employee.LastName, employee.EmployeeId));
        }
    }
}

public class Employee
{
    //public string EmployeeName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int EmployeeId { get; set; }
}