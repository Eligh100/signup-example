using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using signup_api.Models;

namespace signup_api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController()
        {
        }

        // POST: api/user
        [HttpPost]
        public ActionResult<Response> AddUser(UserConfig newUser)
        {
            if (newUser != null)
            {
                string connectionString = "Data Source=(local);Initial Catalog=Northwind;" + "Integrated Security=true";

                string queryString = "SELECT * FROM Users";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Create the Command and Parameter objects.
                    SqlCommand command = new SqlCommand(queryString, connection);
                    //command.Parameters.AddWithValue("@pricePoint", paramValue);

                    // Open the connection in a try/catch block.
                    // Create and execute the DataReader, writing the result
                    // set to the console window.
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Console.WriteLine("\t{0}\t{1}\t{2}",
                                reader[0], reader[1], reader[2]);
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    Console.ReadLine();
                }

                return new Response
                {
                    StatusCode = 200,
                    ResponseMessage = "SUCCESS"
                };
            } else
            {
                return new Response
                {
                    StatusCode = 400,
                    ResponseMessage = "Malformed user details"
                };
            }
        } 

    }
}
