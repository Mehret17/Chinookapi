using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chinook.Models
{
    public class InvoiceLine
    {
        public string CustomerName { get; set; }
        public string SalesAgent { get; set; }
        public decimal Total { get; set; }
        public string Country { get; set; }
        public int InvoiceLineId { get; set; }

    }
}
