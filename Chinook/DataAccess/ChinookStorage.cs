using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Chinook.Models;

namespace Chinook.DataAccess
{
    public class ChinookStorage
    {
        private const string ConnectionString = "Server=localhost;Database=Chinook;Trusted_Connection=True;";
        public List<SalesAgents> GetSalesAgent()
        {


            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                var command = db.CreateCommand();
                command.CommandText = @"Select E.FirstName + ' ' + E.LastName as Name, InvoiceId
                                               from Employee E
                                                join Customer C on E.EmployeeId = C.SupportRepId
                                                join Invoice I on C.CustomerId = I.CustomerId
                                                Group by E.FirstName, E.LastName, InvoiceId";

               // command.Parameters.AddWithValue("@id", id);

                var reader = command.ExecuteReader();

                var employees = new List<SalesAgents>();

                while (reader.Read())
                {
                    var employee = new SalesAgents
                    {
                        InvoiceId = (int)reader["InvoiceId"],
                        Name = reader["Name"].ToString(),
                    };

                    employees.Add(employee);
                }

                return employees;
            }
        }
    }
}
