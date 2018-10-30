using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Chinook.Models;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Chinook.DataAccess
{
    public class ChinookStorage
    {
        // saving the connection of the db to a variable
        private readonly string ConnectionString;

        public ChinookStorage(IConfiguration config)
        {
            ConnectionString = config.GetSection("ConnectionString").Value;
        }

        //Provide an endpoint that shows the invoices associated with each sales agent. The result should include the Sales Agent's full name.
        public List<SalesAgents> GetSalesAgent()
        {

            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                // using dapper
                var result = db.Query<SalesAgents>(@"select E.FirstName + ' ' + E.LastName as Name, InvoiceId
                                                 from Employee E
                                                join Customer C on E.EmployeeId = C.SupportRepId
                                                join Invoice I on C.CustomerId = I.CustomerId
                                                Group by E.FirstName, E.LastName, InvoiceId");

                return result.ToList();

                // using ADO.NET
                //var command = db.CreateCommand();
                //command.CommandText = @"Select E.FirstName + ' ' + E.LastName as Name, InvoiceId
                //                               from Employee E
                //                                join Customer C on E.EmployeeId = C.SupportRepId
                //                                join Invoice I on C.CustomerId = I.CustomerId
                //                                Group by E.FirstName, E.LastName, InvoiceId";


                //var reader = command.ExecuteReader();

                //var employees = new List<SalesAgents>();

                //while (reader.Read())
                //{
                //    var employee = new SalesAgents
                //    {
                //        InvoiceId = (int)reader["InvoiceId"],
                //        Name = reader["Name"].ToString(),
                //    };

                //    employees.Add(employee);
                //}

                //return employees;
            }

        }

        //Provide an endpoint that shows the Invoice Total, Customer name, Country and Sale Agent name for all invoices.
        public List<InvoiceLine> GetInvoiceLine()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();

                var result = db.Query<InvoiceLine>(@"Select I.Total, C.FirstName+ ' '+ C.LastName as 'CustomerName', C.Country,
                                      E.FirstName +' '+E.LastName as 'SalesAgent' 
                                       From Invoice I
                                       Join Customer C
                                       on I.CustomerId = C.CustomerId
                                       join Employee E
                                       on E.EmployeeId = C.SupportRepId");
                return result.ToList();

                //var command = db.CreateCommand();
                //command.CommandText = @"  Select I.Total, C.FirstName+ ' '+ C.LastName as 'CustomerName', C.Country,
                //                      E.FirstName +' '+E.LastName as 'SalesAgent' 
                //                       From Invoice I
                //                       Join Customer C
                //                       on I.CustomerId = C.CustomerId
                //                       join Employee E
                //                       on E.EmployeeId = C.SupportRepId";

                //var reader = command.ExecuteReader();

                //var invoiceInfo = new List<InvoiceLine>();

                //while (reader.Read())
                //{
                //    var invoice = new InvoiceLine
                //    {
                //        CustomerName = reader["CustomerName"].ToString(),
                //        SalesAgent = reader["SalesAgent"].ToString(),
                //        Total = (decimal)reader["Total"],
                //        Country = reader["Country"].ToString()
                //    };

                //    invoiceInfo.Add(invoice);
                //}
                //return invoiceInfo;
            }
        }

        //Looking at the InvoiceLine table, provide an endpoint that COUNTs the number of line items for an Invoice with a parameterized Id from user input
        public int GetCount(int invoiceid)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                var result = db.QueryFirst<int>(@"Select count(*) from InvoiceLine
                                       where InvoiceId = @id", new { id = invoiceid });

                return result;

               // var command = db.CreateCommand();
               // command.CommandText = @"/*Select count(*) from InvoiceLine*/
               //                        where InvoiceId = @id";

               // command.Parameters.AddWithValue("@id", id);

               //// var counter = command.ExecuteScalar();

               //     id = (int)command.ExecuteScalar();
            
            }
            //return id;
        }

        //Provide a new endpoint to INSERT a new invoice with parameters for customerid and billing address
        public bool AddInvoice(Invoice invoice)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                var result = db.Execute(@"INSERT INTO [dbo].[Invoice]
                                       ([CustomerId],[InvoiceDate],[BillingAddress],[Total])
                                       VALUES (@CustomerId,GetDate(),@BillingAddress,@Total)", invoice);

                return result == 1;
                
                //var command = db.CreateCommand();
                //command.CommandText = @"INSERT INTO [dbo].[Invoice]
                //                        ([CustomerId],[InvoiceDate],[BillingAddress],[Total])
                //                   VALUES (@CustomerId,GetDate(),@BillingAddress,@Total)";

                //command.Parameters.AddWithValue("@CustomerId", invoice.CustomerId);
                ////command.Parameters.AddWithValue("@InvoiceDate", invoice.InvoiceDate);
                //command.Parameters.AddWithValue("@BillingAddress", invoice.BillingAddress);
                //command.Parameters.AddWithValue("@Total", invoice.Total);

                //var result = command.ExecuteNonQuery();

                //return result == 1;
            }
        }

        //Provide a new endpoint to UPDATE an Employee's name with a parameter for Employee Id and new name
        public bool UpdateEmployee(string FirstName, string LastName, int employeeId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                var result = db.Execute(@"UPDATE[dbo].[Employee] 
                                       SET LastName = @LastName,FirstName=@FirstName
                                       where [EmployeeId] = @EmployeeId", new {employeeid = employeeId, LastName = LastName, FirstName = FirstName });

                return result == 1;
                //var command = db.CreateCommand();
                //command.CommandText = @"UPDATE[dbo].[Employee] 
                //                       SET LastName = @LastName,FirstName=@FirstName
                //                       where [EmployeeId] = @EmployeeId";
                //command.Parameters.AddWithValue("@LastName", LastName);
                //command.Parameters.AddWithValue("@FirstName", FirstName);
                //command.Parameters.AddWithValue("@EmployeeId", employeeId);

                //var result = command.ExecuteNonQuery();

                //return result == 1;

            }
        }
    }
}
