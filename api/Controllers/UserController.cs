using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using signup_api.Models;

namespace signup_api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // POST: api/user
        [HttpPost]
        public ActionResult<Response> AddUser(UserConfig newUser)
        {
            if (newUser == null)
            {
                return new Response
                {
                    StatusCode = 400,
                    ResponseMessage = "Failed to create user"
                };
            }

            string dbPassword = System.IO.File.ReadAllText(@"SQL/db_password.txt");
            string connectionString = $"Server=localhost;Username=postgres;Database=signup_db;Port=5432;Password={dbPassword};";

            string queryString = "INSERT INTO Users (Email, PasswordHash, Salt) VALUES (@e, @p, @s)";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                Console.Out.WriteLine("Opening connection");
                connection.Open();

                using (var command = new NpgsqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("e", newUser.Email);
                    command.Parameters.AddWithValue("p", newUser.PasswordHash);
                    command.Parameters.AddWithValue("s", newUser.Salt);
                    try
                    {
                        command.ExecuteNonQuery();
                        Console.Out.WriteLine("Successfully created new user");
                    }
                    catch (Exception e)
                    {
                        using (var selectCommand = new NpgsqlCommand("SELECT * FROM Users WHERE Email=@e", connection))
                        {
                            selectCommand.Parameters.AddWithValue("e", newUser.Email);
                            NpgsqlDataReader dr = selectCommand.ExecuteReader();

                            bool emailAlreadyInTable = false;
                            // Output rows
                            while (dr.Read())
                                emailAlreadyInTable = true;

                            if (emailAlreadyInTable)
                            {
                                return new Response
                                {
                                    StatusCode = 400,
                                    ResponseMessage = "This email is already in use"
                                };
                            } else
                            {
                                return new Response
                                {
                                    StatusCode = 400,
                                    ResponseMessage = "Failed to create user"
                                };
                            }
                        }
                    }
                }
            }

            return new Response
            {
                StatusCode = 200,
                ResponseMessage = "Successfully created new user"
            };
        } 
    }
}
